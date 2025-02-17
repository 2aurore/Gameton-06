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
                _monsterBase.IsAttacking = true;
                // _monsterBase.SetTransition(new AttackState());
                // _monsterBase.SetTransition(new MonsterSkillState());
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            _monsterBase.IsAttacking = false;
            // _monsterBase.SetTransition(new ChasingState());
            
            // Debug.Log("감지 벗어남");
        }
    }
}
