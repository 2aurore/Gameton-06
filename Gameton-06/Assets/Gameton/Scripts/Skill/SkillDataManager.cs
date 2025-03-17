using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace TON
{
    public class SkillDataManager : SingletonBase<SkillDataManager>
    {

        public List<SkillData> skillDatas { get; private set; }
        public SerializableDictionary<string, SkillBase> skillInstances { get; private set; }

        private List<SkillBase> equippedSkills = new List<SkillBase>();
        private BackendSkillDataManager skillDataManager;

        // 각 단계별 이벤트 정의
        private event System.Action OnDataLoadComplete;
        private event System.Action OnSetupComplete;


        public void Initalize(System.Action onComplete = null)

        {
            skillDataManager = new BackendSkillDataManager();

            // 각 단계별 이벤트 등록
            OnDataLoadComplete = () =>
            {
                SetSkillInstances();
                OnSetupComplete?.Invoke();
            };

            OnSetupComplete = () =>
            {
                GetActiveSkillInstance();
                // 장착된 스킬이 변경된 경우, UI 업데이트 실행할 수 있도록 onComplete 이벤트 등록
                onComplete?.Invoke();
            };

            // 첫 단계 시작
            LoadSkillData();
        }

        private void Update()
        {
            SceneType activeScene = Main.Singleton.currentSceneType;
            // 씬이 인게임일 때만 스킬 쿨타임 업데이트 되도록 적용
            if (activeScene != SceneType.Stage)
                return;

            foreach (var skill in equippedSkills)
            {
                UpdateSkillCoolDown(skill.SkillData.id);
            }
        }

        private void LoadSkillData()
        {
            if (skillDatas != null)
            {
                skillDatas.Clear();
            }

            // gamedata 폴더에 있는 skill.json 파일을 불러옴
            skillDatas = JSONLoader.LoadFromResources<List<SkillData>>("skill");
            GetSkillDataList();

            if (skillDatas == null)
            {
                skillDatas = new List<SkillData>();
            }
        }

        private void GetSkillDataList()
        {

            // 서버에 저장된 사용자의 스킬 슬롯 데이터를 가져옴
            skillDataManager.LoadMySkillData(userSkillData =>
            {
                // 스킬 슬롯 데이터가 없는 경우
                if (userSkillData == null)
                {
                    Debug.LogError("스킬 슬롯 데이터가 없습니다");
                    return;
                }

                // 스킬 슬롯 데이터가 있는 경우
                UpdateSkillData(userSkillData.slot_1, 1);
                UpdateSkillData(userSkillData.slot_2, 2);
                UpdateSkillData(userSkillData.slot_3, 3);

                // 다음 단계로 진행
                OnDataLoadComplete?.Invoke();
            });

        }

        public void UpdateSkillData(string skillId, int slotNumber, System.Action onComplete = null)
        {
            // 현재 슬롯에 스킬이 있는지 확인
            foreach (var skill in skillDatas)
            {
                if (skill.id == skillId)
                {
                    skill.slotNumber = slotNumber;
                }
                if (skill.slotNumber == slotNumber && skill.id != skillId)
                {
                    skill.slotNumber = 0;
                }
            }

            // 스킬 슬롯 데이터를 서버에 저장
            if (onComplete != null)
            {
                UpdateSkillSlotDataToServer(onComplete);
            }

        }

        private void UpdateSkillSlotDataToServer(System.Action onComplete)
        {
            UserSkillData userSkillData = new()
            {
                slot_1 = skillDatas.Find(skill => skill.slotNumber == 1)?.id,
                slot_2 = skillDatas.Find(skill => skill.slotNumber == 2)?.id,
                slot_3 = skillDatas.Find(skill => skill.slotNumber == 3)?.id
            };

            Debug.Log($"UpdateSkillSlotDataToServer() : {userSkillData.slot_1}, {userSkillData.slot_2}, {userSkillData.slot_3}");

            skillDataManager.UpdateSkillData(userSkillData, () =>
            {
                Initalize(onComplete);
            });
        }

        public void SetSkillInstances()
        {
            skillInstances = new SerializableDictionary<string, SkillBase>();
            // skillData를 skillBase로 치환
            foreach (var skillData in skillDatas)
            {
                skillInstances.Add(skillData.id, new SkillBase(skillData));
            }
        }

        // 스킬 슬롯에 배치할 수 있는 스킬 수 리턴하는 메소드
        public int GetActiveSkillCount()
        {
            int characterLevel = PlayerDataManager.Singleton.player.level;
            int availableSkillCount = 0;

            foreach (SkillData skill in skillDatas)
            {
                if (skill.requiredLevel <= characterLevel)
                {
                    availableSkillCount++;
                }
            }
            return availableSkillCount > 3 ? 3 : availableSkillCount;
        }

        // 스킬 슬롯에 적용해야하는 스킬 리스트 리턴
        public List<SkillBase> GetEquippedSkills()
        {
            return equippedSkills;
        }

        // 스킬 슬롯에 적용될 스킬 리스트 초기화 및 업데이트에 사용
        public List<SkillBase> GetActiveSkillInstance()
        {
            if (equippedSkills != null)
            {
                equippedSkills.Clear();
            }

            foreach (SkillData skill in skillDatas)
            {
                if (skill.slotNumber == 1 || skill.slotNumber == 2 || skill.slotNumber == 3)
                {
                    // Debug.Log("GetActiveSkillInstance() : " + skill.id);
                    equippedSkills.Add(skillInstances.GetValueOrDefault(skill.id));
                }
            }
            equippedSkills.Sort((a, b) => a.SkillData.slotNumber.CompareTo(b.SkillData.slotNumber));
            return equippedSkills;
        }

        // 스킬 쿨타임 설정하는 메소드 
        public void SetCoolTime(string skillId)
        {
            if (skillInstances.TryGetValue(skillId, out SkillBase skillBase))
            {
                skillBase.SetCurrentCoolDown();
            }
        }

        // 스킬 쿨타임 업데이트 메소드 
        public void UpdateSkillCoolDown(string skillId)
        {
            if (skillInstances.TryGetValue(skillId, out SkillBase skillBase))
            {
                skillBase.UpdateSkill(Time.deltaTime);
            }
        }

        // 스킬을 실행할 수 있는지 확인
        public bool CanExecuteSkill(string skillId)
        {
            if (skillInstances.TryGetValue(skillId, out SkillBase skill))
            {
                return skill.CurrentCoolDown <= 0;
            }
            else
            {
                return false;
            }
        }

        // 스킬 발사(생성) 메소드 추가
        public void ExecuteSkill(string skillId, Transform firePoint, float lastDirection)
        {
            // 스킬 생성
            GameObject effectGameObject = ObjectPoolManager.Instance.GetEffect(skillId);
            Projectile projectile = effectGameObject.GetComponent<Projectile>();
            SkillBase targetSkillBase = GetSkillInstance(skillId);

            // 현재 스킬의 쿨타임 시작 
            targetSkillBase.SetCurrentCoolDown();

            // 스킬 투사체 초기화
            projectile.Init(targetSkillBase.SkillData.damage, targetSkillBase.SkillData.maxHitCount);

            effectGameObject.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);

            // 스킬 방향 반전
            var bulletScale = effectGameObject.transform.localScale;
            bulletScale.x = Mathf.Abs(bulletScale.x) * lastDirection;
            effectGameObject.transform.localScale = bulletScale;

            // 스킬 이동 방향 설정
            Rigidbody2D skillRb = effectGameObject.GetComponent<Rigidbody2D>();
            skillRb.velocity = new Vector2(lastDirection * 5f, 0f);

            targetSkillBase.OnSkillExecuted?.Invoke();
        }

        public SkillBase GetSkillInstance(string skillId)
        {
            // 스킬 베이스가 null일때 방어로직 추가
            SkillBase result = skillInstances.GetValueOrDefault(skillId);
            Assert.IsNotNull(result, "SkillDataManager.ExecuteSkill() : targetSkillBase is null");
            return result;
        }


    }
}
