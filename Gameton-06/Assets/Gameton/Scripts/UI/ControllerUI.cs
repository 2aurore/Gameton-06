using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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

        public UserItemData userItem { get; private set; } = new UserItemData();
        // 보유 포션 수량 
        [SerializeField] private TextMeshProUGUI hpPotionCount;
        [SerializeField] private TextMeshProUGUI mpPotionCount;

        public List<ControllerUI_ItemButton> itemButtons = new List<ControllerUI_ItemButton>();

        private void OnEnable()
        {
            skillButtonPrefab.gameObject.SetActive(false);
            Initalize();
        }

        private void FixedUpdate()
        {
            InitalizeItemData();
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
            List<SkillBase> activatedSkills = SkillDataManager.Singleton.GetEquippedSkills();
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
                    newSkillButton.GetComponent<Button>().interactable = false;
                }

                createdSkillButtons.Add(newSkillButton);
            }
        }

        private void InitalizeItemData()
        {
            userItem = PlayerDataManager.Singleton.userItem;

            hpPotionCount.text = $"{userItem.hpPotion}";
            mpPotionCount.text = $"{userItem.mpPotion}";
        }

        public void OnClickHpPotionButton()
        {
            if (userItem.hpPotion <= 0)
                return;

            PlayerDataManager.Singleton.UsePotion("HP");

            itemButtons[0].SetCurrentCoolDown();
            linkedCharactor.UsePotion("HP");
        }

        public void OnClickMpPotionButton()
        {
            if (userItem.mpPotion <= 0)
                return;

            PlayerDataManager.Singleton.UsePotion("MP");

            itemButtons[1].SetCurrentCoolDown();
            linkedCharactor.UsePotion("MP");
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
            linkedCharactor.SkillAttack(button.skillBase.SkillData.id);
        }

    }
}
