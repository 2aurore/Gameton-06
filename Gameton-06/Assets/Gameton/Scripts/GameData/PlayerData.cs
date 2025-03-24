using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    [System.Serializable]
    public class PlayerData
    {
        // 필드 선언 (외부 수정 방지)
        public string type;
        public string name;
        public int level = 1;
        public int experience = 0;
        public int hp = 100;
        public int mp = 100;
        public float attackPower = 50f;
        public float defensivePower = 30f;
        public int critical = 30;

        public PlayerData() : this("w", "") { }

        public PlayerData(string t, string n)
        {
            type = t == "BlackCat" ? "b" : "w";
            name = n;
        }
    }

}
