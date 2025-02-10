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
                // TODO : 플레이어 감지하면 따라가기
                
                _monsterBase.IsDetect = true;
                Debug.Log("감지됨");
                
                if (_monsterBase.IsDetect)
                {
                    _monsterBase.Detect(other.gameObject);
                    
                    // _monsterBase.IsWalking = false;
                }
            }
        }
        
        // 단순 플레이어 태그 기준 따라가는 코드
        private void FixedUpdate()
        {   
            // 타겟의 위치에서 내 현제 위치를 뺌
            // UnityEngine.Vector2 direction = target.transform.position - transform.position;
            
            // 방향 * 속도 * 시간간격 
            // transform.Translate(direction.normalized * speed * Time.deltaTime);
            // animator.SetBool("Iidle", true);
            
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _monsterBase.IsDetect = false;
            _monsterBase.IsWalking = true;
        }
    }
}
