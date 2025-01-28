using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class CharacterBase : MonoBehaviour
    {

        public float speed;
        public float jumpForce = 10f;  // 점프 힘
        private bool isGrounded = true; // 플레이어가 바닥에 있는지 여부를 판단

        private VariableJoystick joystick;
        public Rigidbody2D rb;


        public void Start()
        {
            joystick = ControllerUI.Instance.joystick;
            ControllerUI.Instance.linkedCharactor = this;
        }

        public void FixedUpdate()
        {
            // 키보드 입력과 조이스틱 입력 통합
            float horizontalInput = Input.GetAxis("Horizontal"); // 키보드 좌우 입력
            if (joystick != null && Mathf.Abs(joystick.Horizontal) > 0.01f)
            {
                horizontalInput = joystick.Horizontal; // 조이스틱 입력 우선
            }

            // 좌우 이동 처리 (X축 속도 설정)
            float newVelocityX = horizontalInput * speed;

            // Rigidbody2D의 속도 업데이트 (X축은 입력값 기반, Y축은 중력/점프 유지)
            rb.velocity = new Vector2(newVelocityX, rb.velocity.y);

            // 디버그용 출력
            // Debug.Log($"Horizontal Input: {horizontalInput}, Velocity: {rb.velocity}");
        }

        public void Jump()
        {
            // 바닥에 있을 때만 점프 가능
            if (isGrounded)
            {
                Debug.Log("Character Jump");

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
            Debug.Log("character Attack");
        }




    }
}
