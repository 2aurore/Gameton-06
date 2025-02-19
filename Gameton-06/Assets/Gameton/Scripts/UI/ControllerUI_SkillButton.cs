using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace TON
{
    public class ControllerUI_SkillButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coolTimeText;
        [SerializeField] private Image coolTimeDimd;
        [SerializeField] private GameObject skillIcon;
        [SerializeField] private GameObject lockImage;

        public SkillBase skillBase;


        public void Initalize(SkillBase skillData)
        {
            if (skillData != null)
            {
                skillData.OnSkillExecuted -= OnSkillExecuted;
                // skillData.OnCooldownCompleted -= OnCooldownCompleted;
            }

            skillBase = skillData;
            skillData.OnSkillExecuted += OnSkillExecuted;
            // skillData.OnCooldownCompleted += OnCooldownCompleted;

            skillIcon.SetActive(true);

            Assert.IsTrue(AssetManager.Singleton.LoadSkillIcon(skillBase.SkillData.id, out Sprite loadedSkillImage));
            skillIcon.GetComponent<Image>().sprite = loadedSkillImage;
            lockImage.SetActive(false);
        }

        private void OnSkillExecuted()
        {
            UpdateCooldownUI();
        }

        private void UpdateCooldownUI()
        {
            if (coolTimeText == null || coolTimeDimd == null)
            {
                return; // UI가 삭제되었으면 업데이트 중단
            }

            coolTimeText.gameObject.SetActive(skillBase.CurrentCoolDown > 0); // 남은 쿨타임이 있을 때만 표시

            if (coolTimeText.IsActive())
            {
                coolTimeText.text = $"{skillBase.CurrentCoolDown: 0}s"; // 정수 초단위 표시
                coolTimeDimd.fillAmount = skillBase.CurrentCoolDown / skillBase.SkillCoolDown; // 1 → 0 으로 감소
            }
        }

        void Update()
        {
            UpdateCooldownUI(); // UI 업데이트
        }

    }
}
