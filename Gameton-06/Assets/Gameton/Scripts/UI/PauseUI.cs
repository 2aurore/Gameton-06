using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TON
{
    public class PauseUI : UIBase
    {
        // 컨티뉴 버튼 선택 시
        public void OnPressContinueButton()
        {
            // UI 숨김
            UIManager.Hide<PauseUI>(UIList.PauseUI);
            // 게임 재생
            Time.timeScale = 1f;
        }
    }
}
