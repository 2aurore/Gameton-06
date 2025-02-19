using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace TON
{
    public class IngameUI : UIBase
    {
        public static IngameUI Instance => UIManager.Singleton.GetUI<IngameUI>(UIList.IngameUI);

        public Image characterImage;
        public Image hpBar;
        public Image spBar;

        public TextMeshProUGUI stageText;
        public TextMeshProUGUI monsterHp;
        public Image monsterHpBar;
        public Image monsterImage;


        private string playerType;

        [SerializeField] private CharacterBase character;


        private void OnEnable()
        {
            character = GameObject.FindWithTag("Player").GetComponent<CharacterBase>();
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
