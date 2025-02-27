using UnityEngine;

namespace TON
{
    public class Attack : MonoBehaviour
    {
        private MonsterBase _monsterBase;
        
        public void SetMonsterBase(MonsterBase monsterBase)
        {
            _monsterBase = monsterBase;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _monsterBase.IsAttacking = true;
                _monsterBase.IsFinishAttack = false; // 공격 시작 시 FinishAttack 초기화
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _monsterBase.IsAttacking = false;
                _monsterBase.IsFinishAttack = true;
            }
        }
    }
}
