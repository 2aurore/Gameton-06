using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace TON
{
    public class LobbyUI_StagePage : MonoBehaviour
    {
        public Button stageEntryButton;
        public GameObject stageImage;
        public GameObject starPoint;

        public GameObject stageEntryUI;
        public GameObject lockState;

        private string stageId;

        public StageClearData stageClearData;

        public void Initalize(string stageId)
        {
            this.stageId = stageId;

            if (stageId == "STG001")
            {
                lockState.SetActive(false);
            }

            SerializableDictionary<string, StageClearData> bestStageClearDict = StageManager.Singleton.bestStageClearDict;
            stageClearData = bestStageClearDict.GetValueOrDefault(stageId, null);
            SetStageImage();

            // 스테이지 슬롯에 스테이지 클리어 정보가 있는 경우
            if (stageClearData != null)    // 스테이지 슬롯에 별점이 지정된 경우
            {
                // for (int i = 0; i < starCount; i++)
                // {
                //     starPoint.transform.GetChild(i).gameObject.SetActive(true);
                // }
            }
            else
            {
                // 이전 스테이지가 클리어되지 않아 진입할 수 없는 경우
                stageEntryButton.interactable = false;
            }
        }

        private void SetStageImage()
        {
            Assert.IsTrue(AssetManager.Singleton.LoadStageIcon(stageId, out Sprite loadedStageImage));
            stageImage.GetComponent<Image>().sprite = loadedStageImage;
            stageImage.SetActive(true);
        }

        public void OnClickStageButton()
        {
            stageEntryUI.SetActive(true);
        }
    }
}
