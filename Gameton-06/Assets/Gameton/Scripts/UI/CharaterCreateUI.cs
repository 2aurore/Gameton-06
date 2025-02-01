using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TON
{
    public class CharaterCreateUI : UIBase
    {
        [SerializeField] private Button createButton; // Create 버튼 참조
        [SerializeField] private List<PlayerData> playerDatas;


        private string selectedCharacter; // 선택한 캐릭터의 타입 저장 (예: "MaleCat", "FemaleCat")

        private void Start()
        {
            playerDatas = PlayerDataManager.Singleton.players;
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

            // TODO: 캐릭터 이름 받아서 생성하게끔 로직 변경 필요
            // 생성한 캐릭터를 저장한다
            PlayerData player = new PlayerData(playerDatas.Count, selectedCharacter, "name");
            playerDatas.Add(player);
            JSONLoader.SaveToFile(playerDatas, "player");


            // 씬 변경
            UIManager.Hide<CharaterCreateUI>(UIList.CharaterCreateUI);
            Main.Singleton?.ChangeScene(SceneType.Lobby);
        }
    }
}
