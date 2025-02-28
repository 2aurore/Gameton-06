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
        [SerializeField] private Button cancelButton; // cancel 버튼 참조
        [SerializeField] private Button confirmButton; // cancel 버튼 참조
        [SerializeField] private Button createButton; // Create 버튼 참조
        [SerializeField] private List<PlayerData> playerDatas;
        [SerializeField] private TextMeshProUGUI nicknameCondition;


        public GameObject characterCreateUI_Modal;
        public GameObject blackCat_Spotlight;
        public GameObject whiteCat_Spotlight;

        private string selectedCharacter; // 선택한 캐릭터의 타입 저장 (예: "MaleCat", "FemaleCat")

        private void Start()
        {
            playerDatas = PlayerDataManager.Singleton.playersData;

            // 처음에는 버튼을 비활성화
            createButton.interactable = false;
        }

        public void SelectCharacter(string characterType)
        {
            selectedCharacter = characterType;
            createButton.interactable = true; // 캐릭터가 선택되면 버튼 활성화

            switch (characterType)
            {
                case "BlackCat":
                    blackCat_Spotlight.SetActive(true);
                    whiteCat_Spotlight.SetActive(false);
                    break;
                case "WhiteCat":
                    blackCat_Spotlight.SetActive(false);
                    whiteCat_Spotlight.SetActive(true);
                    break;
            }
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
            string nickname = characterName.text.Trim();

            // 입력 값 검증
            if (string.IsNullOrEmpty(nickname) || nickname.Length > 12)
            {
                // 닉네임은 비어있을 수 없고, 12자 이내로만 생성 가능
                return;
            }

            // 캐릭터 생성 및 취소 버튼을 비활성화 처리
            cancelButton.interactable = false;
            confirmButton.interactable = false;

            // 서버에 닉네임을 추가로 업데이트 한다.
            BackendManager.Singleton.ChangeNickname(nickname, (success, message) =>
            {
                // UI 업데이트 (메인 스레드에서 실행)
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    if (success)
                    {
                        // 생성한 캐릭터를 저장한다
                        PlayerData player = new PlayerData(playerDatas.Count, selectedCharacter, nickname);
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
                    else
                    {
                        // 닉네임 중복체크
                        if (message.Equals("409"))
                        {
                            DuplicateNickname();
                        }
                        else
                        {
                            Debug.LogError("서버 오류 :: 캐릭터 닉네임 저장 실패");
                        }
                    }
                });
            });
        }

        private void DuplicateNickname()
        {
            nicknameCondition.text = "이미 사용중인 이름입니다.";
            nicknameCondition.color = Color.red;
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
