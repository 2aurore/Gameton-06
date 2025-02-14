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
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            _monsterBase.SetTransition(new ChasingState());
            
            Debug.Log("감지 벗어남");
        }
    }
}
