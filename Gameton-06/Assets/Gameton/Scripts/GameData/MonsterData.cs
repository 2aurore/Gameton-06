using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    [System.Serializable]
    public class MonsterData
    {
        // 몬스터 아이디
        public int id;
        // 몬스터 명 or 프리팹 명?
        public string name;
        // 몬스터 레벨
        public int level;
        // 몬스터 타입 ex : melee, ranged
        public string monsterType;
        // 몬스터 체력
        public int hp;
        // 기본 공격력
        public int attackPower;
        // 기본 방어력
        public int defensivePower;
        // 몬스터 스킬 ID
        public string monsterSkillID;
        // 패트롤 범위
        public float patrolRange;
        // 인식 범위
        public float detectionRange;
        // 추적 범위
        public float chaseRange;
        // 공격 범위
        public float attackRange;
        
        public MonsterData(int id, string name, int level, string monsterType, int hp, int attackPower, int defensivePower, int monsterSkillID, float patrolRange, float detectionRange, float chaseRange, float attackRange)
        {
            id = id;
            name = name;
            level = level;
            monsterType = monsterType;
            hp = hp;
            attackPower = attackPower;
            defensivePower = defensivePower;
            monsterSkillID = monsterSkillID;
            patrolRange = patrolRange;
            detectionRange = detectionRange;
            chaseRange = chaseRange;
            attackRange = attackRange;
        }
    }
}
