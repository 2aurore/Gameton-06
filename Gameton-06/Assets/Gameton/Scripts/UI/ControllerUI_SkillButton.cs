using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TON
{
    public class ControllerUI_SkillButton : MonoBehaviour
    {

        public SkillData skillData;
        public GameObject skillImage;
        public GameObject lockImage;

        [SerializeField]
        private SerializableDictionary<string, Sprite> skillSprite = new SerializableDictionary<string, Sprite>();

        public void Initalize(SkillData skillData)
        {
            Debug.Log($"Initalize :: {skillData.id}");
            this.skillData = skillData;
            skillImage.SetActive(true);
            skillImage.GetComponent<Image>().sprite = skillSprite.GetValueOrDefault(skillData.id, null);
            lockImage.SetActive(false);

        }


    }


}
