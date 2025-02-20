using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class GameWinUI : UIBase
    {
        public static GameWinUI Instance => UIManager.Singleton.GetUI<GameWinUI>(UIList.GameWinUI);

        public GameObject rechargeModal;

        private void OnEnable()
        {
            rechargeModal.SetActive(false);

            // TODO: 획득한 아이템 리스트 출력 로직 구현

            // TODO: 획득한 아이템 정보 저장 로직 구현


        }

        public void OnClickHomeButton()
        {
            Main.Singleton.ChangeScene(SceneType.Lobby);
        }

        public void OnClickRetryButton()
        {
            // 가지고 있는 하트가 없다면 입장 불가
            if (HeartDataManager.Singleton.GetCurrentHearts() < 1)
            {
                // 하트 충전 modal 출력
                rechargeModal.SetActive(true);
                return;
            }

            // 입장 시 하트 소모
            HeartDataManager.Singleton.UseHeart();

            Main.Singleton.ChangeScene(SceneType.Stage);
        }

        public void OnClickCloseButton()
        {
            rechargeModal.SetActive(false);
        }

        public void OnClickUseCashButton(int count)
        {
            // TODO: 보유한 생선 갯수를 소모하고 하트를 충전
            Debug.Log("OnClickUseCashButton::: fish :: " + count);
        }

        public void OnClickAdButton()
        {
            // TODO: 광고 시청 로직 추가, 골드 보상 2배 적용 후 Lobby로 이동
            Debug.Log("OnClickAdButton::: ");
        }

        public void OnClickAdForGoldButton()
        {
            // TODO: 광고 시청 로직 추가, 골드 보상 2배 적용 후 Lobby로 이동
            Debug.Log("OnClickAdButton::: ");
        }

    }
}
