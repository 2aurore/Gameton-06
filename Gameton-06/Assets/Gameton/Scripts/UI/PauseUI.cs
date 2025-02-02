using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TON
{
    public class PauseUI : UIBase
    {
        public void OnPressPause()
        {
            // UIManager.Show<PauseUI>(UIList.PauseUI);
           
            Debug.Log("버튼 클릭");
            
            
            // 게임 일시 정지
            // Time.timeScale = 0f;
        }
    }
}
