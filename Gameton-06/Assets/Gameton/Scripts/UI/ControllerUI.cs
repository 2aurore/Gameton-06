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
        private List<SkillData> skillDatas;


        public void Initalize()
        {
            int characterLevel = PlayerDataManager.Singleton.player.level;
            skillDatas = SkillDataManager.Singleton.skillDatas;

            if (skillDatas != null)
            {
                // 사용 가능한 스킬 필터링 (캐릭터 레벨보다 필요 레벨이 낮거나 같은 것만)
                List<SkillData> availableSkills = skillDatas
                    .Where(skill => skill.requiredLevel <= characterLevel)
                    .OrderBy(skill => skill.requiredLevel) // 필요 레벨이 낮은 순으로 정렬
                    .Take(3) // 최대 3개 선택
                    .ToList();

                // 버튼 설정
                for (int i = 0; i < buttons.Length; i++)
                {
                    if (i < availableSkills.Count)
                    {
                        buttons[i].interactable = true; // 사용 가능
                        SkillData skillData = skillDatas.Find(skill => skill.slotNumber == i + 1);
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

        public void OnClickSkillButton(ControllerUI_SkillButton skill)
        {

            linkedCharactor.SkillAttack(skill.skillData.name);
        }

        private void Update()
        {
            foreach (var index in skillButtons.Keys)
            {
                ControllerUI_SkillButton skillButton = skillButtons.GetValueOrDefault(index);
                if (skillButton != null && skillButton.skillData != null)
                {
                    // skillButton.SetCoolTime()
                }
            }
        }
    }
}
