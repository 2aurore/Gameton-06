using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TON
{
    public class SkillScrollViewController : MonoBehaviour
    {
        public ScrollRect scrollRect;
        public float space = 10f;

        public GameObject uiPrefab;
        public List<RectTransform> uiPrefabList = new List<RectTransform>();

        private void Start()
        {
            scrollRect = GetComponent<ScrollRect>();

            Initialize();
        }

        private void Initialize()
        {
            List<SkillData> skillDatas = SkillDataManager.Singleton.skillDatas;

            float y = 0;
            for (int i = 0; i < skillDatas.Count; i++)
            {
                SkillData skillData = skillDatas[i];
                GameObject skillInfoItem = Instantiate(uiPrefab, scrollRect.content);
                skillInfoItem.GetComponent<SkillInformationItem>().Initalize(skillData);

                RectTransform rectTransform = skillInfoItem.GetComponent<RectTransform>();

                uiPrefabList.Add(rectTransform);
                uiPrefabList[i].anchoredPosition = new Vector2(0f, -y);
                y += uiPrefabList[i].sizeDelta.y;
            }

            scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, y);
        }
    }
}
