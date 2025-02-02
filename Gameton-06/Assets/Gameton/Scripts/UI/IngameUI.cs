using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TON
{
    public class IngameUI : UIBase
    {
        public static IngameUI Instance => UIManager.Singleton.GetUI<IngameUI>(UIList.IngameUI);

        public Image hpBar;
        public Image spBar;

        public TextMeshProUGUI hpText;
        public TextMeshProUGUI spText;

        public void SetHP(float current, float max)
        {
            hpBar.fillAmount = current / max;
            hpText.text = $"{current:0} / {max: 0}";
        }

        public void SetSP(float current, float max)
        {
            spBar.fillAmount = current / max;
            spText.text = $"{current:0} / {max: 0}";
        }



        public void OnPressPauseButton()
        {
            UIManager.Show<PauseUI>(UIList.PauseUI);

            Debug.Log("버튼 클릭");

            // UIManager.Show<PauseUI>(UIList.PauseUI);


            // 게임 일시 정지
            // Time.timeScale = 0f;
        }

    }
}
