using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class SkillSettingUI_SkillSlot : MonoBehaviour
    {
        public GameObject lockerImage;

        public void SelectedSlot()
        {
            lockerImage.SetActive(true);
        }

        public void UnselectedSlot()
        {
            lockerImage.SetActive(false);
        }
    }
}
