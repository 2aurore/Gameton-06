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

        public Button[] buttons; // UI 버튼 (3개)

        [SerializeField]
        private SerializableDictionary<int, ControllerUI_SkillButton> skillButtons;
        private SerializableDictionary<string, SkillBase> skillInstances;
        private List<SkillData> skillDatas;
        private List<SkillBase> skillBases;


        public void Initalize()
        {
            int characterLevel = PlayerDataManager.Singleton.player.level;
            skillInstances = SkillDataManager.Singleton.skillInstances;

            // 내가 사용할 스킬은 스킬 매니저에서 가져오게 변경

            if (skillInstances != null)
            {
                int skillSlotCount = SkillDataManager.Singleton.GetActiveSkillCount();
                List<SkillBase> skillList = SkillDataManager.Singleton.GetActiveSkillInstance();

                // 버튼 설정
                for (int i = 0; i < buttons.Length; i++)
                {
                    if (i < skillSlotCount)
                    {
                        buttons[i].interactable = true; // 사용 가능
                        SkillBase skillData = skillList.Find(skill => skill.SkillData.slotNumber == i + 1);
                        skillButtons[i].Initalize(skillData);
                    }
                    else
                    {
                        buttons[i].interactable = false; // 사용 불가
                    }
                }
            }
            else
            {
                Debug.LogError("스킬 정보 로드 오류 발생");
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
            // skill Attack 이 true 일때 만 쿨타임 흘러가게끔
            if (skillAttack)
            {
                // SkillData skillData = skillDatas.Find(skill => skill.id == button.skillId);
                button.SetCoolTime();

            }
        }

    }
}
