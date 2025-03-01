using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace TON
{
    public class ControllerUI_ItemButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coolTimeText;
        [SerializeField] private Image coolTimeDimd;

        private float currentCoolDown = 0f;
        private float potionCoolDown = 5f;

        public void SetCurrentCoolDown()
        {
            currentCoolDown = potionCoolDown; // 쿨타임 시작
        }

        private void Update()
        {
            if (currentCoolDown == 0f)
                return;

            UpdateCooldownUI();
        }

        private void UpdateCooldownUI()
        {
            if (coolTimeText == null || coolTimeDimd == null)
            {
                return; // UI가 삭제되었으면 업데이트 중단
            }

            currentCoolDown -= Time.deltaTime;
            currentCoolDown = Mathf.Max(0, currentCoolDown);

            coolTimeText.gameObject.SetActive(currentCoolDown > 0); // 남은 쿨타임이 있을 때만 표시

            if (coolTimeText.IsActive())
            {
                coolTimeText.text = $"{currentCoolDown: 0}s"; // 정수 초단위 표시
                coolTimeDimd.fillAmount = currentCoolDown / potionCoolDown; // 1 → 0 으로 감소
            }
        }
    }
}
