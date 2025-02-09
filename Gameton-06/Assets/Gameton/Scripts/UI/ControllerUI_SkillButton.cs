using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TON
{
    public class ControllerUI_SkillButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coolTimeText;
        [SerializeField] private Image coolTimeDimd;
        [SerializeField] private GameObject skillIcon;
        [SerializeField] private GameObject lockImage;

        [SerializeField]
        private SerializableDictionary<string, Sprite> skillSprite = new SerializableDictionary<string, Sprite>();

        public float currentCoolDown;
        public SkillBase skillBase;


        public void Initalize(SkillBase skillData)
        {
            skillBase = skillData;
            skillIcon.SetActive(true);
            skillIcon.GetComponent<Image>().sprite = skillSprite.GetValueOrDefault(skillBase.SkillData.id, null);
            lockImage.SetActive(false);
        }

        public void SetCoolTime()
        {
            SkillDataManager.Singleton.SetCoolTime(skillBase.SkillData.id);
            if (skillBase.CurrentCoolDown <= 0)
            {
                // 현재 스킬의 스킬 쿨다운 값을 설정
                skillBase.SetCurrentCoolDown();
                currentCoolDown = skillBase.CurrentCoolDown;
                UpdateCooldownUI();
            }
        }

        private void UpdateCooldownUI()
        {
            coolTimeText.gameObject.SetActive(currentCoolDown > 0); // 남은 쿨타임이 있을 때만 표시
            coolTimeText.text = $"{Mathf.CeilToInt(currentCoolDown)}s"; // 정수 초단위 표시
            coolTimeDimd.fillAmount = currentCoolDown / skillBase.SkillData.coolDown; // 1 → 0 으로 감소
        }

        void Update()
        {
            if (currentCoolDown > 0)
            {
                SkillDataManager.Singleton.UpdateSkillCoolDown(skillBase.SkillData.id); // 남은 쿨타임 감소
                UpdateCooldownUI(); // UI 업데이트
            }
            else
            {

            }
        }

    }
}
