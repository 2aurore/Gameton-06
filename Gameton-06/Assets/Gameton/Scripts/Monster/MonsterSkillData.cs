using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    [System.Serializable]
    
    public class MonsterSkillData
    {
        public int id;  // 몬스터 id
        public string skillType;    // 스킬 타입(공격, 근거리, 원거리)
        public int damage;  // 스킬 피해량
        public float coolTime; // 스킬 쿨타임
        public float skillRange;    // 스킬 범위(콜라이더 조정)
    }
}
