using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TON
{
    public class OptionUI : UIBase
    {
        public GameObject goldObject;   // 골드
        public GameObject fishObject;   // 생선
        public GameObject settingObject;    // 더보기(옵션) 버튼
        public GameObject parseObject;  // 일시정지

        // 로비 : 골드/생선/인벤토리/더보기 버튼
        // 스테이지 : 인벤토리/일시정지 버튼
        // 상점 : 골드/생선
        private void OnEnable()
        {
            SetCashAmount();
            SetObjectActive();
        }

        public void SetCashAmount()
        {
            goldObject.GetComponentInChildren<TextMeshProUGUI>().text = $"{PlayerDataManager.Singleton.goldAmount:N0}";
            fishObject.GetComponentInChildren<TextMeshProUGUI>().text = $"{PlayerDataManager.Singleton.fishAmount:N0}";
        }

        private void SetObjectActive()
        {
            SceneType activeScene = Main.Singleton.currentSceneType;

            if (activeScene == SceneType.Lobby)
            {
                goldObject.SetActive(true);
                fishObject.SetActive(true);
                settingObject.SetActive(true);
                parseObject.SetActive(false);
            }
            else if (activeScene == SceneType.Stage)
            {
                goldObject.SetActive(false);
                fishObject.SetActive(false);
                settingObject.SetActive(false);
                parseObject.SetActive(true);
            }
            else if (activeScene == SceneType.Shop)
            {
                goldObject.SetActive(true);
                fishObject.SetActive(true);
                settingObject.SetActive(false);
                parseObject.SetActive(false);
            }
        }

        public void OnClickSettingButton()
        {
            Debug.Log("Setting Button Clicked");
            // UIManager.Show<SettingUI>(UIList.SettingUI);
        }

        public void OnClickPauseButton()
        {
            Debug.Log("Pause Button Clicked");
            UIManager.Show<PauseUI>(UIList.PauseUI);
        }
    }
}
