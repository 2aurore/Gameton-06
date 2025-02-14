using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class Attack : MonoBehaviour
    {
        [SerializeField]
        private MonsterBase _monsterBase;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _monsterBase.SetTransition(new AttackState());
                // _monsterBase.SetTransition(new MonsterSkillState());
            }

            // if (!_monsterBase.monsterSkillID)
            // {
            //     if (other.CompareTag("Player"))
            //     {
            //         // 일정 확률로 스킬 사용
            //         if (Random.value < 0.3f) // 30% 확률
            //         {
            //             _monsterBase.SetTransition(new SkillState());
            //         }
            //         else
            //         {
            //             _monsterBase.SetTransition(new AttackState());
            //         }
            //     }
            // }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            _monsterBase.SetTransition(new ChasingState());
            
            Debug.Log("감지 벗어남");
        }
    }
}
