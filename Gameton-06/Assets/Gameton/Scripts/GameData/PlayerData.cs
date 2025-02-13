using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    [System.Serializable]
    public class PlayerData
    {
        // 캐릭터 아이디
        public string id;
        // 캐릭터 이미지 타입 (w/b)
        public string type;
        // 캐릭터 이름
        public string name;
        // 캐릭터 레벨
        public int level;
        // 캐릭터 경험치
        public int experience;
        // 캐릭터 체력
        public int hp;
        // 캐릭터 마나(스킬 포인트)
        public int mp;
        // 기본 공격력
        public int attackPower;
        // 기본 방어력
        public int defensivePower;
        // 캐릭터 크리티컬 수치 
        public int critical;

        public PlayerData(int i, string t, string n)
        {
            id = $"P{i:000}";
            type = t == "BlackCat" ? "b" : "w";
            name = n;
            level = 1;
            experience = 0;
            hp = 100;
            mp = 100;
            attackPower = 50;
            defensivePower = 30;
            critical = 30;
        }
    }

}
