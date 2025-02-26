using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TON
{
    public class OptionUI : UIBase
    {
        public GameObject goldObject;   // 골드
        public GameObject fishObject;   // 생선
        public GameObject inventoryObject;  // 인벤토리 버튼
        public GameObject settingObject;    // 더보기(옵션) 버튼
        public GameObject parseObject;  // 일시정지


        // 로비 : 골드/생선/인벤토리/더보기 버튼
        // 스테이지 : 인벤토리/일시정지 버튼
        // 상점 : 골드/생선
        private void OnEnable()
        {
            // Scene activeScene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
            SceneType activeScene = Main.Singleton.currentSceneType;

            if (activeScene == SceneType.Lobby)
            {
                goldObject.SetActive(true);
                fishObject.SetActive(true);
                inventoryObject.SetActive(true);
                settingObject.SetActive(true);
                parseObject.SetActive(false);
            }
            else if (activeScene == SceneType.Stage)
            {
                goldObject.SetActive(false);
                fishObject.SetActive(false);
                inventoryObject.SetActive(false);
                settingObject.SetActive(false);
                parseObject.SetActive(true);
            }
            else if (activeScene == SceneType.Shop)
            {
                goldObject.SetActive(true);
                fishObject.SetActive(true);
                inventoryObject.SetActive(false);
                settingObject.SetActive(false);
                parseObject.SetActive(false);
            }

        }

        public void OnClickSettingButton()
        {
            Debug.Log("Setting Button Clicked");
            // UIManager.Show<SettingUI>(UIList.SettingUI);
        }

        public void OnClickInventoryButton()
        {
            Debug.Log("Inventory Button Clicked");
            // UIManager.Show<InventoryUI>(UIList.InventoryUI);
        }

        public void OnClickPauseButton()
        {
            Debug.Log("Pause Button Clicked");
            UIManager.Show<PauseUI>(UIList.PauseUI);
        }
    }
}
