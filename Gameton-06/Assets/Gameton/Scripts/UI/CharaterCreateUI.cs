using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace TON
{
    public class CharaterCreateUI : UIBase
    {
        [SerializeField] private Button cancelButton; // Create 버튼 참조
        [SerializeField] private Button createButton; // Create 버튼 참조
        [SerializeField] private List<PlayerData> playerDatas;
        [SerializeField] private List<HeartData> heartDatas;


        public GameObject characterCreateUI_Modal;

        private string selectedCharacter; // 선택한 캐릭터의 타입 저장 (예: "MaleCat", "FemaleCat")

        private void Start()
        {
            playerDatas = PlayerDataManager.Singleton.playersData;
            heartDatas = HeartDataManager.Singleton.heartDatas;

            // 처음에는 버튼을 비활성화
            createButton.interactable = false;
        }

        public void SelectCharacter(string characterType)
        {
            selectedCharacter = characterType;
            createButton.interactable = true; // 캐릭터가 선택되면 버튼 활성화

        }

        public void OnClickCreateButton()
        {
            if (string.IsNullOrEmpty(selectedCharacter))
            {
                // 캐릭터 선택되지 않은 상태에서 버튼 동작 되지 않도록 적용
                return;
            }

            // 선택된 캐릭터 인덱스 정보를 저장 (다음 씬에서도 사용할 수 있도록)
            PlayerPrefs.SetInt("SelectedPlayerIndex", playerDatas.Count);

            // 캐릭터 이름 입력 모달 활성화
            characterCreateUI_Modal.SetActive(true);
        }

        public void OnClickConfirmButton()
        {
            TMP_InputField characterName = characterCreateUI_Modal.GetComponentInChildren<TMP_InputField>();
            Debug.Log("characterName" + characterName.text);
            // 생성한 캐릭터를 저장한다
            PlayerData player = new PlayerData(playerDatas.Count, selectedCharacter, characterName.text);
            playerDatas.Add(player);
            Assert.IsTrue(JSONLoader.SaveUpdatedJsonToPersistentData(playerDatas, "player"));

            PlayerDataManager.Singleton.SetCurrentUserData();

            // 하트 시스템을 생성한다
            HeartDataManager.Singleton.CreateNewHeartSystem(playerDatas.Count);
            HeartDataManager.Singleton.SetCurrentUserHeart();

            // 씬 변경
            UIManager.Hide<CharaterCreateUI>(UIList.CharaterCreateUI);

            Main.Singleton?.ChangeScene(SceneType.Lobby);
        }

        public void OnClickCancelButton()
        {
            // 캐릭터 이름 입력 모달 비활성화
            characterCreateUI_Modal.SetActive(false);
            TMP_InputField characterName = characterCreateUI_Modal.GetComponentInChildren<TMP_InputField>();
            characterName.text = "";
        }
    }
}
