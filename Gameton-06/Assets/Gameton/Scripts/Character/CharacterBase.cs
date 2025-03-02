using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class CharacterBase : MonoBehaviour, IDamage
    {

        [SerializeField] private PlayerData playerData;
        [SerializeField] private float currentHP;
        [SerializeField] private float currentSP;
        private float maxHP;
        private float maxSP;

        [SerializeField] private float speed;
        [SerializeField] private float jumpForce = 5f;  // 점프 힘
        [SerializeField] private float airControl;  // 점프 힘

        [SerializeField] private Transform groundCheck;  // GroundCheck 위치 설정
        [SerializeField] private float groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private WallChecker wallChecker;


        private bool isGrounded = true; // 플레이어가 바닥에 있는지 여부를 판단
        private bool isAttack = false; // 플레이어가 기본 공격중인지 판단단
        private float lastDirection = 1f; // 기본적으로 오른쪽(1) 바라보는 상태
        private VariableJoystick joystick;
        private Animator animator;

        public Transform firePoint; // 스킬 발사 위치
        public CollisionDetector attackCollider; // 기본 공격 감지를 위한 자식 오브젝트
        public Rigidbody2D rb;

        // ingame UI의 캐릭터 stat 적용을 위한 이벤트
        public event System.Action<float, float> OnHPChanged;
        public event System.Action<float, float> OnSPChanged;

        [SerializeField] private float mpRecoveryRate = 1f;  // MP 회복량
        [SerializeField] private float mpRecoveryInterval = 3f;  // 회복 간격(초)
        [SerializeField] private bool isRecovering = false;

        public AudioClip _attackSound;
        public AudioClip _deathSound;

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

        // 게임이 실행 중이지 않을 때도 항상 기즈모를 보여줍니다
        private void OnDrawGizmos()
        {
            if (groundCheck == null) return;

            // 기본 색상을 흰색으로 설정
            Gizmos.color = Color.red;
            // OverlapCircle의 범위를 와이어프레임 원으로 표시
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        private bool CheckIsGrounded()
        {
            return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        public void FixedUpdate()
        {
            isGrounded = CheckIsGrounded();

            // 키보드 입력과 조이스틱 입력 통합
            float horizontalInput = Input.GetAxis("Horizontal");
            if (joystick != null && Mathf.Abs(joystick.Horizontal) > 0.01f)
            {
                horizontalInput = joystick.Horizontal; // 조이스틱 입력 우선
            }

            // 걷는 애니메이션 적용
            animator.SetBool("IsMoving", Mathf.Abs(horizontalInput) > 0f);

            // 측면 충돌 체크
            if (!isGrounded && wallChecker.IsWallTouching)
            {
                // 벽을 밀고 있을 때는 수평 이동 제한
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else
            {
                // 기본 이동 속도 계산
                float newVelocityX = horizontalInput * speed;

                // 경사로 감지
                bool isOnSlope = false;
                Vector2 rayOrigin = rb.position;
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 1.1f);


                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle > 0 && slopeAngle <= 45f)
                {
                    isOnSlope = true;
                    // 경사면 방향 벡터 계산
                    Vector2 slopeDirection = new Vector2(hit.normal.y, -hit.normal.x);
                    rb.velocity = slopeDirection * (newVelocityX / Mathf.Cos(slopeAngle * Mathf.Deg2Rad));
                }

                // 경사가 아닐 경우 일반 이동 적용
                if (!isOnSlope)
                {
                    rb.velocity = new Vector2(newVelocityX, rb.velocity.y);
                }

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
            }
        }

        public void Attack()
        {
            // 이미 기본 공격중인 경우 공격 제한
            if (isAttack)
                return;

            isAttack = true;
            // 공격 애니메이션 적용
            animator.Play("Default Attack");

            SoundManager.instance.SFXPlay("Attack", _attackSound);

            // 공격 범위 Collider 활성화
            attackCollider.EnableCollider(true);

            // 일정 시간 후 Collider 다시 비활성화 (예: 0.5초 후)
            Invoke("DisableAttackCollider", 0.5f);
        }

        private void DisableAttackCollider()
        {
            isAttack = false;
            attackCollider.EnableCollider(false);
        }


        // MP 회복 코루틴
        private IEnumerator RecoverSP()
        {
            isRecovering = true;

            while (currentSP < maxSP)
            {
                yield return new WaitForSeconds(mpRecoveryInterval);

                if (currentSP < maxSP)
                {
                    currentSP = Mathf.Min(maxSP, currentSP + mpRecoveryRate);
                    OnSPChanged?.Invoke(currentSP, maxSP);
                }
            }

            isRecovering = false;
        }

        public void UsePotion(string type, System.Action<bool> callback)
        {

            if (type.Equals("HP") && currentHP == maxHP)
            {
                callback?.Invoke(false);
                return;
            }
            if (type.Equals("MP") && currentSP == maxSP)
            {
                callback?.Invoke(false);
                return;
            }

            switch (type)
            {
                case "HP":
                    if (currentHP < maxHP)  // currentHP가 maxHP보다 작을 때만 실행
                    {
                        currentHP = Mathf.Min(currentHP + (maxHP * 0.2f), maxHP); // maxHP를 초과하지 않도록 보정
                        OnHPChanged?.Invoke(currentHP, maxHP);
                    }
                    break;
                case "MP":
                    if (currentSP < maxSP)  // currentSP가 maxSP보다 작을 때만 실행
                    {
                        currentSP = Mathf.Min(currentSP + (maxSP * 0.2f), maxSP); // maxSP를를 초과하지 않도록 보정
                        OnSPChanged?.Invoke(currentSP, maxSP);
                    }
                    break;
            }

            callback?.Invoke(true);
        }

        public void SkillAttack(string skillId)
        {
            SkillBase skillBase = SkillDataManager.Singleton.GetSkillInstance(skillId);
            // 스킬을 사용할 수 있는 스킬포인트가 있는지 판단
            // 스킬 포인트가 부족하다면 스킬을 수행하지 못함
            if (currentSP < skillBase.SkillData.mpConsumption) return;

            // 스킬 매니저에서 스킬을 쏠 수 있는지 여부를 판단 
            bool canExecute = SkillDataManager.Singleton.CanExecuteSkill(skillId);
            if (canExecute)
            {
                // 스킬을 쓸 수 있는 상태 - 쿨타임이 돌지 않을때만 마나 소모
                currentSP -= skillBase.SkillData.mpConsumption;
                OnSPChanged?.Invoke(currentSP, maxSP);

                // 스킬 애니메이터 실행
                animator.Play("Skill Attack");

                // 스킬 매니저에 스킬 발사 요청 
                SkillDataManager.Singleton.ExecuteSkill(skillId, firePoint, lastDirection);

                // RecoverSP 가 이미 진행중인 경우 이중으로 코루틴을 실행하지 않도록 함함
                if (!isRecovering)
                {
                    StartCoroutine(RecoverSP());
                }
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
                Dead();
                SoundManager.instance.SFXPlay("Death", _deathSound);
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
            animator.SetTrigger("Dead Trigger");
        }

        // 플레이어 사망 애니메이션 종료 후 호출
        public void DestroyDead()
        {
            PlayerDataManager.Singleton.PlayerDeadEvent();

            gameObject.SetActive(false);
        }
    }
}
