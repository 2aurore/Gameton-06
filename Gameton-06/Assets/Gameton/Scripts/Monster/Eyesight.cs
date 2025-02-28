using UnityEngine;

namespace TON
{
    public class Eyesight : MonoBehaviour
    {
        [SerializeField]
        private MonsterBase _monsterBase;

        public void SetMonsterBase(MonsterBase monsterBase)
        {
            _monsterBase = monsterBase;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _monsterBase.IsDetect = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _monsterBase.IsDetect = false;
            }
        }
    }
}
