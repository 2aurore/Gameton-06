using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class TitleUI : UIBase
    {
        public CharaterSelectUI charaterSelectUI;

        public void OnClickStartButton()
        {
            // Main.Singleton?.ChangeScene(SceneType.Ingame);
            UIManager.Hide<TitleUI>(UIList.TitleUI);

            // 플레이어가 가지고 있는 캐릭터들의 데이터 불러옴
            List<PlayerData> players = PlayerDataManager.Singleton.playersData;

            if (players.Count == 0)
            {
                // 현재 가지고 있는 캐릭터가 없다면 CharaterCreateUI 를 보여주고
                UIManager.Show<CharaterCreateUI>(UIList.CharaterCreateUI);
            }
            else
            {
                // 캐릭터가 있다면 내 캐릭터 목록에서 선택할 수 있도록 함
                // UIManager.Show<CharaterSelectUI>(UIList.CharaterSelectUI);

                // select 요소는 나중에 인게임 화면으로 바로 전환
                PlayerPrefs.SetInt("SelectedPlayerIndex", 0);
                PlayerDataManager.Singleton.SetCurrentUserData();
                Main.Singleton?.ChangeScene(SceneType.Lobby);
            }
        }


        public void OnClickExitButton()
        {
            Main.Singleton?.SystemQuit();
        }
    }
}
