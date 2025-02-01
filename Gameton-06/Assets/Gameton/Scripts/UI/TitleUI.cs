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

            List<PlayerData> players = JSONLoader.LoadFromResources<List<PlayerData>>("Player");

            if (players == null)
            {
                // 현재 가지고 있는 캐릭터가 없다면 CharaterCreateUI 를 보여주고
                UIManager.Show<CharaterCreateUI>(UIList.CharaterCreateUI);
            }
            else
            {
                // 캐릭터가 있다면 내 캐릭터 목록에서 선택할 수 있도록 함
                charaterSelectUI.SetPlayerDatas(players);
                UIManager.Show<CharaterSelectUI>(UIList.CharaterSelectUI);
            }
        }


        public void OnClickExitButton()
        {
            Main.Singleton?.SystemQuit();
        }
    }
}
