using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class damageTest : MonoBehaviour, IDamage
    {
        public float hp = 1000;

        public void ApplyDamage(float damage)
        {
            hp -= damage;
        }
    }
}
