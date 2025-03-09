using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TON
{
    public class CharaterSelectUI : UIBase
    {

        [SerializeField] private Button createButton; // Create 버튼 참조
        [SerializeField] private Button playButton; // Play 버튼 참조

        [SerializeField] private List<PlayerData> playerDatas;
        [SerializeField] private List<HeartData> heartDatas;

        private int currentSelectCharacterIndex;

        public List<CharaterSelectUI_SlotItem> CharacterSlots = new List<CharaterSelectUI_SlotItem>();

        public SerializableDictionary<string, Sprite> CharacterSpriteDict = new SerializableDictionary<string, Sprite>();


        private void Start()
        {
            playerDatas = PlayerDataManager.Singleton.playersData;
            heartDatas = HeartDataManager.Singleton.heartDatas;

            // 캐릭터를 선택한 이후에 버튼 활성화 할 수 있도록 초기 비활성화 적용
            playButton.interactable = false;

            if (playerDatas.Count >= 5)
            {
                // 캐릭터 슬롯을 모두 사용하고 있다면 버튼 비활성화 적용
                createButton.interactable = false;
            }

            for (int i = 0; i < playerDatas.Count; i++)
            {
                CharacterSpriteDict.TryGetValue(playerDatas[i].type, out Sprite sprite);
                string name = playerDatas[i].name != null ? playerDatas[i].name : "";

                if (sprite)
                {
                    CharacterSlots[i].SetCharaterData(sprite, name, i);
                }
            }

        }


        public void SelectCharacter(int index)
        {
            Debug.Log("선택한 캐릭터 인덱스" + index);
            currentSelectCharacterIndex = index;
            playButton.interactable = true;
        }

        public void OnClickPlayButton()
        {
            PlayerPrefs.SetInt("SelectedPlayerIndex", currentSelectCharacterIndex);
            PlayerDataManager.Singleton.SetCurrentUserData();
            HeartDataManager.Singleton.SetCurrentUserHeart();

            UIManager.Hide<CharaterSelectUI>(UIList.CharaterSelectUI);

            Main.Singleton.ChangeScene(SceneType.Intro);
        }

        public void OnClickCreateButton()
        {
            UIManager.Show<CharaterCreateUI>(UIList.CharaterCreateUI);
            UIManager.Hide<CharaterSelectUI>(UIList.CharaterSelectUI);
        }
    }
}
