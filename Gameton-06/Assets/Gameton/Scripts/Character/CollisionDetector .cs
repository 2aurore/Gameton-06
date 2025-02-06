using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class CollisionDetector : MonoBehaviour
    {
        private Collider2D col; // 자식 오브젝트의 Collider

        private void Awake()
        {
            col = GetComponent<Collider2D>();
            col.enabled = false; // 초기에는 감지 비활성화
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!col.enabled) return; // Collider가 활성화된 경우에만 실행

            if (collision.CompareTag("Monster")) // 적과 충돌 시 제거
            {
                collision.GetComponent<IDamage>().ApplyDamage(10f);
            }
        }

        // 부모(캐릭터)에서 호출할 메서드
        public void EnableCollider(bool isEnabled)
        {
            col.enabled = isEnabled;
        }
    }
}
