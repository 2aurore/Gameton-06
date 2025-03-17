using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
        public SkillButtonItem skillButtonPrefab;
        private List<SkillButtonItem> createdSkillButtons = new List<SkillButtonItem>();

        public UserItemData userItem { get; private set; } = new UserItemData();
        // 보유 포션 수량
        [SerializeField] private TextMeshProUGUI hpPotionCount;
        [SerializeField] private TextMeshProUGUI mpPotionCount;

        private int potionLimit = 5; // 게임에서 사용할 수 있는 포션 수 제한
        private int hpPotion = 0;
        private int mpPotion = 0;

        public List<PotionButtonItem> itemButtons = new List<PotionButtonItem>();

        private void OnEnable()
        {
            skillButtonPrefab.gameObject.SetActive(false);
            Initalize();
            InitalizeItemData();
        }

        private void FixedUpdate()
        {
            SetPotionText();
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
                SkillButtonItem newSkillButton = Instantiate(skillButtonPrefab, skillButtonGroup);
                newSkillButton.gameObject.SetActive(true);

                if (i < activatedSkills.Count && activatedSkills[i] != null) // 해당 인덱스에 활성화된 스킬이 있을 경우
                {
                    // 직접 equippedSkills의 인스턴스를 전달
                    newSkillButton.Initalize(activatedSkills[i]);
                    newSkillButton.GetComponent<Button>().interactable = true;
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

            // 게임 진입 시 포션 초기 수량 세팅
            hpPotion = userItem.hpPotion >= potionLimit ? potionLimit : userItem.hpPotion;
            mpPotion = userItem.mpPotion >= potionLimit ? potionLimit : userItem.mpPotion;

            SetPotionText();
        }

        // 현재 인게임 내에서 사용중인 포션의 숫자 업데이트
        private void SetPotionText()
        {
            hpPotionCount.text = $"{hpPotion}";
            mpPotionCount.text = $"{mpPotion}";
        }


        public void OnClickHpPotionButton()
        {
            if (hpPotion <= 0)
                return;

            // 현재 캐릭터가 포션을 사용할 수 있는 상태일때만 포션 수 감소
            linkedCharactor.UsePotion("HP", isSuccess =>
            {
                if (isSuccess)
                {
                    hpPotion--;
                    PlayerDataManager.Singleton.UsePotion("HP");
                    itemButtons[0].SetCurrentCoolDown();
                }
            });
        }

        public void OnClickMpPotionButton()
        {
            if (mpPotion <= 0)
                return;

            // 현재 캐릭터가 포션을 사용할 수 있는 상태일때만 포션 수 감소
            linkedCharactor.UsePotion("MP", isSuccess =>
            {
                if (isSuccess)
                {
                    mpPotion--;
                    PlayerDataManager.Singleton.UsePotion("MP");
                    itemButtons[1].SetCurrentCoolDown();
                }
            });

        }

        public void OnClickJumpButton()
        {
            linkedCharactor.Jump();
        }

        public void OnClickAttackButton()
        {
            linkedCharactor.Attack();
        }

        public void OnClickSkillButton(SkillButtonItem button)
        {
            if (button.skillBase != null)
            {
                linkedCharactor.SkillAttack(button.skillBase);
            }
        }

    }
}
