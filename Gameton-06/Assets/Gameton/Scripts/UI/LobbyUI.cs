using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
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

        public List<LobbyUI_StagePage> stagePages = new List<LobbyUI_StagePage>();
        public GameObject stagePagePrefab;
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
            playerObj.sprite = AssetManager.Singleton.LoadPlayerIcon(player.type, FaceStatue.Idle, out Sprite playerImage) ? playerImage : null;

            characterName.text = player.name;
            characterHp.text = $"{player.hp}";
            characterMp.text = $"{player.mp}";
            characterAttck.text = $"{player.attackPower}";
            characterDefence.text = $"{player.defensivePower}";
            characterCritical.text = $"{player.critical}";
        }

        private void SetStageData()
        {
            if (stagePages.Count > 0)
            {
                foreach (var stagePage in stagePages)
                {
                    Destroy(stagePage.gameObject);
                }
                stagePages.Clear();
            }


            for (int i = 0; i < stageList.Count; i++)
            {
                string stageId = stageList[i];
                GameObject stagePageObject = Instantiate(stagePagePrefab, stagePageGroup);
                LobbyUI_StagePage stagePage = stagePageObject.GetComponent<LobbyUI_StagePage>();
                stagePageObject.SetActive(true);
                stagePage.Initalize(stageId, i);
                stagePages.Add(stagePage);
            }
        }

        public void OnClickChangeStageButton()
        {
            // TODO: 스테이지 입장 popupUI 비활성화
            stagePages.ForEach(page => page.GetComponent<LobbyUI_StagePage>().OnClickStageChangeButton());
        }

        public void OnClickStageButton()
        {
            // 스테이지 입장 UI 활성화
            GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
            LobbyUI_StagePage lobbyUI_StagePage = currentSelectedGameObject.GetComponentInParent<LobbyUI_StagePage>();

            PlayerPrefs.SetString("StageId", lobbyUI_StagePage.stageId);
            lobbyUI_StagePage.OnClickStageButton();
        }

        public void OnClickStagePlayButton()
        {
            // FIXME: 개발 편의를 위해 스테이지 입장시 하트 소모 로직 주석처리 
            // // 가지고 있는 하트가 없다면 입장 불가
            // if (HeartDataManager.Singleton.GetCurrentHearts() < 1)
            // {
            //     // TODO: 입장 불가 modal 출력
            //     Debug.Log("보유한 하트 없음");
            //     return;
            // }

            // // 입장 시 하트 소모
            // HeartDataManager.Singleton.UseHeart();

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
