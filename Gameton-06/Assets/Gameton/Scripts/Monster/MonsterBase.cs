using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace TON
{
    public class MonsterBase : MonoBehaviour, IDamage
    {
        public int id;  // 적 고유 ID
        public int hp;  // HP
        public int damage;  // 공격력
        
        const int speed = 2; // 이동속도
        
        
        
        public Animator animator;

        GameObject target;
        private IDamage _damageImplementation;

        // Start is called before the first frame update
        void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player");
            animator = GetComponent<Animator>();
            
            
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
            // animator.SetBool("Iidle", true);
            animator.SetBool("Walk", true );    // 걷기 애니메이션
        }

        public void ApplyDamage(float damage)
        {
            _damageImplementation.ApplyDamage(damage);
        }
    }
}
