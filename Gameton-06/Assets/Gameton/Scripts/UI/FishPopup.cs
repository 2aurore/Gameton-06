using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class FishPopup : UIBase
    {
        private void OnEnable()
        {
            Invoke(nameof(HidePopup), 3f);
        }

        private void HidePopup()
        {
            UIManager.Hide<FishPopup>(UIList.FishPopup);
        }
    }
}
