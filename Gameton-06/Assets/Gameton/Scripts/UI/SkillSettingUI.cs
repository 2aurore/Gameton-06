using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class SkillSettingUI : UIBase
    {
        public static SkillSettingUI Instance => UIManager.Singleton.GetUI<SkillSettingUI>(UIList.SkillSettingUI);

        public Transform skillSlotGroup;
        public SkillSettingUI_SkillSlot skillSlotPrefab;
        public List<SkillSettingUI_SkillSlot> createSkillSlots = new List<SkillSettingUI_SkillSlot>();


        public void Start()
        {
            SetSkillSlots();
        }

        private void SetSkillSlots()
        {
            // 이미 기존에 UI가 생성되어 있다면 삭제
            if (createSkillSlots.Count > 0)
            {
                foreach (var button in createSkillSlots)
                {
                    Destroy(button.gameObject);
                }
                createSkillSlots.Clear();
            }

            // 스킬 버튼을 생성
            List<SkillBase> activatedSkills = SkillDataManager.Singleton.GetEquippedSkills();
            for (int i = 0; i < 3; i++)
            {
                SkillSettingUI_SkillSlot newSkillSlot = Instantiate(skillSlotPrefab, skillSlotGroup);
                newSkillSlot.gameObject.SetActive(true);

                if (i < activatedSkills.Count) // 해당 인덱스에 활성화된 스킬이 있을 경우
                {
                    newSkillSlot.Initalize(activatedSkills[i].SkillData.id);
                }
                else
                {
                    // 복제 됐을때 기본 상태가 잠금 상태 
                }

                createSkillSlots.Add(newSkillSlot);
            }
        }
    }
}
