using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class GoldPopup : UIBase
    {
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
