using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    [System.Serializable]
    public class MonsterSkillData
    {
        public int skillId;        // 스킬 ID
        public string skillName;      // 스킬 이름
        public float damage;          // 스킬 데미지
        public float cooldown;        // 스킬 쿨다운
        public float range;           // 스킬 범위
        public string prefabName;  // 스킬 프리팹 이름
    }
}