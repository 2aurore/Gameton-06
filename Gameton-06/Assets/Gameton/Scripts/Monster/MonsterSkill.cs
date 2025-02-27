using UnityEngine;

namespace TON
{
    public class MonsterSkill : MonoBehaviour
    {
        public float speed = 5f;
        public float damage = 0f;
        
        Vector2 direction;
        Transform playerTransform; // 플레이어의 Transform을 저장할 변수
        [SerializeField] private SpriteRenderer _spriteRenderer; // 스킬의 스프라이트 렌더러
        private MonsterBase _monsterBase;
        private CharacterBase _characterBase;
        public Vector2 Direction
        {
            set { direction = value.normalized; }
        }
        
        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            _characterBase = GameObject.Find("TON.Player").GetComponentInChildren<CharacterBase>();
                
            if (_characterBase != null)
            {
                playerTransform = _characterBase.transform;
            }
            else
            {
                Debug.LogError("Player 오브젝트를 찾을 수 없습니다. 태그를 확인하세요.");
                return; // 플레이어 없으면 이후 로직 실행 중지
            }

            SetDirection(); // 발사 방향 설정
        }

        void Update()
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                // 플레이어에게 직접 데미지 적용
                _characterBase?.ApplyDamage(damage);
                Destroy(gameObject);
            }
            else if (collision.CompareTag("Ground"))
            {
                // 지형에 부딪히면 스킬 오브젝트만 파괴
                Destroy(gameObject);
            }
        }

        public void SetSkillDamage(float skillDamage)
        {
            damage += skillDamage;
        }
        
        void SetDirection()
        {
            if (playerTransform == null) return; // 플레이어가 없으면 방향 설정 불가
            
            Vector2 toPlayer = playerTransform.position - transform.position;   // MonsterSkill과 Player의 위치 차이 계산

            // 플레이어의 왼쪽/오른쪽 판별
            if (toPlayer.x < 0)
            {
                direction = new Vector2(-1, 0); // 플레이어가 왼쪽에 있으면 왼쪽 방향으로 발사
            }
            else
            {
                direction = new Vector2(1, 0);  // 플레이어가 오른쪽에 있으면 오른쪽 방향으로 발사
                _spriteRenderer.flipX = true; // 왼쪽 방향일 때 좌우 반전
            }
        }
    }
    
    
}
