using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public enum UIList
    {
        PANEL_START,

        TitleUI,
        CharaterCreateUI,
        CharaterSelectUI,
        IngameUI,
        ControllerUI,
        LobbyUI,
        LogUI,
        
        
        GameOverUI, // 게임 오버 시 노출되는 UI
        GameWinUI,  // 게임 클리어 시 노출되는 UI
        PauseUI,    // 일시중지 버튼 선택 시 노출되는 UI
        IngameOptionUI, // 화면 우측 상단 골드, 인벤토리, 옵션 버튼 UI


        PANEL_END,

        POPUP_START,
        PausePopupUI,
        LoadingUI,
        StageEntryUI,



        POPUP_END,
    }
}
