using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    [System.Serializable]
    public class CashData
    {
        public int gold;
        public int fish;

        public CashData()
        {
            gold = 0;
            fish = 0;
        }
    }
}
