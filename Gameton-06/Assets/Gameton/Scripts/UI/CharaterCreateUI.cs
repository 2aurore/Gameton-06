using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace TON
{
    public class CharaterCreateUI : UIBase
    {
        [SerializeField] private Button cancelButton; // cancel 버튼 참조
        [SerializeField] private Button confirmButton; // confirm 버튼 참조
        [SerializeField] private Button createButton; // Create 버튼 참조
        [SerializeField] private TextMeshProUGUI nicknameCondition;


        public TMP_InputField nicknameInputField;
        public GameObject characterCreateUI_Modal;
        public GameObject blackCat_Spotlight;
        public GameObject whiteCat_Spotlight;

        private string selectedCharacter; // 선택한 캐릭터의 타입 저장 (예: "MaleCat", "FemaleCat")

        private void Start()
        {
            // 처음에는 버튼을 비활성화
            createButton.interactable = false;


            // 입력 필드가 할당되지 않았다면 현재 게임 오브젝트의 InputField 컴포넌트를 가져옵니다.
            if (nicknameInputField == null)
            {
                nicknameInputField = GetComponent<TMP_InputField>();
            }

            // 입력 필드의 문자 확인 이벤트에 메서드 연결
            nicknameInputField.onValidateInput += ValidateInput;
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

            // 캐릭터 이름 입력 모달 활성화
            characterCreateUI_Modal.SetActive(true);
        }

        private char ValidateInput(string text, int charIndex, char addedChar)
        {
            // 영문 대소문자만 허용 (A-Z, a-z)
            if (Regex.IsMatch(addedChar.ToString(), @"^[a-zA-Z]+$"))
            {
                return addedChar;
            }

            // 영문 이외의 문자는 무시
            return '\0';
        }

        // 추가 옵션: 기존 텍스트에 영문 이외의 문자가 있다면 제거하는 메서드
        public void RemoveNonEnglishCharacters()
        {
            string englishOnly = Regex.Replace(nicknameInputField.text, @"[^a-zA-Z]", "");
            nicknameInputField.text = englishOnly;
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
                        PlayerData player = new PlayerData(selectedCharacter, nickname);
                        PlayerDataManager.Singleton.SetPlayerData(player);

                        // 뒤끝 서버 데이터 저장 로직 적용
                        PlayerDataManager.Singleton.CreateNewPlayer(player, isSuccess =>
                        {
                            if (isSuccess)
                            {
                                // 하트 시스템을 생성한다
                                HeartDataManager.Singleton.CreateNewHeartSystem(0);
                                HeartDataManager.Singleton.SetCurrentUserHeart();

                                // 씬 변경
                                UIManager.Hide<CharaterCreateUI>(UIList.CharaterCreateUI);
                                Main.Singleton.ChangeScene(SceneType.Lobby);
                            }
                        });

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

                        cancelButton.interactable = true;
                        confirmButton.interactable = true;
                    }
                });
            });
        }

        private void DuplicateNickname()
        {
            nicknameCondition.text = "이미 사용중인 이름입니다.";
            nicknameCondition.color = Color.yellow;
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
