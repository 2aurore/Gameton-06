using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TON
{
    public class CharaterSelectUI_SlotItem : MonoBehaviour
    {
        [SerializeField] private Image character_image;
        [SerializeField] private TextMeshProUGUI charater_name;

        public void SetCharaterData(Sprite image, string name)
        {
            character_image.sprite = image;
            charater_name.text = name;
        }

    }
}
