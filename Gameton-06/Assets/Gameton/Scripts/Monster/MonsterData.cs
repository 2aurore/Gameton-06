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
        // 몬스터 타입 ex : melee, ranged
        public string monsterType;
        // 몬스터 체력
        public int hp;
        
        // 기본 공격력
        public int attackPower;
        // 기본 방어력
        public int defensivePower;

        public MonsterData(int id, string monsterType, string name, int hp, int attackPower, int defensivePower)
        {
            id = this.id;
            monsterType = this.monsterType == "monster" ? "melle" : "ranged";
            name = this.name;
            hp = this.hp;
            attackPower = this.attackPower;
            defensivePower = this.defensivePower;
        }
    }
}
