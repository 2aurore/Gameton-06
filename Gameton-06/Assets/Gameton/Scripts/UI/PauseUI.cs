using System.Collections;
using System.Collections.Generic;
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

        void OnEnable()
        {
            // 일시정지 시 게임 일시정지
            Time.timeScale = 0f;
        }

        // 컨티뉴 버튼 선택 시
        public void OnPressContinueButton()
        {
            // UI 숨김
            UIManager.Hide<PauseUI>(UIList.PauseUI);
            // 게임 재생
            Time.timeScale = 1f;
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
            retryModal.SetActive(true);
        }
        public void OnClickStageRetryButton()
        {
            // 가지고 있는 하트가 없다면 입장 불가
            if (HeartDataManager.Singleton.GetCurrentHearts() < 1)
            {
                Time.timeScale = 0f;
                // 하트 충전 modal 출력
                rechargeModal.SetActive(true);
                return;
            }

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
            // TODO: 광고 시청 로직 추가, 광고 종료 후 하트 충전
            Debug.Log("OnClickAdButton::: ");
        }
    }
}
