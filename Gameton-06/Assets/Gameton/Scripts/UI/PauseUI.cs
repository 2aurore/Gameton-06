using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TON
{
    public class PauseUI : UIBase
    {
        public static PauseUI Instance => UIManager.Singleton.GetUI<PauseUI>(UIList.PauseUI);

        public GameObject rechargeModal;
        public GameObject retryModal;
        public GameObject homeModal;

        [SerializeField] private TextMeshProUGUI fishAmount;


        void OnEnable()
        {
            InitModalActive();
            UpdateFishCount();
            // 일시정지 시 게임 일시정지
            Time.timeScale = 0f;
        }

        public void InitModalActive()
        {
            rechargeModal.SetActive(false);
            retryModal.SetActive(false);
            homeModal.SetActive(false);
        }

        public void UpdateFishCount()
        {
            fishAmount.text = string.Format("{0:#,###}", PlayerDataManager.Singleton.fishAmount);
        }

        // 컨티뉴 버튼 선택 시
        public void OnPressContinueButton()
        {
            // 게임 재생
            Time.timeScale = 1f;
            // UI 숨김
            UIManager.Hide<PauseUI>(UIList.PauseUI);
        }

        public void OnClickHomeModal()
        {
            homeModal.SetActive(true);
        }
        public void OnClickHomeButton()
        {
            Time.timeScale = 1f;
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

            OnClickStageRetryButton();
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
            // 보유한 생선 갯수를 소모하고 하트를 충전
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
                }
            });
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
            // TODO: 광고 시청 로직 추가, 광고 종료 후 하트 충전
            Debug.Log("OnClickAdButton::: ");

            // 광고 시청 종료 후 콜백
            rechargeModal.SetActive(false);
        }
    }
}
