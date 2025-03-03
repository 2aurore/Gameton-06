using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class GuideUI : UIBase
    {
        [SerializeField] private List<GameObject> guideList = new List<GameObject>();
        private int index = 0;


        private void OnEnable()
        {
            index = 0;

            ShowGuideObject();
        }

        private void OnDisable()
        {
            guideList.ForEach(guide => guide.SetActive(false));
        }

        private void ShowGuideObject()
        {
            if (index > 0)
            {
                guideList[index - 1].SetActive(false);
            }
            guideList[index].SetActive(true);
        }

        public void HandleClickScreen()
        {
            if (index == guideList.Count)
            {
                UIManager.Hide<GuideUI>(UIList.GuideUI);
                return;
            }

            index++;
            ShowGuideObject();
        }

    }
}
