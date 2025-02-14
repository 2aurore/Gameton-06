using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace TON
{
    public class LobbyUI : UIBase
    {
        [SerializeField]
        private TextMeshProUGUI characterName;
        [SerializeField]
        private TextMeshProUGUI characterHp;
        [SerializeField]
        private TextMeshProUGUI characterMp;
        [SerializeField]
        private TextMeshProUGUI characterAttck;
        [SerializeField]
        private TextMeshProUGUI characterDefence;
        [SerializeField]
        private TextMeshProUGUI characterCritical;

        private void Start()
        {
            PlayerData player = PlayerDataManager.Singleton.player;

            Image playerObj = GameObject.Find("TON.Player").GetComponent<Image>();
            playerObj.sprite = AssetManager.Singleton.LoadPlayerIcon(player.type, out Sprite playerImage) ? playerImage : null;

            characterName.text = player.name;
            characterHp.text = $"{player.hp}";
            characterMp.text = $"{player.mp}";
            characterAttck.text = $"{player.attackPower}";
            characterDefence.text = $"{player.defensivePower}";
            characterCritical.text = $"{player.critical}";
        }

        public void OnClickStageEntryButton()
        {
            UIManager.Hide<LobbyUI>(UIList.LobbyUI);
            UIManager.Show<StageEntryUI>(UIList.StageEntryUI);

        }

        public void OnClickSkillSettingButton()
        {
            UIManager.Show<SkillSettingUI>(UIList.SkillSettingUI);
        }

        public void OnClickShopButton()
        {
            // TODO: 상점 UI 추가
            // UIManager.Show<ShopUI>(UIList.ShopUI);
        }
    }
}
