using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TON
{
    public class SkillSettingUI_SkillSlot : MonoBehaviour
    {
        public GameObject skillImage;
        public GameObject lockerImage;

        public void Initalize()
        {
            // 스킬 이미지 세팅하기
            // skillImage = GetComponent<GameObject>();
        }

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
