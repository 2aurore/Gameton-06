using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TON
{
    public class ControllerUI_SkillButton : MonoBehaviour
    {

        public SkillData skillData;
        [SerializeField] private TextMeshProUGUI coolTimeText;
        [SerializeField] private Image coolTimeDimd;
        [SerializeField] private GameObject skillIcon;
        [SerializeField] private GameObject lockImage;

        [SerializeField]
        private SerializableDictionary<string, Sprite> skillSprite = new SerializableDictionary<string, Sprite>();

        public void Initalize(SkillData skillData)
        {
            Debug.Log($"Initalize :: {skillData.id}");
            this.skillData = skillData;
            skillIcon.SetActive(true);
            skillIcon.GetComponent<Image>().sprite = skillSprite.GetValueOrDefault(skillData.id, null);
            lockImage.SetActive(false);

        }

        public void SetCoolTime(float remain, float max)
        {
            coolTimeText.gameObject.SetActive(remain > 0f);
            coolTimeText.text = $"{Mathf.CeilToInt(remain)}s";
            coolTimeDimd.fillAmount = remain / max;
        }

    }


}
