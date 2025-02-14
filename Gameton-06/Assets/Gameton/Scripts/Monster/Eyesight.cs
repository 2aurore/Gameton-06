using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class Eyesight : MonoBehaviour
    {
        [SerializeField]
        private MonsterBase _monsterBase;
        // private bool _isDetect;
        

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // 플레이어 감지하면 따라가기
                _monsterBase.SetTransition(new ChasingState());
                Debug.Log("감지됨");
                
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _monsterBase.SetTransition(new IdleState());
            
            Debug.Log("감지 벗어남");
        }
    }
}
