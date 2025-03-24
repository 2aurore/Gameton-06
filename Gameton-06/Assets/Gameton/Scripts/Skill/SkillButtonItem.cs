using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace TON
{
    public class SkillButtonItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coolTimeText;
        [SerializeField] private Image coolTimeDimd;
        [SerializeField] private GameObject skillIcon;
        [SerializeField] private GameObject lockImage;

        public SkillBase skillBase { get; private set; }


        public void Initalize(SkillBase skillData)
        {
            // 직접 주어진 skillData 인스턴스 사용
            skillBase = skillData;

            skillIcon.SetActive(true);

            if (AssetManager.Singleton.LoadSkillIcon(skillBase.SkillData.id, out Sprite loadedSkillImage))
            {
                skillIcon.GetComponent<Image>().sprite = loadedSkillImage;
                lockImage.SetActive(false);
            }
        }

        private void UpdateCooldownUI()
        {
            if (coolTimeText == null || coolTimeDimd == null)
            {
                return; // UI가 삭제되었으면 업데이트 중단
            }
            if (skillBase == null)
            {
                return;
            }

            SkillBase targetSkill = SkillDataManager.Singleton.GetEquippedSkillFromId(skillBase.SkillData.id);

            // 현재 쿨타임 상태 로그
            // Debug.Log($"Skill: {skillBase.SkillData.id}, CurrentCoolDown: {targetSkill.CurrentCoolDown}, SkillCoolDown: {targetSkill.SkillCoolDown}");

            // 남은 쿨타임이 있을 때만 표시
            coolTimeText.gameObject.SetActive(targetSkill.CurrentCoolDown > 0);

            if (targetSkill.CurrentCoolDown > 0)
            {
                coolTimeText.text = $"{targetSkill.CurrentCoolDown:0.0}s"; // 소수점 한 자리까지 표시
                coolTimeDimd.fillAmount = targetSkill.CurrentCoolDown / targetSkill.SkillCoolDown; // 1 → 0 으로 감소
            }
            else
            {
                coolTimeDimd.fillAmount = 0; // 쿨타임이 없으면 딤드 효과 제거
            }
        }

        void Update()
        {
            UpdateCooldownUI(); // UI 업데이트
        }
    }
}
