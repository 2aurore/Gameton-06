using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TON
{

    public class GameWinUI : UIBase
    {
        public static GameWinUI Instance => UIManager.Singleton.GetUI<GameWinUI>(UIList.GameWinUI);

        public readonly string GAME_OVER = "£ GAME OVER £";
        public readonly string YOU_WIN = "♧ YOU WIN ♧";

        public GameObject rechargeModal;
        public GameObject retryModal;
        public GameObject homeModal;

        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI goldReward;
        [SerializeField] private TextMeshProUGUI expReward;
        [SerializeField] private TextMeshProUGUI wave;
        [SerializeField] private TextMeshProUGUI playTime;
        [SerializeField] private TextMeshProUGUI score;
        [SerializeField] private GameObject levelUpText;

        private void OnEnable()
        {
            InitModalActive();

            SetUITextMesh();
            // 해당 UI 노출과 함께 게임 클리어 정보 저장
            StageManager.Singleton.StageClear();

            // 현재 획득한 경험치로 인한 레벨업 처리
            SetLevelUpText(StageManager.Singleton.expReward);
            // TODO: 획득한 아이템 정보 저장 로직 구현


        }

        public void InitModalActive()
        {
            levelUpText.SetActive(false);
            rechargeModal.SetActive(false);
            retryModal.SetActive(false);
            homeModal.SetActive(false);
        }

        public void SetUITextMesh()
        {
            title.text = (StageManager.Singleton.waveCount == 10) ? YOU_WIN : GAME_OVER;
            wave.text = $"{StageManager.Singleton.waveCount} wave";
            goldReward.text = $"{StageManager.Singleton.goldReward} G";
            expReward.text = $"EXP {StageManager.Singleton.expReward}";
            playTime.text = $"{StageManager.Singleton.PlayTime / 60}m {StageManager.Singleton.PlayTime % 60:D2}s";
        }

        // 경험치 추가 및 레벨업 처리
        public void SetLevelUpText(int amount)
        {
            bool leveledUp = PlayerDataManager.Singleton.UpdateExpericence(amount);

            if (leveledUp)
            {
                levelUpText.SetActive(true);
            }

            // 경험치와 변경된 데이터를 파일에 업데이트 한다.
            PlayerDataManager.Singleton.UpdatePlayerData();
        }

        public void OnClickHomeModal()
        {
            homeModal.SetActive(true);
        }
        public void OnClickHomeButton()
        {
            Main.Singleton.ChangeScene(SceneType.Lobby);
        }

        public void OnClickStageRetryModal()
        {
            // 가지고 있는 하트가 없다면 입장 불가
            if (HeartDataManager.Singleton.GetCurrentHearts() < 1)
            {
                Time.timeScale = 0f;
                // 하트 충전 modal 출력
                rechargeModal.SetActive(true);
                return;
            }

            retryModal.SetActive(true);
        }
        public void OnClickStageRetryButton()
        {
            Time.timeScale = 1f;
            // 입장 시 하트 소모
            HeartDataManager.Singleton.UseHeart();

            Main.Singleton.ChangeScene(SceneType.Stage);
        }

        public void OnClickCloseButton(string keyword)
        {
            switch (keyword)
            {
                case "recharge":
                    rechargeModal.SetActive(false);
                    break;
                case "retry":
                    retryModal.SetActive(false);
                    break;
                case "home":
                    homeModal.SetActive(false);
                    break;
            }
        }

        public void OnClickUseCashButton(int count)
        {
            // TODO: 보유한 생선 갯수를 소모하고 하트를 충전
            Debug.Log("OnClickUseCashButton::: fish :: " + count);
        }

        public void OnClickAdButton()
        {
            // TODO: 광고 시청 로직 추가, 하트를 충전
            Debug.Log("OnClickAdButton::: ");
        }

        public void OnClickAdForGoldButton()
        {
            // TODO: 광고 시청 로직 추가, 골드 보상 2배 적용 후 Lobby로 이동
            Debug.Log("OnClickAdButton::: ");
        }

    }
}
