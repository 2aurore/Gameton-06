using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TON
{
    public class DamageEffect : PoolAble
    {
        public TextMeshProUGUI textMesh;
        public float floatSpeed = 1.0f;
        public float duration = 1.0f;

        private float timer = 0f;

        private void Awake()
        {
            // Canvas의 Render Mode가 World Space일 경우, Main Camera를 Event Camera로 설정
            Canvas canvas = gameObject.GetComponentInChildren<Canvas>();
            if (canvas.renderMode == RenderMode.WorldSpace && canvas.worldCamera == null)
            {
                canvas.worldCamera = Camera.main; // Main Camera 할당
            }

        }

        public void SetDamage(int damage, bool isCritical)
        {
            // 데미지 값에 따라 색상이나 크기 변경 가능
            textMesh.text = string.Format("{0:#,###}", damage);

            // 크리티컬 히트일 경우 색상 변경
            if (isCritical)
                textMesh.color = Color.yellow;
            else
                textMesh.color = Color.white;

            // 타이머 리셋
            timer = 0f;
        }

        void Update()
        {
            // 위로 떠오르는 효과
            transform.position += Vector3.up * floatSpeed * Time.deltaTime;

            // 시간에 따른 투명도 조절
            timer += Time.deltaTime;
            float alpha = 1.0f - (timer / duration);
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, alpha);

            // 지속 시간이 지나면 비활성화
            if (timer >= duration)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
