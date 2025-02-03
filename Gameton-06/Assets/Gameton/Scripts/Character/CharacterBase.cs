using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class CharacterBase : MonoBehaviour, IDamage
    {


        public float currentHP;
        public float maxHP;
        public float currentSP;
        public float maxSP;

        public float speed;
        public float jumpForce = 5f;  // 점프 힘
        private bool isGrounded = true; // 플레이어가 바닥에 있는지 여부를 판단
        private float lastDirection = 1f; // 기본적으로 오른쪽(1) 바라보는 상태


        public Transform firePoint; // 스킬 발사 위치

        public Animator animator;

        private VariableJoystick joystick;
        public Rigidbody2D rb;


        public void Start()
        {
            animator = GetComponent<Animator>();
            joystick = ControllerUI.Instance.joystick;
            ControllerUI.Instance.linkedCharactor = this;

            Initialize();
        }

        public void Initialize()
        {
            int playerIndex = PlayerPrefs.GetInt("SelectedPlayerIndex", 0);
            PlayerData playerData = PlayerDataManager.Singleton.players[playerIndex];

            currentHP = maxHP = playerData.hp;
            currentSP = maxSP = playerData.mp;
        }

        public void FixedUpdate()
        {

            // 키보드 입력과 조이스틱 입력 통합
            float horizontalInput = Input.GetAxis("Horizontal"); // 키보드 좌우 입력
            if (joystick != null && Mathf.Abs(joystick.Horizontal) > 0.01f)
            {
                horizontalInput = joystick.Horizontal; // 조이스틱 입력 우선
            }

            // 걷는 애니메이션 적용
            animator.SetBool("IsMoving", Mathf.Abs(horizontalInput) > 0f);

            // 좌우 이동 처리 (X축 속도 설정)
            float newVelocityX = horizontalInput * speed;

            // Rigidbody2D의 속도 업데이트 (X축은 입력값 기반, Y축은 중력/점프 유지)
            rb.velocity = new Vector2(newVelocityX, rb.velocity.y);

            // 방향을 변경하는 로직 (0이 아닐 때만 방향 업데이트)
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
        }

        public void SkillAttack(string skillName)
        {
            animator.Play("Skill Attack");

            // 총알 생성
            GameObject skill = ObjectPoolManager.Instance.GetEffect(skillName);

            // skill.transform.SetParent(firePoint);
            skill.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);


            // 🔥 총알 방향 반전
            var bulletScale = skill.transform.localScale;
            bulletScale.x = Mathf.Abs(bulletScale.x) * lastDirection;
            skill.transform.localScale = bulletScale;

            // 총알 이동 방향 설정
            Rigidbody2D skillRb = skill.GetComponent<Rigidbody2D>();
            skillRb.velocity = new Vector2(lastDirection * 5f, 0f);
        }


        public void ApplyDamage(float damage)
        {
            float prevHP = currentHP;
            currentHP -= damage;
            currentHP = Mathf.Clamp(currentHP, 0, maxHP);

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
