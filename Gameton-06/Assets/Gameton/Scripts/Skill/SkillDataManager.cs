using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class SkillDataManager : SingletonBase<SkillDataManager>
    {
        public List<SkillData> skillDatas { get; private set; }
        public SerializableDictionary<string, SkillBase> skillInstances { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            LoadSkillData();
        }

        private void LoadSkillData()
        {
            skillDatas = JSONLoader.LoadFromResources<List<SkillData>>("Skill");
            if (skillDatas == null)
            {
                skillDatas = new List<SkillData>();
            }
        }

        public void SetSkillInstances()
        {
            skillInstances = new SerializableDictionary<string, SkillBase>();
            // skillData를 skillBase로 치환
            foreach (var skillData in skillDatas)
            {
                skillInstances.Add(skillData.id, InitSkillData(skillData));
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
        public List<SkillBase> GetActiveSkillInstance()
        {
            List<SkillBase> filteredSkills = new List<SkillBase>();

            foreach (SkillData skill in skillDatas)
            {
                if (skill.slotNumber == 1 || skill.slotNumber == 2 || skill.slotNumber == 3)
                {
                    filteredSkills.Add(skillInstances.GetValueOrDefault(skill.id));
                }
            }
            return filteredSkills;
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

        // 스킬의 쿨타임 및 파괴 로직 실행 메소드
        public void InvokeExcuteSkill(string skillId)
        {
            if (skillInstances.TryGetValue(skillId, out SkillBase skill))
            {
                skill.InvokeExcuteSkill();
            }
        }

        // 스킬 발사(생성) 메소드 추가
        public void ExecuteSkill(string skillId, Transform firePoint, float lastDirection)
        {
            // 스킬 생성
            GameObject skill = ObjectPoolManager.Instance.GetEffect(skillId);

            skill.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);

            // 🔥 스킬 방향 반전
            var bulletScale = skill.transform.localScale;
            bulletScale.x = Mathf.Abs(bulletScale.x) * lastDirection;
            skill.transform.localScale = bulletScale;

            // 스킬 이동 방향 설정
            Rigidbody2D skillRb = skill.GetComponent<Rigidbody2D>();
            skillRb.velocity = new Vector2(lastDirection * 5f, 0f);


            InvokeExcuteSkill(skillId);
        }

        private SkillBase InitSkillData(SkillData skillData)
        {
            SkillBase skill = gameObject.AddComponent<SkillBase>();
            skill.Init(skillData);
            return skill;
        }

        public SkillBase GetSkillData(string skillId)
        {
            return skillInstances.GetValueOrDefault(skillId);
        }
    }
}
