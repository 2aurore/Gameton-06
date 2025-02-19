using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TON
{
    public class GameoverUI : UIBase
    {

        public static GameoverUI Instance => UIManager.Singleton.GetUI<GameoverUI>(UIList.GameOverUI);

        public GameObject rechargeModal;



        private void OnEnable()
        {
            rechargeModal.SetActive(false);
        }

        public void OnClickHomeButton()
        {
            UIManager.Hide<GameoverUI>(UIList.GameOverUI);

            Main.Singleton.ChangeScene(SceneType.Lobby);
        }

        public void OnClickRechargeButton()
        {
            rechargeModal.SetActive(true);
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
            // TODO: 광고 시청 로직 추가, 광고 종료 후 하트 충전
            Debug.Log("OnClickAdButton::: ");
        }

    }
}
