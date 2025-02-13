using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TON
{
    public class SkillScrollViewController : MonoBehaviour
    {
        public ScrollRect scrollRect;

        public SkillInformationItem skillInfoPrefab;
        public List<RectTransform> uiPrefabList = new List<RectTransform>();
        public List<SkillInformationItem> createSkillInfo = new List<SkillInformationItem>();

        public int playerLevel;

        private SkillInformationItem selectedSkillInfo;


        private void Start()
        {
            scrollRect = GetComponent<ScrollRect>();
            playerLevel = PlayerDataManager.Singleton.player.level;

            Initialize();
        }

        private void Initialize()
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

        public void OnClickSkillInfo()
        {
            GameObject selectedSlotGameObject = EventSystem.current.currentSelectedGameObject;
            selectedSkillInfo = selectedSlotGameObject.GetComponent<SkillInformationItem>();

            string selectSkillId = selectedSkillInfo.SelectedSkillInfo();
            // SkillSettingUI.Instance.OnClickSkillInfo(selectSkillId);
            Debug.Log($"OnClickSkillInfo() : {createSkillInfo.Count}");
            foreach (var skillInfo in createSkillInfo)
            {
                Debug.Log($"OnClickSkillInfo() : {selectSkillId} , {skillInfo.skillId}");
                Debug.Log(skillInfo.skillId != selectSkillId);
                if (skillInfo.skillId != selectSkillId)
                {
                    skillInfo.UnselectedSkillInfo();
                }
            }


        }
    }
}
