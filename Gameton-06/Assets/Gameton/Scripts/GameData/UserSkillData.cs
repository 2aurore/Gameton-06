using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    [System.Serializable]
    public class UserSkillData
    {

        public string slot_1;
        public string slot_2;
        public string slot_3;

        public UserSkillData()
        {
            slot_1 = "K0001";
            slot_2 = string.Empty;
            slot_3 = string.Empty;
        }
    }
}
