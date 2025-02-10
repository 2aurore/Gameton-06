using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TON
{
    public class ControllerUI : UIBase
    {
        public static ControllerUI Instance => UIManager.Singleton.GetUI<ControllerUI>(UIList.ControllerUI);

        /// <summary> 조이스틱에서 컨트롤된 x 값을 Player에서 사용할 수 있도록 객체화 </summary>
        public VariableJoystick joystick;
        public CharacterBase linkedCharactor { get; set; }


        public Transform skillButtonGroup;
        public ControllerUI_SkillButton skillButtonPrefab;
        private List<ControllerUI_SkillButton> createdSkillButtons = new List<ControllerUI_SkillButton>();

        private void Awake()
        {
            skillButtonPrefab.gameObject.SetActive(false);
        }

        public void Initalize()
        {
            // 이미 기존에 UI가 생성되어 있다면 삭제
            if (createdSkillButtons.Count > 0)
            {
                foreach (var button in createdSkillButtons)
                {
                    Destroy(button.gameObject);
                }
                createdSkillButtons.Clear();
            }

            // 스킬 버튼을 생성
            List<SkillBase> activatedSkills = SkillDataManager.Singleton.GetActiveSkillInstance();
            for (int i = 0; i < 3; i++)
            {
                ControllerUI_SkillButton newSkillButton = Instantiate(skillButtonPrefab, skillButtonGroup);
                newSkillButton.gameObject.SetActive(true);

                if (i < activatedSkills.Count) // 해당 인덱스에 활성화된 스킬이 있을 경우
                {
                    newSkillButton.Initalize(activatedSkills[i]);
                }
                else
                {
                    // 복제 됐을때 기본 상태가 잠금 상태 
                }

                createdSkillButtons.Add(newSkillButton);
            }
        }

        public void OnClickJumpButton()
        {
            linkedCharactor.Jump();
        }

        public void OnClickAttackButton()
        {
            linkedCharactor.Attack();
        }

        public void OnClickSkillButton(ControllerUI_SkillButton button)
        {
            bool skillAttack = linkedCharactor.SkillAttack(button.skillBase.SkillData.id);

        }

    }
}
