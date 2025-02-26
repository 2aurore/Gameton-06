using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace TON
{
    public class LobbyUI_StagePage : MonoBehaviour
    {
        public Button stageEntryButton;
        // public TextMeshProUGUI stageTitle;
        public GameObject stageImage;
        public Transform starGroup;
        public StageStar starPrefab;

        public GameObject stageEntryUI;
        public GameObject lockState;
        public StageClearData stageClearData;


        public string stageId;
        private SerializableDictionary<string, StageClearData> bestStageClearDict = new SerializableDictionary<string, StageClearData>();

        /// <summary>
        ///  스테이지 1개 축소 및 게임 장르 변경으로 아래 메소드 사용하지 않음음
        /// </summary>
        public void Initalize(string stageId)
        {
            this.stageId = stageId;

            if (stageId == "STG001")
            {
                lockState.SetActive(false);
                stageEntryButton.interactable = true;
            }

            bestStageClearDict = StageManager.Singleton.bestStageClearDict;
            stageClearData = bestStageClearDict.GetValueOrDefault(stageId, null);

            SetStageImage();
            // SetStageEntryInfo(index);
            // stageTitle.text = $"stage {index + 1}";
        }

        /// <summary>
        ///  스테이지 1개 축소 및 게임 장르 변경으로 아래 메소드 사용하지 않음음
        /// </summary>
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

                        // FIXME: 개발 편의를 위해 스테이지 입장 가능하게 오픈
                        stageEntryButton.interactable = true;

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
            // 현재 입장 UI가 열려 있는 상태에 따라 영역을 클릭했을때 on off 하도록 함
            stageEntryUI.SetActive(!stageEntryUI.activeSelf);
        }

        public void OnClickStageChangeButton()
        {
            stageEntryUI.SetActive(false);
        }
    }
}
