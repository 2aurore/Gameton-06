using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TON
{
    public class MonsterSkill : MonoBehaviour
    {
        public float speed = 3;
        public float damage = 1;

        Vector2 direction;
        Transform playerTransform; // 플레이어의 Transform을 저장할 변수
        [SerializeField] private SpriteRenderer _spriteRenderer; // 스킬의 스프라이트 렌더러
        
        public Vector2 Direction
        {
            set { direction = value.normalized; }
            
            
        }
        
        // Start is called before the first frame update
        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            // 플레이어 태그로 플레이어 오브젝트를 찾아서 Transform 컴포넌트를 가져옴
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogError("Player 오브젝트를 찾을 수 없습니다. 태그를 확인하세요.");
                return; // 플레이어 없으면 이후 로직 실행 중지
            }

            SetDirection(); // 발사 방향 설정
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") || collision.CompareTag("Ground"))
            {
                Destroy(gameObject);
            }
        }
        
        void SetDirection()
        {
            if (playerTransform == null) return; // 플레이어가 없으면 방향 설정 불가

            // MonsterSkill과 Player의 위치 차이 계산
            Vector2 toPlayer = playerTransform.position - transform.position;

            // 플레이어의 왼쪽/오른쪽 판별
            if (toPlayer.x < 0)
            {
                // 플레이어가 왼쪽에 있으면 왼쪽 방향으로 발사
                direction = new Vector2(-1, 0);
            }
            else
            {
                // 플레이어가 오른쪽에 있으면 오른쪽 방향으로 발사
                direction = new Vector2(1, 0);
                _spriteRenderer.flipX = true; // 왼쪽 방향일 때 좌우 반전
            }
        }
    }
    
    
}
