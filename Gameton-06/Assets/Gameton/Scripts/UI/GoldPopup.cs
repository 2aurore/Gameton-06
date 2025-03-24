using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class GoldPopup : UIBase
    {
        public static GoldPopup Instance => UIManager.Singleton.GetUI<GoldPopup>(UIList.GoldPopup);

        private void OnEnable()
        {
            Invoke(nameof(HidePopup), 3f);
        }

        private void HidePopup()
        {
            UIManager.Hide<GoldPopup>(UIList.GoldPopup);
        }

    }
}
