using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class SkillDataManager : SingletonBase<SkillDataManager>
    {
        public List<SkillData> skillDatas { get; private set; }
        public List<SkillBase> skillBases { get; private set; }

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
            // skillData를 skillBase로 치환
        }

        public SkillData GetSkillData(string skillId)
        {
            return skillDatas.Find(v => v.id == skillId);
        }
    }
}
