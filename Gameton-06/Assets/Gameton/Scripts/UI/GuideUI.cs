using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class GuideUI : UIBase
    {
        public static GuideUI Instance => UIManager.Singleton.GetUI<GuideUI>(UIList.GuideUI);

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
            if (index == guideList.Count - 1)
            {
                UIManager.Hide<GuideUI>(UIList.GuideUI);
                UIManager.Show<LobbyUI>(UIList.LobbyUI);
                return;
            }

            index++;
            ShowGuideObject();
        }

    }
}
