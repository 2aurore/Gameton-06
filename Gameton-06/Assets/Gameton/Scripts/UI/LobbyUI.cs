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

        public List<string> stageList = new List<string> { "STG001", "STG002", "STG003", "STG004" };

        public LobbyUI_StagePage stagePagePrefab;
        public Transform stagePageGroup;

        private void Start()
        {
            SetCharacterData();
            SetStageData();
        }

        private void SetCharacterData()
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

        private void SetStageData()
        {
            stageList.ForEach(stageId =>
            {
                LobbyUI_StagePage stagePage = Instantiate(stagePagePrefab, stagePageGroup);
                stagePage.Initalize(stageId);
            });
        }

        public void OnClickChangeStageButton()
        {
            // TODO: 스테이지 입장 popupUI 비활성화
        }

        public void OnClickStageButton()
        {
            // TODO: 스테이지 선택 UI 활성화

        }

        public void OnClickStagePlayButton()
        {
            UIManager.Hide<LobbyUI>(UIList.LobbyUI);
            // TODO: 선택된 스테이지 아이디 불러오기
            // 선택한 스테이지 아이디 활용 필요
            Main.Singleton.ChangeScene(SceneType.Stage);
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
