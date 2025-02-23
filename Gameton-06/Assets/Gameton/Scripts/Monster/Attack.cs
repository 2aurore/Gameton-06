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
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _monsterBase.IsAttacking = false;
                _monsterBase.IsFinishAttack = true; // 공격 종료 상태로 설정
            }
        }
    }
}
