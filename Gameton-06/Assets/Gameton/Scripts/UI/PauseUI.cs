using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TON
{
    public class PauseUI : UIBase
    {

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

        public void OnClickHomeButton()
        {
            Time.timeScale = 1f;
            Main.Singleton.ChangeScene(SceneType.Lobby);
        }

        public void OnClickStageRetryButton()
        {
            // TODO: 스테이지 재시작 로직 추가
            Time.timeScale = 1f;
            Debug.Log("OnClickStageRetryButton");
        }
    }
}
