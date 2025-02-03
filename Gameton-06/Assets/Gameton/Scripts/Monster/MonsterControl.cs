using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace TON
{
    public class MonsterControl : MonoBehaviour
    {
        public int id;  // 적 고유 ID
        public string name; // 적 이름
        public int hp;  // HP
        public int damage;  // 공격력
        public int defense; // 방어력
        public int speed = 2; // 이동속도
        public string aiType; // AI 패턴?
        public int dropItems;   // 드롭 아이템 ID
        public int eXperiencePoint; // 경험치?
        
        
        
        public Animator animator;

        GameObject target;
        
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
    }
}
