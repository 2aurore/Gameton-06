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
        [SerializeField]
        SerializableDictionary<string, Sprite> imageSprites = new SerializableDictionary<string, Sprite>();

        public GameObject skillImage;
        public TextMeshProUGUI skillName;
        public TextMeshProUGUI skillDamage;
        public TextMeshProUGUI skillCooltime;
        public TextMeshProUGUI skillReqMp;
        public TextMeshProUGUI skillReqLvTitle;
        public TextMeshProUGUI skillReqLv;

        public GameObject locker;


        public void Initalize(SkillData skillData, int playerLevel)
        {
            skillName.text = skillData.name;
            skillDamage.text = $"{skillData.damage}";
            skillCooltime.text = $"{skillData.coolDown}";
            skillReqMp.text = $"{skillData.mpConsumption}";
            skillReqLv.text = $"{skillData.requiredLevel}";

            Assert.IsTrue(AssetManager.Singleton.LoadSkillIcon(skillData.id, out Sprite loadedSkillImage));
            skillImage.SetActive(true);
            skillImage.GetComponent<Image>().sprite = loadedSkillImage;

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

    }
}
