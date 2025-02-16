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
                _monsterBase.IsDetect = true;
                
                // _monsterBase.SetTransition(new MonsterSkillState());
                //
                // Debug.Log("감지됨");
                
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _monsterBase.IsDetect = false;
            //
            // _monsterBase.SetTransition(new IdleState());
            //
            // Debug.Log("감지 벗어남");
        }
    }
}
