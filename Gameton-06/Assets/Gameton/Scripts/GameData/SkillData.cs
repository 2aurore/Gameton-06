using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    [System.Serializable]
    public class SkillData
    {
        public string id;
        public string name;
        public int mpConsumption;
        public float damage;
        public int coolDown;
        public int slotNumber;
        public int requiredLevel;
    }
}
