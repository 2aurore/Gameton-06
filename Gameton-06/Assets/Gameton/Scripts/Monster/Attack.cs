using UnityEngine;

namespace TON
{
    public class Attack : MonoBehaviour
    {
        private MonsterBase _monsterBase;
        private GameObject target; // target 변수 선언
        
        void Start()
        {
            // target 변수 초기화 (예시)
            target = GameObject.Find("TON.Player"); // Player라는 이름을 가진 게임 오브젝트 찾기
            if (target == null)
            {
                // Debug.LogError("Player not found!");
            }
        }
        
        public void SetMonsterBase(MonsterBase monsterBase)
        {
            _monsterBase = monsterBase;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(target != null) // target이 null이 아닌지 확인
            {
                if (other.CompareTag("Player"))
                {
                    _monsterBase.IsAttacking = true;
                    _monsterBase.IsFinishAttack = false; // 공격 시작 시 FinishAttack 초기화
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (target != null) // target이 null이 아닌지 확인
            {
                if (other.CompareTag("Player"))
                {
                    _monsterBase.IsAttacking = false;
                    _monsterBase.IsFinishAttack = true;
                }
            }
        }
    }
}
