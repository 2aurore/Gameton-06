using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class CharacterBase : MonoBehaviour, IDamage
    {

        [SerializeField]  //
        private PlayerData playerData;
        public float currentHP;
        public float maxHP;
        public float currentSP;
        public float maxSP;

        public float speed;
        public float jumpForce = 5f;  // 점프 힘
        private bool isGrounded = true; // 플레이어가 바닥에 있는지 여부를 판단
        private float lastDirection = 1f; // 기본적으로 오른쪽(1) 바라보는 상태


        public Transform firePoint; // 스킬 발사 위치
        public CollisionDetector attackCollider; // 기본 공격 감지를 위한 자식 오브젝트

        public Animator animator;

        private VariableJoystick joystick;
        public Rigidbody2D rb;

        public event System.Action<float, float> OnHPChanged;
        public event System.Action<float, float> OnSPChanged;


        public void Start()
        {
            animator = GetComponent<Animator>();
            joystick = ControllerUI.Instance.joystick;
            ControllerUI.Instance.linkedCharactor = this;

            attackCollider.EnableCollider(false); // 기본 공격 Enable 비활성화

            Initialize();
        }

        public void Initialize()
        {
            // int playerIndex = PlayerPrefs.GetInt("SelectedPlayerIndex", 0);
            PlayerDataManager.Singleton.SetCurrentUserData();
            playerData = PlayerDataManager.Singleton.player;

            currentHP = maxHP = playerData.hp;
            currentSP = maxSP = playerData.mp;

            OnHPChanged?.Invoke(currentHP, maxHP);
            OnSPChanged?.Invoke(currentSP, maxSP);
        }


        // 경험치 추가 및 레벨업 처리
        public void AddExp(int amount)
        {
            bool leveledUp = PlayerDataManager.Singleton.UpdateExpericence(amount);

            if (leveledUp)
            {
                // TODO: 레벨업 시 처리할 내용 추가
                Debug.Log($"레벨업! ");
            }

            // 경험치와 변경된 데이터를 파일에 업데이트 한다.
            PlayerDataManager.Singleton.UpdatePlayerData();
        }

        public void FixedUpdate()
        {

            // 키보드 입력과 조이스틱 입력 통합
            float horizontalInput = Input.GetAxis("Horizontal");
            if (joystick != null && Mathf.Abs(joystick.Horizontal) > 0.01f)
            {
                horizontalInput = joystick.Horizontal; // 조이스틱 입력 우선
            }

            // 걷는 애니메이션 적용
            animator.SetBool("IsMoving", Mathf.Abs(horizontalInput) > 0f);

            // 기본 이동 속도 계산
            float newVelocityX = horizontalInput * speed;

            // 경사로 감지
            bool isOnSlope = false;
            Vector2 rayOrigin = rb.position;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 1.1f);

            if (hit.collider != null && hit.collider.CompareTag("Ground"))  // Ground 태그 확인
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle > 0 && slopeAngle <= 45f)
                {
                    isOnSlope = true;
                    // 경사면 방향 벡터 계산
                    Vector2 slopeDirection = new Vector2(hit.normal.y, -hit.normal.x);
                    rb.velocity = slopeDirection * (newVelocityX / Mathf.Cos(slopeAngle * Mathf.Deg2Rad));
                }
            }

            // 경사가 아닐 경우 일반 이동 적용
            if (!isOnSlope)
            {
                rb.velocity = new Vector2(newVelocityX, rb.velocity.y);
            }

            // 방향 전환
            if (horizontalInput != 0)
            {
                Turn(horizontalInput);
            }
        }

        // 캐릭터가 양방향으로 이동시에 알맞은 방향을 바라보도록 적용
        private void Turn(float direction)
        {

            if (direction != 0)
            {
                lastDirection = Mathf.Sign(direction); // 마지막 이동 방향 저장
            }

            var scale = transform.localScale;

            scale.x = Mathf.Sign(direction) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

        public void Jump()
        {
            // 바닥에 있을 때만 점프 가능
            if (isGrounded)
            {
                // 점프: 기존 X축 속도 유지, Y축 속도를 점프 힘으로 설정
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);

                // 점프 상태로 설정
                isGrounded = false;
            }
        }

        // 바닥 충돌 감지 (2D Physics)
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Ground 태그가 붙은 오브젝트와 충돌 시 바닥 상태로 전환
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
            }
        }

        public void Attack()
        {
            // 공격 애니메이션 적용
            animator.Play("Default Attack");

            // 공격 범위 Collider 활성화
            attackCollider.EnableCollider(true);

            // 일정 시간 후 Collider 다시 비활성화 (예: 0.5초 후)
            Invoke("DisableAttackCollider", 0.5f);
        }

        private void DisableAttackCollider()
        {
            attackCollider.EnableCollider(false);
        }

        public void SkillAttack(string skillId)
        {
            SkillBase skillBase = SkillDataManager.Singleton.GetSkillInstance(skillId);
            // 스킬을 사용할 수 있는 스킬포인트가 있는지 판단
            // 스킬 포인트가 부족하다면 스킬을 수행하지 못함
            if (currentSP < skillBase.SkillData.mpConsumption) return;

            currentSP -= skillBase.SkillData.mpConsumption;
            OnSPChanged?.Invoke(currentSP, maxSP);

            // 스킬 매니저에서 스킬을 쏠 수 있는지 여부를 판단 
            bool canExecute = SkillDataManager.Singleton.CanExecuteSkill(skillId);
            if (canExecute)
            {
                // 스킬 애니메이터 실행
                animator.Play("Skill Attack");

                // 스킬 매니저에 스킬 발사 요청 
                SkillDataManager.Singleton.ExecuteSkill(skillId, firePoint, lastDirection);
            }
        }


        public void ApplyDamage(float damage)
        {
            float prevHP = currentHP;
            currentHP -= damage;
            currentHP = Mathf.Clamp(currentHP, 0, maxHP);

            OnHPChanged?.Invoke(currentHP, maxHP);

            // 체력이 0 아래로 떨어지고 현 상태가 IsAlive 일때만 동작하도록 함
            if (currentHP <= 0f && prevHP > 0)
            {
                animator.SetTrigger("Dead Trigger");
            }

            // 체력이 0 보다 클때만 피격 모션 실행
            if (currentHP > 0)
            {
                if (damage < 10)
                {
                    animator.SetTrigger("Hit Trigger");
                }
            }
        }

        public void Dead()
        {
            gameObject.SetActive(false);

        }
    }
}
