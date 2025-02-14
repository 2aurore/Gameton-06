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
        public Transform starGroup;
        public StageStar starPrefab;

        public GameObject stageEntryUI;
        public GameObject lockState;
        public StageClearData stageClearData;


        public string stageId;
        private SerializableDictionary<string, StageClearData> bestStageClearDict = new SerializableDictionary<string, StageClearData>();

        public void Initalize(string stageId, int index)
        {
            this.stageId = stageId;

            if (stageId == "STG001")
            {
                lockState.SetActive(false);

            }

            Debug.Log($"LobbyUI_StagePage stageId: {stageId}");

            bestStageClearDict = StageManager.Singleton.bestStageClearDict;
            stageClearData = bestStageClearDict.GetValueOrDefault(stageId, null);

            SetStageImage();
            SetStageEntryInfo(index);
        }

        private void SetStageEntryInfo(int index)
        {
            if (stageClearData != null)
            {
                // 스테이지 슬롯에 스테이지 클리어 정보가 있는 경우
                for (int i = 0; i < 3; i++)
                {
                    StageStar stageStar = Instantiate(starPrefab, starGroup);
                    stageStar.gameObject.SetActive(true);
                    if (i < stageClearData.starRating)
                    {
                        stageStar.SetStar(true);
                    }
                    else
                    {
                        stageStar.SetStar(false);
                    }
                }
            }
            else
            {
                Debug.Log($"StageClearData is null");
                // 스테이지 슬롯에 스테이지 클리어 정보가 없고 첫번째 스테이지가 아닌 경우
                if (index != 0)
                {
                    StageClearData prevStageClearData = bestStageClearDict.GetValueOrDefault($"STG00{index - 1}", null);
                    // 직전 스테이지의 클리어 결과가 있고 별점이 1개 이상인 경우
                    if (prevStageClearData != null && prevStageClearData.starRating > 0)
                    {
                        lockState.SetActive(false);
                        stageEntryButton.interactable = true;
                    }
                    else
                    {
                        SetDefaultStarPoint();
                        // 이전 스테이지를 클리어하지 않은 경우
                        stageEntryButton.interactable = false;
                    }
                }
                else
                {
                    SetDefaultStarPoint();
                }

            }
        }

        private void SetDefaultStarPoint()
        {
            for (int i = 0; i < 3; i++)
            {
                StageStar stageStar = Instantiate(starPrefab, starGroup);
                stageStar.gameObject.SetActive(true);
                Instantiate(starPrefab, starGroup.transform).SetStar(false);
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

        public void OnClickStageChangeButton()
        {
            stageEntryUI.SetActive(false);
        }
    }
}
