using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TON
{
    public class StageEntryUI : UIBase
    {

        [SerializeField] private Button playButton; // Play 버튼 참조

        private int currentSelectStage;

        private void Start()
        {
            playButton.interactable = false;
        }

        public void OnClickBackButton()
        {
            UIManager.Show<LobbyUI>(UIList.LobbyUI);
            UIManager.Hide<StageEntryUI>(UIList.StageEntryUI);
        }

        public void OnClickStageButton(int stage)
        {
            currentSelectStage = stage;
            if (currentSelectStage != 0)
            {
                playButton.interactable = true;
            }
        }

        public void OnClickPlayButton()
        {
            if (currentSelectStage == 0)
                return;

            Debug.Log("OnClickPlayButton::::" + currentSelectStage);
        }

        public void OnClickTest()
        {
            Debug.Log("OnClickTest::::");
            HeartDataManager.Singleton.UseHeart();
        }
    }
}
