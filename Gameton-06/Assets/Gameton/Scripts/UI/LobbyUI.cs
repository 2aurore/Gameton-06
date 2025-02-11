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

        private void Start()
        {
            Image playerObj = GameObject.Find("TON.Player").GetComponent<Image>();
            PlayerData player = PlayerDataManager.Singleton.player;
            playerObj.sprite = playerImages.GetValueOrDefault(player.type);

            characterName.text = player.name;
        }

        public void OnClickStageEntryButton()
        {
            UIManager.Hide<LobbyUI>(UIList.LobbyUI);
            UIManager.Show<StageEntryUI>(UIList.StageEntryUI);

        }
    }
}
