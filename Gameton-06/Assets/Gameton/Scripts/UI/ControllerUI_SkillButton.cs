using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TON
{
    public class ControllerUI_SkillButton : MonoBehaviour
    {
        public CharacterBase linkedCharactor { get; set; }

        public SkillData skillData;

        public Image lockImage;

        public void Initalize(SkillData skillData)
        {
            this.skillData = skillData;
            // this.linkedCharactor = linkedCharactor; // 씬에서 캐릭터 찾기
            Debug.Log(linkedCharactor.name);
        }

        public void OnClickSkillButton()
        {
            linkedCharactor.SkillAttack(skillData.id);
        }
    }


}
