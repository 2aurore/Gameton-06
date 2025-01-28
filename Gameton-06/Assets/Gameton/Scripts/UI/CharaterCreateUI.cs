using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TON
{
    public class CharaterCreateUI : UIBase
    {
        [SerializeField] private Button createButton; // Create 버튼 참조

        private string selectedCharacter; // 선택한 캐릭터의 타입 저장 (예: "MaleCat", "FemaleCat")

        private void Start()
        {
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
                Debug.Log("캐릭터를 선택하세요!");
                return;
            }

            // 선택된 캐릭터 정보를 저장 (다음 씬에서도 사용할 수 있도록)
            PlayerPrefs.SetString("SelectedCharacter", selectedCharacter);
            // TODO: 생성한 캐릭터를 어떻게 저장할지???


            // 씬 변경
            UIManager.Hide<CharaterCreateUI>(UIList.CharaterCreateUI);
            Main.Singleton?.ChangeScene(SceneType.Ingame);
        }
    }
}
