using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public enum UIList
    {
        PANEL_START,

        TitleUI,    // 게임 시작 화면 UI
        CharaterCreateUI,   // 캐릭터 생성 UI
        CharaterSelectUI,
        ControllerUI,   // 캐릭터 컨트롤러 UI
        LobbyUI,        // 게임 로비 UI
        SkillSettingUI,     // 스킬 세팅 팝업 UI
        RankingUI,      // 랭킹 UI
        IngameUI,       // 인게임 표시 UI
        OptionUI, // 화면 우측 상단 골드, 인벤토리, 옵션 버튼 UI
        ShopUI, // 상점
        PANEL_END,


        POPUP_START,
        LoadingUI,

        GameWinUI,  // 게임 오버 및 클리어 시 노출되는 UI
        PauseUI,    // 일시중지 버튼 선택 시 노출되는 UI
        GoldPopup,

        POPUP_END,
    }
}
