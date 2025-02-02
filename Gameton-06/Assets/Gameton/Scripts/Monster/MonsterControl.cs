using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace TON
{
    public class MonsterControl : MonoBehaviour
    {
        public float speed = 2;

        GameObject target;
        
        // Start is called before the first frame update
        void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        
        // Update is called once per frame
        void Update()
        {
        
        }

        private void FixedUpdate()
        {   
            // 타겟의 위치에서 내 현제 위치를 뺌
            UnityEngine.Vector2 direction = target.transform.position - transform.position;
            
            // 방향 * 속도 * 시간간격 
            transform.Translate(direction.normalized * speed * Time.deltaTime);
        }
    }
}
