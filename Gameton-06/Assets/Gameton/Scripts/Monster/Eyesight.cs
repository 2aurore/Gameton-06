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
        private GameObject skillPrefab;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // 30% 확률로 스킬 사용, 70% 확률로 추격 후 근접 공격
                if (Random.Range(0, 10) < 3) // 0~9 사이의 난수 생성, 3 미만이면 30% 확률
                {
                    _monsterBase.SetTransition(new MonsterSkillState());
                }
                else
                {
                    // 플레이어 감지하면 따라가기
                    _monsterBase.SetTransition(new ChasingState());
                }

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
