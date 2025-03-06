using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace TON
{
    public class SkillInformationItem : MonoBehaviour
    {
        public GameObject skillImage;
        public TextMeshProUGUI skillName;
        public TextMeshProUGUI skillDamage;
        public TextMeshProUGUI skillCooltime;
        public TextMeshProUGUI skillReqMp;
        public TextMeshProUGUI skillReqLvTitle;
        public TextMeshProUGUI skillReqLv;

        public GameObject locker;
        public GameObject selectedState;

        public string skillId;


        public void Initalize(SkillData skillData, int playerLevel)
        {
            skillId = skillData.id;

            skillName.text = skillData.name;
            skillDamage.text = $"{skillData.damage}";
            skillCooltime.text = $"{skillData.coolDown}";
            skillReqMp.text = $"{skillData.mpConsumption}";
            skillReqLv.text = $"{skillData.requiredLevel}";

            // out 으로 받을 변수 초기화
            Sprite loadedSkillImage = null;
            if(AssetManager.Singleton.LoadSkillIcon(skillData.id, out loadedSkillImage))
            {
                skillImage.SetActive(true);
                skillImage.GetComponent<Image>().sprite = loadedSkillImage;
            }

            if (playerLevel >= skillData.requiredLevel)
            {
                locker.SetActive(false);
            }
            else
            {
                skillReqLvTitle.color = Color.red;
                skillReqLv.color = Color.red;
            }
        }

        public string SelectedSkillInfo()
        {
            selectedState.SetActive(true);
            return skillId;
        }

        public void UnselectedSkillInfo()
        {
            selectedState.SetActive(false);
        }

    }
}
