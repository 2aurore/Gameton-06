using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TON
{
    public class IngameUI : UIBase
    {
        public static IngameUI Instance => UIManager.Singleton.GetUI<IngameUI>(UIList.IngameUI);

        public Image characterImage;
        public Image hpBar;
        public Image spBar;

        public TextMeshProUGUI waveText;
        public TextMeshProUGUI playTimeText;
        public TextMeshProUGUI scoreText;

        // 플레이 시간 노출하게 하는 변수
        private int lastMinutes = -1;
        private int lastSeconds = -1;
        private string cachedTimeString = "00:00";

        private string playerType;

        [SerializeField] private CharacterBase character;


        private void OnEnable()
        {
            character = GameObject.Find("TON.Player").GetComponentInChildren<CharacterBase>();
            playerType = PlayerDataManager.Singleton.player.type;

            if (character != null)
            {
                character.OnHPChanged += UpdateHPBar;
                character.OnSPChanged += UpdateSPBar;
            }

            // UI가 활성화될 때 저장된 값들로 업데이트
            RefreshUI();
        }

        private void OnDisable()
        {
            if (character != null)
            {
                character.OnHPChanged -= UpdateHPBar;
                character.OnSPChanged -= UpdateSPBar;
            }
        }


        private void Update()
        {
            UpdatePlayTimeDisplay();
        }

        private void UpdatePlayTimeDisplay()
        {
            float playTime = StageManager.Singleton.PlayTime;

            int minutes = Mathf.FloorToInt(playTime / 60f);
            int seconds = Mathf.FloorToInt(playTime % 60f);

            // 시간이 변경된 경우에만 문자열 생성
            if (minutes != lastMinutes || seconds != lastSeconds)
            {
                lastMinutes = minutes;
                lastSeconds = seconds;
                cachedTimeString = $"time {minutes:00}:{seconds:00}";
                playTimeText.text = cachedTimeString;
            }
        }

        private void UpdateHPBar(float current, float max)
        {
            hpBar.fillAmount = current / max;
        }

        private void UpdateSPBar(float current, float max)
        {
            spBar.fillAmount = current / max;
        }

        public void SetPlayerImage(string type)
        {
            playerType = type;
            if (gameObject.activeInHierarchy)
            {
                UpdatePlayerImage();
            }
        }

        private void RefreshUI()
        {
            if (!string.IsNullOrEmpty(playerType)) UpdatePlayerImage();
        }

        private void UpdatePlayerImage()
        {
            if (AssetManager.Singleton.LoadPlayerIcon(playerType, FaceStatue.Idle, out Sprite result))
            {
                characterImage.sprite = result;
            }
        }

    }
}
