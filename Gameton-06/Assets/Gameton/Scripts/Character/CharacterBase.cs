using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class CharacterBase : MonoBehaviour
    {
        public float speed;
        public VariableJoystick joystick;
        public Rigidbody2D rb;

        public void Start()
        {
            joystick = ControllerUI.Instance.joystick;
        }

        public void FixedUpdate()
        {
            if (joystick != null)
            {
                Debug.Log($"Horizontal Input: {joystick.input.x}, {joystick.input.y}");
                Debug.Log($"Horizontal Horizontal: {joystick.Horizontal}");
                Debug.Log($"Horizontal Direction: {joystick.Direction}");

                // 조이스틱의 Horizontal 값 가져오기
                float horizontalInput = joystick.Horizontal;

                // 입력값이 0이 아닐 때만 처리
                if (Mathf.Abs(horizontalInput) > 0.0f)
                {
                    // 현재 위치 가져오기
                    Vector2 currentPosition = rb.position;

                    // 새로운 X 위치 계산
                    float newXPosition = currentPosition.x + horizontalInput * speed * Time.fixedDeltaTime;

                    // Rigidbody2D의 위치 업데이트
                    rb.MovePosition(new Vector2(newXPosition, currentPosition.y));
                }
            }
        }
    }
}
