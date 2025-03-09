using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TON
{
    public class IntroUI : UIBase
    {
        public List<IntroStoryData> introStories = new List<IntroStoryData>();

        private IntroStoryDataManager introStoryDataManager;
        private int index = 0;

        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image speakerImage;
        [SerializeField] private TextMeshProUGUI speakerText;
        [SerializeField] private TextMeshProUGUI content;


        private void OnEnable()
        {
            index = 0;

            introStoryDataManager = new IntroStoryDataManager();

            introStoryDataManager.Initialize();
            introStories = introStoryDataManager.introStories;

            SetStoryData();
        }

        public void HandleClickScreen()
        {
            if (index == introStories.Count - 1)
            {
                Main.Singleton.ChangeScene(SceneType.Lobby);
                return;
            }

            index++;
            SetStoryData();
        }

        private void SetStoryData()
        {
            IntroStoryData storyData = introStories[index];

            content.text = storyData.content;

            if (AssetManager.Singleton.LoadIntroBackgroundImage(index, out Sprite background))
            {
                backgroundImage.sprite = background;
            }

            LoadSpeaker(storyData.speaker);
        }

        private void LoadSpeaker(string speaker)
        {
            PlayerData player = PlayerDataManager.Singleton.player;

            Sprite loadImage = null;

            switch (speaker)
            {
                case "me":
                    AssetManager.Singleton.LoadPlayerIcon(player.type, FaceStatue.Idle, out loadImage);
                    speakerText.text = $"{player.name}";
                    break;
                case "villagers":
                    AssetManager.Singleton.LoadIntroVillagerImage(out loadImage);
                    speakerText.text = "마을 사람들";
                    break;
                case "dragon":
                    AssetManager.Singleton.LoadMonsterWaveIcon(10, out loadImage);
                    speakerText.text = "사악한 드래곤";
                    break;
            }

            if (loadImage != null)
            {
                speakerImage.sprite = loadImage;
            }
        }

        public void OnClickSkipButton()
        {
            Main.Singleton.ChangeScene(SceneType.Lobby);
        }


    }
}
