using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace TON
{
    public class SkillDataManager : SingletonBase<SkillDataManager>
    {
        public List<SkillData> skillDatas { get; private set; }
        public SerializableDictionary<string, SkillBase> skillInstances { get; private set; }

        private List<SkillBase> equippedSkills = new List<SkillBase>();

        public void Initalize()
        {
            LoadSkillData();
            SetSkillInstances();
            GetActiveSkillInstance();
        }

        private void Update()
        {
            // 씬이 인게임일때만 돌게 조건 추가 (stage 이름을 가지고 잇을대?)
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

            JSONLoader.SaveJsonToPersistentData("skill");
            skillDatas = JSONLoader.LoadJsonFromPersistentData<List<SkillData>>("skill");

            if (skillDatas == null)
            {
                skillDatas = new List<SkillData>();
            }
        }

        public void UpdateSkillData(string skillId, int slotNumber)
        {
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

            Assert.IsTrue(JSONLoader.SaveUpdatedJsonToPersistentData(skillDatas, "skill"));
            Initalize();
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
