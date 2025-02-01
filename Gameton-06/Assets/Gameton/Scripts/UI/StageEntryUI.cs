using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class StageEntryUI : UIBase
    {
        public void OnClickBackButton()
        {
            UIManager.Show<LobbyUI>(UIList.LobbyUI);
            UIManager.Hide<StageEntryUI>(UIList.StageEntryUI);
        }
    }
}
