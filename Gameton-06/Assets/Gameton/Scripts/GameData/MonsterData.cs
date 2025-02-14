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
        public int defencePower;
        // 몬스터 스킬 ID
        public int monsterSkillID;
        // 패트롤 범위
        public float patrolRange;
        // 인식 범위
        public float detectionRange;
        // 추적 범위
        public float chaseRange;
        public float moveSpeed;
        // 공격 범위
        public float attackRange;

    }
}
