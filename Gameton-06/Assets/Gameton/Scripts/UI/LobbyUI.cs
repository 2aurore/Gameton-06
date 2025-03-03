using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TON
{
    public class LobbyUI : UIBase
    {
        [SerializeField] private TextMeshProUGUI characterName;
        [SerializeField] private TextMeshProUGUI characterLevel;
        [SerializeField] private TextMeshProUGUI characterHp;
        [SerializeField] private TextMeshProUGUI characterMp;
        [SerializeField] private TextMeshProUGUI characterAttck;
        [SerializeField] private TextMeshProUGUI characterDefence;
        [SerializeField] private TextMeshProUGUI characterCritical;
        [SerializeField] private TextMeshProUGUI characterExp;

        // 스테이지 클리어 기록 텍스트
        [SerializeField] private Image monsterIcon;
        [SerializeField] private TextMeshProUGUI wave;
        [SerializeField] private TextMeshProUGUI playTime;
        [SerializeField] private TextMeshProUGUI score;

        // 보유 포션 수량 
        [SerializeField] private TextMeshProUGUI hpPotionCount;
        [SerializeField] private TextMeshProUGUI mpPotionCount;

        public GameObject emptyHeartAlert;

        private void OnEnable()
        {
            SetCharacterData();
            SetUserItemData();
            SetUserRankData();
        }

        // 사용자 최고 기록 데이터 세팅
        private void SetUserRankData()
        {
            ClearData TOP_RECORD = StageManager.Singleton.TOP_RECORD;

            if (TOP_RECORD.wave > 0)
            {
                if (AssetManager.Singleton.LoadMonsterWaveIcon(TOP_RECORD.wave, out Sprite result))
                {
                    monsterIcon.sprite = result;
                }
                wave.text = $"{TOP_RECORD.wave}";
            }

            if (TOP_RECORD.score > 0)
            {
                score.text = $"{TOP_RECORD.score}";
            }

            if (TOP_RECORD.playTime > 0)
            {
                int minutes = Mathf.FloorToInt(TOP_RECORD.playTime / 60f);
                int seconds = Mathf.FloorToInt(TOP_RECORD.playTime % 60f);
                playTime.text = $"{minutes:0}m {seconds:0}s";
            }
        }

        // 캐릭터 기본 스탯 표시 
        private void SetCharacterData()
        {
            PlayerData player = PlayerDataManager.Singleton.player;

            Image playerObj = GameObject.Find("TON.Player").GetComponent<Image>();
            playerObj.sprite = AssetManager.Singleton.LoadPlayerIcon(player.type, FaceStatue.Idle, out Sprite playerImage) ? playerImage : null;

            characterName.text = player.name;
            characterHp.text = $"{player.hp}";
            characterMp.text = $"{player.mp}";
            characterLevel.text = $"Lv {player.level}";
            characterAttck.text = $"{player.attackPower}";
            characterDefence.text = $"{player.defensivePower}";
            characterCritical.text = $"{player.critical}";

            characterExp.text = $"{player.experience}/{PlayerDataManager.Singleton.requireLevelUpExp}";
        }

        // 사용자 보유 포션 수량 세팅
        private void SetUserItemData()
        {
            UserItemData userItem = PlayerDataManager.Singleton.userItem;

            hpPotionCount.text = $"{userItem.hpPotion}";
            mpPotionCount.text = $"{userItem.mpPotion}";
        }

        public void OnClickStagePlayButton()
        {
            PlayerPrefs.SetString("StageId", "STG004");

            // 가지고 있는 하트가 없다면 입장 불가
            if (HeartDataManager.Singleton.GetCurrentHearts() < 1)
            {
                // 입장 불가 modal 출력
                emptyHeartAlert.SetActive(true);
                // 입장 불가 modal 1초 후 숨김
                Invoke(nameof(EnactiveAlert), 1f);
                return;
            }

            // 입장 시 하트 소모
            HeartDataManager.Singleton.UseHeart();

            Main.Singleton.ChangeScene(SceneType.Stage);
        }

        private void EnactiveAlert()
        {
            emptyHeartAlert.SetActive(false);
        }

        public void OnClickSkillSettingButton()
        {
            UIManager.Show<SkillSettingUI>(UIList.SkillSettingUI);
        }

        public void OnClickRankingButton()
        {
            // 랭킹 UI 추가
            UIManager.Show<RankingUI>(UIList.RankingUI);
        }

        public void OnClickShopButton()
        {
            // TODO: 상점 UI 추가
            // 상점은 Scene으로 전환하자.
        }

        public void OnClickRouletteButton()
        {
            // TODO: 룰렛 UI 추가가
        }

        public void OnClickGuideButton()
        {
            UIManager.Show<GuideUI>(UIList.GuideUI);
        }
    }
}
