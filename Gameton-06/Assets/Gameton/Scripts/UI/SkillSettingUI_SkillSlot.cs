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

        private int slotIndex;

        public void Initalize(string skillId, int index)
        {
            slotIndex = index;
            // 스킬 이미지 세팅하기
            if (skillId != null)    // 스킬 슬롯에 스킬이 지정된 경우
            {
                // out 으로 받을 변수 초기화
                Sprite loadedSkillImage = null;
                Assert.IsTrue(AssetManager.Singleton.LoadSkillIcon(skillId, out loadedSkillImage));
                skillImage.SetActive(true);
                skillImage.GetComponent<Image>().sprite = loadedSkillImage;
            }

            lockerImage.SetActive(false);
        }

        public int SelectedSlot()
        {
            selectedState.SetActive(true);
            return slotIndex;
        }

        public void UnselectedSlot()
        {
            selectedState.SetActive(false);
        }
    }
}
