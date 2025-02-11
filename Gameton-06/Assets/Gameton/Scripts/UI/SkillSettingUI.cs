using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TON
{
    public class SkillSettingUI : UIBase
    {
        public static SkillSettingUI Instance => UIManager.Singleton.GetUI<SkillSettingUI>(UIList.SkillSettingUI);

        public Transform skillSlotGroup;
        public SkillSettingUI_SkillSlot skillSlotPrefab;
        public List<SkillSettingUI_SkillSlot> createSkillSlots = new List<SkillSettingUI_SkillSlot>();

        public ScrollRect scrollRect;
        public SkillInformationItem skillInfoPrefab;
        public List<RectTransform> uiPrefabList = new List<RectTransform>();
        public List<SkillInformationItem> createSkillInfo = new List<SkillInformationItem>();

        public Button settingButton;


        private int selectedSlotIndex = -1;
        private string selectedSkillId = null;
        private int playerLevel;

        public void Start()
        {
            playerLevel = PlayerDataManager.Singleton.player.level;

            SetSkillSlots();
            SetSkillInfoItem();
        }

        private void Update()
        {
            if (selectedSlotIndex != -1 && !string.IsNullOrWhiteSpace(selectedSkillId))
            {
                settingButton.interactable = true;
            }
            else
            {
                settingButton.interactable = false;
            }
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
                    newSkillSlot.Initalize(activatedSkills[i].SkillData.id, i);
                }
                else
                {
                    // 복제 됐을때 기본 상태가 잠금 상태 
                    newSkillSlot.GetComponent<Button>().interactable = false;
                }
                createSkillSlots.Add(newSkillSlot);
            }
        }

        private void SetSkillInfoItem()
        {
            List<SkillData> skillDatas = SkillDataManager.Singleton.skillDatas;

            float y = 0;
            for (int i = 0; i < skillDatas.Count; i++)
            {
                SkillData skillData = skillDatas[i];
                SkillInformationItem skillInfoItem = Instantiate(skillInfoPrefab, scrollRect.content);

                skillInfoItem.gameObject.SetActive(true);
                skillInfoItem.Initalize(skillData, playerLevel);
                createSkillInfo.Add(skillInfoItem);

                if (playerLevel < skillData.requiredLevel)
                {
                    skillInfoItem.GetComponent<Button>().interactable = false;
                }

                RectTransform rectTransform = skillInfoItem.GetComponent<RectTransform>();

                uiPrefabList.Add(rectTransform);
                uiPrefabList[i].anchoredPosition = new Vector2(0f, -y);
                y += uiPrefabList[i].sizeDelta.y;
            }

            scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, y);
        }

        public void OnClickSkillSlot()
        {
            GameObject selectedSlotGameObject = EventSystem.current.currentSelectedGameObject;
            SkillSettingUI_SkillSlot selectedSlot = selectedSlotGameObject.GetComponent<SkillSettingUI_SkillSlot>();

            selectedSlotIndex = selectedSlot.SelectedSlot();
            for (int i = 0; i < 3; i++)
            {
                if (i != selectedSlotIndex)
                {
                    createSkillSlots[i].UnselectedSlot();
                }
            }
        }

        public void OnClickSkillInfo()
        {
            GameObject selectedSlotGameObject = EventSystem.current.currentSelectedGameObject;
            SkillInformationItem selectedSkillInfo = selectedSlotGameObject.GetComponent<SkillInformationItem>();

            selectedSkillId = selectedSkillInfo.SelectedSkillInfo();
            foreach (var skillInfo in createSkillInfo)
            {
                if (skillInfo.skillId != selectedSkillId)
                {
                    skillInfo.UnselectedSkillInfo();
                }
            }
        }

        public void OnClickSettingButton()
        {
            Debug.Log($"OnClickSettingButton() : {selectedSkillId} , {selectedSlotIndex}");
            // 스킬 데이터 업데이트 할때 selectedSlotIndex +1 해서 넘겨줘야함
            SkillDataManager.Singleton.UpdateSkillData(selectedSkillId, selectedSlotIndex + 1);

            // 스킬 업데이트 후 UI 갱신
            var unselectedSkill = createSkillInfo.Find(skill => skill.skillId == selectedSkillId);
            unselectedSkill?.UnselectedSkillInfo();
            createSkillSlots[selectedSlotIndex].UnselectedSlot();

            selectedSkillId = null;
            selectedSlotIndex = -1;

            RefreshUI();
        }

        private void RefreshUI()
        {
            SetSkillSlots();
            SetSkillInfoItem();
        }

        public void OnClickCloseButton()
        {
            UIManager.Hide<SkillSettingUI>(UIList.SkillSettingUI);
        }
    }
}
