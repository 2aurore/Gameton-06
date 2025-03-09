using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class TitleUI : UIBase
    {

        public void OnClickStartButton()
        {
            StartCoroutine(StartButtonDelay());
        }

        IEnumerator StartButtonDelay()
        {
            yield return new WaitForSeconds(0.2f); // 0.2초 대기

            UIManager.Hide<TitleUI>(UIList.TitleUI);

            // 플레이어가 가지고 있는 캐릭터들의 데이터 불러옴
            PlayerData player = PlayerDataManager.Singleton.player;

            if (player == null)
            {
                // 현재 가지고 있는 캐릭터가 없다면 캐릭터 생성 화면으로 이동
                UIManager.Show<CharaterCreateUI>(UIList.CharaterCreateUI);
            }
            else
            {
                HeartDataManager.Singleton.Initalize(() =>
                {
                    Main.Singleton.ChangeScene(SceneType.Lobby);
                });
            }
        }

        public void OnClickExitButton()
        {
            Main.Singleton.SystemQuit();
        }
    }
}
