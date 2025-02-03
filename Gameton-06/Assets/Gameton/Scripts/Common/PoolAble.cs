using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace TON
{
    public class PoolAble : MonoBehaviour
    {
        public IObjectPool<GameObject> Pool { get; set; }

        public void ReleaseObject()
        {
            Pool.Release(gameObject);
        }
    }
}
