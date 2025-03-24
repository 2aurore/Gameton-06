using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    [System.Serializable]
    public class UserItemData
    {
        public int hpPotion;
        public int mpPotion;

        public UserItemData()
        {
            hpPotion = 5;
            mpPotion = 5;
        }
    }
}
