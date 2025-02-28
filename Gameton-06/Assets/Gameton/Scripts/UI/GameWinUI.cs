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

        [SerializeField] private GameObject rechargeModal;
        [SerializeField] private GameObject retryModal;
        [SerializeField] private GameObject fishPopup;

        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI goldReward;
        [SerializeField] private TextMeshProUGUI expReward;
        [SerializeField] private TextMeshProUGUI wave;
        [SerializeField] private TextMeshProUGUI playTime;
        [SerializeField] private TextMeshProUGUI score;
        [SerializeField] private GameObject levelUpText;
        [SerializeField] private TextMeshProUGUI fishAmount;

        private Coroutine autoCloseCoroutine;   // 팝업 닫기 코루틴 저장


        private int goldAmount = 0;     // 광고 보상 수령 후 초기화

        private void OnEnable()
        {
            Time.timeScale = 0f;
            goldAmount = StageManager.Singleton.goldReward;

            InitModalActive();

            SetUITextMesh();
            UpdateFishCount();

            // 해당 UI 노출과 함께 게임 클리어 정보 저장
            StageManager.Singleton.StageClear();

            // 현재 획득한 경험치로 인한 레벨업 처리
            SetLevelUpText(StageManager.Singleton.expReward);
            // 획득한 골드 정보 저장
            PlayerDataManager.Singleton.AddGold(goldAmount);
        }

        public void InitModalActive()
        {
            levelUpText.SetActive(false);
            rechargeModal.SetActive(false);
            retryModal.SetActive(false);
            fishPopup.SetActive(false);
        }

        public void SetUITextMesh()
        {
            title.text = (StageManager.Singleton.waveCount == 10) ? YOU_WIN : GAME_OVER;
            wave.text = $"{StageManager.Singleton.waveCount} wave";
            goldReward.text = $"{goldAmount} G";
            expReward.text = $"EXP {StageManager.Singleton.expReward}";
            score.text = $"{StageManager.Singleton.gameScore}";

            float clearTime = StageManager.Singleton.PlayTime;
            playTime.text = $"{(int)(clearTime / 60)}m {(int)(clearTime % 60)}s";
        }

        public void UpdateFishCount()
        {
            fishAmount.text = $"{PlayerDataManager.Singleton.fishAmount:N0}";
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

        public void OnClickHomeButton()
        {
            Time.timeScale = 1f;
            UIManager.Hide<GameWinUI>(UIList.GameWinUI);
            Main.Singleton.ChangeScene(SceneType.Lobby);
        }

        public void OnClickStageRetryModal()
        {
            // 가지고 있는 하트가 없다면 입장 불가
            if (HeartDataManager.Singleton.GetCurrentHearts() < 1)
            {
                // 하트 충전 modal 출력
                rechargeModal.SetActive(true);
                return;
            }
            OnClickStageRetryButton();
        }
        public void OnClickStageRetryButton()
        {
            Time.timeScale = 1f;
            // 입장 시 하트 소모
            HeartDataManager.Singleton.UseHeart();

            UIManager.Hide<GameWinUI>(UIList.GameWinUI);
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
            }
        }

        public void OnClickUseCashButton(int count)
        {
            // 보유한 생선 갯수를 소모하고 하트를 충전
            Debug.Log("OnClickUseCashButton::: fish :: " + count);
            PlayerDataManager.Singleton.UseFish(count, (isSuccess) =>
            {
                if (isSuccess)
                {
                    UpdateFishCount();
                    // 충전을 완료하고 modal 닫기
                    rechargeModal.SetActive(false);

                    AddHeart(count);
                }
                else
                {
                    // 생선 재화 사용 불가 팝업
                    Debug.Log("생선 재화 사용 불가 팝업");

                    ShowTimedPopup();
                }
            });
        }
        // UI 버튼에 연결할 메서드
        public void ShowTimedPopup()
        {
            // 이미 실행 중인 코루틴이 있다면 중지
            if (autoCloseCoroutine != null)
            {
                StopCoroutine(autoCloseCoroutine);
            }

            // UI 표시
            fishPopup.SetActive(true);

            // 자동 닫기 코루틴 시작
            autoCloseCoroutine = StartCoroutine(AutoClosePopup());
        }

        private IEnumerator AutoClosePopup()
        {
            // Time.timeScale의 영향을 받지 않는 대기
            yield return new WaitForSecondsRealtime(3f);

            // 시간이 지나면 UI 닫기
            fishPopup.SetActive(false);
            autoCloseCoroutine = null;
        }

        private static void AddHeart(int count)
        {
            switch (count)
            {
                case 5:
                    // 하트 1개 충전
                    HeartDataManager.Singleton.AddHeart(1);
                    break;
                case 45:
                    // 하트 10개 충전
                    HeartDataManager.Singleton.AddHeart(10);
                    break;
            }
        }

        public void OnClickAdButton()
        {
            // TODO: 광고 시청 로직 추가, 하트를 충전
            Debug.Log("OnClickAdButton::: ");


            // 광고 시청 종료 후 콜백
            rechargeModal.SetActive(false);
        }

        public void OnClickAdForGoldButton()
        {
            // TODO: 광고 시청 로직 추가, 골드 보상 2배 적용 후 Lobby로 이동
            Debug.Log("OnClickAdButton::: ");

            PlayerDataManager.Singleton.AddGold(goldAmount);
            goldAmount = 0;

            OnClickHomeButton();
        }

    }
}
