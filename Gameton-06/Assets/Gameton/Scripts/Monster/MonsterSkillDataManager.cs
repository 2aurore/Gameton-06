using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;
    
    public class MonsterSkillDataManager : SingletonBase<MonsterSkillDataManager>
    {
        // 전체 몬스터 스킬 데이터 리스트
        public List<MonsterSkillData> monstersSkillData { get; private set; }

        // 현재 선택된 몬스터 스킬 데이터
        public MonsterSkillData currentMonsterSkill { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            LoadMonsterSkillData();
        }

        private void LoadMonsterSkillData()
        {
            monstersSkillData = JSONLoader.LoadFromResources<List<MonsterSkillData>>("MonsterSkills");
            if (monstersSkillData == null)
            {
                monstersSkillData = new List<MonsterSkillData>();
                Debug.LogError("몬스터 스킬 데이터 로드 실패");
            }
        }

        public MonsterSkillData GetMonsterSkillData(int monsterSkillId)
        {
            if (monsterSkillId >= 0 && monsterSkillId < monstersSkillData.Count)
            {
                currentMonsterSkill = monstersSkillData[monsterSkillId];
                return currentMonsterSkill;
            }
            else
            {
                Debug.LogError($"유효하지 않은 몬스터 SkillID입니다: {monsterSkillId}");
                return null;
            }
        }

        public List<MonsterSkillData> GetAllMonsterSkillData()
        {
            return monstersSkillData;
        }
    }
}
