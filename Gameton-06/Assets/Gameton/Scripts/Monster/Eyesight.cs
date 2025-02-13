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
                _monsterBase.IsDetect = true;
                Debug.Log("감지됨");
                
                if (_monsterBase.IsDetect)
                {
                    _monsterBase.Detect(other.gameObject);
                    
                    // _monsterBase.IsWalking = false;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _monsterBase.SetTransition(new IdleState());
            
            // _monsterBase.IsDetect = false;
            // _monsterBase.IsWalking = true;
            // _monsterBase.ResetTime();
            // _monsterBase.ChangeAnimationState(MonsterBase.AniIdle);
            Debug.Log("감지 벗어남");
        }
    }
}
