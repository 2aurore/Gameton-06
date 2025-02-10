using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TON
{
    public class SkillInformationItem : MonoBehaviour
    {
        [SerializeField]
        SerializableDictionary<string, Sprite> imageSprites = new SerializableDictionary<string, Sprite>();

        public Image skillImage;
        public TextMeshProUGUI skillName;
        public TextMeshProUGUI skillDamage;
        public TextMeshProUGUI skillCooltime;
        public TextMeshProUGUI skillReqMp;
        public TextMeshProUGUI skillReqLv;

        public void Initalize(SkillData skillData)
        {
            skillName.text = skillData.name;
            skillDamage.text = $"{skillData.damage}";
            skillCooltime.text = $"{skillData.coolDown}";
            skillReqMp.text = $"{skillData.mpConsumption}";
            skillReqLv.text = $"{skillData.requiredLevel}";
        }

    }
}
