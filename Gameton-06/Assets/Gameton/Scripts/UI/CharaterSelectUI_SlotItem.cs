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
        [SerializeField] private int index;

        private CharaterSelectUI charaterSelectUI;

        public void SetCharaterData(Sprite image, string name, int i)
        {
            character_image.sprite = image;
            charater_name.text = name;
            index = i;
        }

        private void Start()
        {
            // 상위 오브젝트에서 CharacterSelectUI 찾기
            charaterSelectUI = FindObjectOfType<CharaterSelectUI>();

            // 버튼 클릭 이벤트 등록
            GetComponent<Button>().onClick.AddListener(OnClickPlayer);
        }

        public void OnClickPlayer()
        {
            charaterSelectUI.SelectCharacter(index);
        }
    }
}
