using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using DamageCalculator = TON.DamageCalculator;
using Vector3 = UnityEngine.Vector3;

namespace TON
{
    public class MonsterBase : MonoBehaviour, IDamage
    {
        public int id;  // 적 고유 ID
        public float currentHP = 100;  // HP

        public string name; // 몬스터 명 or 프리팹 명
        public string monsterType;  // 몬스터 타입 ex : melee, ranged
        
        
        // public int damage;  // 공격력
        public float speed = 2; // 이동속도
        
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;

        private Vector3 _direction;
        private bool _isWalking;
        private bool _isHit;
        private bool _isDetect;
        private float _currentTime;

        [SerializeField]
        private int _idleTime = 2;

        [SerializeField]
        private int walkingTime = 2;

        [SerializeField]
        private GameObject _target;
        
        [SerializeField]
        private Collider2D _collider;

        public float defencePower;

        // Start is called before the first frame update
        void Start()
        {
            _currentTime = Time.realtimeSinceStartup;

            _animator = GetComponent<Animator>();

            _direction = new Vector3(1, 0, 0);

            _spriteRenderer.flipX = !(_direction.x > 0);
            _collider = GetComponent<Collider2D>();
            
            // TODO: 몬스터 방어력 임시값
            defencePower = 10f;
        }

        // Update is called once per frame
        void Update()
        {
            // todo : 타겟 감지 >> 몬스터의 원형 시야 안에 플레이어가 충돌했는지 여부
            // todo : 충돌 했으면 attack 전환 (바로 그냥 공격하게 따라가지 말고)
            // todo : 시야를 벗어났으면 idle 전환
            
            _isDetect = false;

            if (_isWalking)
            {
                // walking 상태에서 walkingTime을 초과할 경우 idle 애니메이션 재생
                transform.Translate(_direction * speed * Time.deltaTime);

                if (Time.realtimeSinceStartup - _currentTime >= walkingTime)
                {
                    _isWalking = false;
                    _currentTime = Time.realtimeSinceStartup;
                }
            }
            else
            {
                // 지금 기다렸던 시간이 idleTime을 초과할 경우에 walk 애니메이션 재생
                if (Time.realtimeSinceStartup - _currentTime >= _idleTime)
                {
                    // 초기화
                    _currentTime = Time.realtimeSinceStartup;

                    if (_isWalking == false)
                    {
                        _direction *= -1;
                        _spriteRenderer.flipX = !(_direction.x > 0);
                    }

                    _isWalking = true;
                }
            }

            // if (_isHit)
            // {
            //     _animator.SetBool("Attack", _isDetect);
            //     
            //     _isWalking = false;
            //     
            // }
            _animator.SetBool("Walk", _isWalking); // 걷기 애니메이션
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

        public void ApplyDamage(float damage)
        {
            // 몬스터 피해 값 코드
            // 체력이 감소되는 부분
            // 몬스터 죽고 파괴되는 부분
            // 피격됐을때 애니메이션

            float prevHP = currentHP;
            currentHP -= damage;

            if (prevHP > 0 && currentHP <= 0)
            {
                _animator.SetBool("Die", true);  // 몬스터 죽는 애니메이션 트리거
                Destroy(gameObject);  // 몬스터 파괴
            }
            else if (prevHP > 0 && currentHP > 0)
            {
                _animator.SetBool("Hit", true); // 피격 애니메이션
            }
        }
        
        void MonsterAttack(GameObject player)
        {
            // 임시 반영 수정 예정
            DamageCalculator damageCalculator = new DamageCalculator();

            float baseAttack = 30f;
            float equipmentAttack = 10f;
            float defense = 0.1f;

            // 기본 데미지 계산 (치명타 없음)
            float damage = damageCalculator.CalculateBaseDamage(baseAttack, equipmentAttack, defense);

            Debug.Log($" 몬스터 공격! 최종 데미지: {damage}");
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            _isDetect = true;
            
            if (other.collider.CompareTag("Player"))
            {
                
                _animator.SetBool("Attack", true); // 공격 애니메이션 재생
                MonsterAttack(other.gameObject);  // 플레이어에게 공격
                Debug.Log("감지됨");
            }

            if (!other.collider.CompareTag("Player"))
            {
                _isDetect = false;
            }
        }
    }
}
