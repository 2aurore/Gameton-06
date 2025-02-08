using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TON
{
    public class ControllerUI_SkillButton : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI coolTimeText;
        [SerializeField] private Image coolTimeDimd;
        [SerializeField] private GameObject skillIcon;
        [SerializeField] private GameObject lockImage;

        [SerializeField]
        private SerializableDictionary<string, Sprite> skillSprite = new SerializableDictionary<string, Sprite>();

        public string skillId;

        public float maxCooldown;



        public void Initalize(SkillData skillData)
        {
            skillId = skillData.id;
            maxCooldown = skillData.coolDown;
            InitSkillData(skillData);
            skillIcon.SetActive(true);
            skillIcon.GetComponent<Image>().sprite = skillSprite.GetValueOrDefault(skillData.id, null);
            lockImage.SetActive(false);

        }

        public void SetCoolTime(float remain)
        {
            coolTimeText.gameObject.SetActive(remain > 0f);
            coolTimeText.text = $"{Mathf.CeilToInt(remain)}s";
            coolTimeDimd.fillAmount = remain / maxCooldown;

        }
        private SkillBase InitSkillData(SkillData skillData)
        {
            switch (skillData.id)
            {
                case "K0001":
                    var fireBall = gameObject.AddComponent<FireBall>();
                    fireBall.Init(skillData);
                    return fireBall;
                case "K0002":
                    var iceBall = gameObject.AddComponent<IceBall>();
                    iceBall.Init(skillData);
                    return iceBall;
                case "K0003":
                    return null;
                case "K0004":
                    return null;
                case "K0005":
                    return null;
                case "K0006":
                    return null;
                case "K0007":
                    return null;
                case "K0008":
                    return null;
                default:
                    return null;
            }
        }


    }


}
