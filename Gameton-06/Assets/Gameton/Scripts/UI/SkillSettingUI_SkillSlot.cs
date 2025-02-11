using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace TON
{
    public class SkillSettingUI_SkillSlot : MonoBehaviour
    {
        public GameObject skillImage;
        public GameObject lockerImage;
        public GameObject selectedState;

        public void Initalize(string skillId)
        {
            // 스킬 이미지 세팅하기
            Assert.IsTrue(AssetManager.Singleton.LoadSkillIcon(skillId, out Sprite loadedSkillImage));
            skillImage.SetActive(true);
            skillImage.GetComponent<Image>().sprite = loadedSkillImage;
            lockerImage.SetActive(false);
        }

        public void SelectedSlot()
        {
            selectedState.SetActive(true);
        }

        public void UnselectedSlot()
        {
            selectedState.SetActive(false);
        }
    }
}
