using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TON
{
    public class LobbyUI : UIBase
    {
        public SerializableDictionary<string, Sprite> playerImages;
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
            Image playerObj = GameObject.Find("TON.Player").GetComponent<Image>();
            PlayerData player = PlayerDataManager.Singleton.player;
            playerObj.sprite = playerImages.GetValueOrDefault(player.type);

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
    }
}
