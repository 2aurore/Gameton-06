using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class LobbyUI : UIBase
    {
        public void OnClickStageEntryButton()
        {
            UIManager.Hide<LobbyUI>(UIList.LobbyUI);
            UIManager.Show<StageEntryUI>(UIList.StageEntryUI);
        }
    }
}
