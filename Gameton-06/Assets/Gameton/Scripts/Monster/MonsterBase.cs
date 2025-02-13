using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Assets.PixelFantasy.PixelMonsters.Common.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using DamageCalculator = TON.DamageCalculator;
using Vector3 = UnityEngine.Vector3;

namespace TON
{
    public class MonsterBase : MonoBehaviour, IDamage
    {
        public int id;  // 몬스터의 ID
        public float currentHP = 100;  // 몬스터의 현재 체력
        
        // public string name; // 몬스터 이름
        public string monsterType;  // 몬스터의 타입 (예: melee, ranged)
        
        // public int damage;  // 공격력
        public float speed = 2; // 몬스터의 이동 속도
        
        [SerializeField]
        private SpriteRenderer _spriteRenderer;  // 몬스터의 스프라이트 렌더러
        private Animator _animator;  // 몬스터 애니메이터
        
        private Vector3 _direction;  // 몬스터의 이동 방향
        private bool _isWalking;  // 몬스터가 걷고 있는지 여부
        private bool _isHit;  // 몬스터가 맞았는지 여부
        private bool _isDetect;  // 몬스터가 대상을 인식했는지 여부
        private float _currentTime;  // 현재 시간

        [SerializeField]
        private int _idleTime = 2;  // 대기 시간
        
        [SerializeField]
        private int walkingTime = 2;  // 걷기 시간 

        [SerializeField]
        private GameObject _target;  // 몬스터의 타겟

        [SerializeField]
        private Collider2D _collider;  // 몬스터의 콜라이더
        
        public float defencePower;  // 몬스터의 방어력

        // 애니메이션 관련 선언
        private string currentState;  // 현재 애니메이션 상태
        
        const string AniIdle = "Idle";  // 대기 애니메이션
        const string AniWalk = "Walk";  // 걷기 애니메이션
        const string AniAttack = "Attack";  // 공격 애니메이션
        
        public bool IsWalking
        {
            get => _isWalking;
            set => _isWalking = value;
        }

        public bool IsDetect
        {
            get => _isDetect;
            set => _isDetect = value;
        }
        
        // 첫 번째 프레임 전에 호출됩니다.
        void Start()
        {
            // 몬스터 데이터 로드 (테스트용, 첫 번째 몬스터 데이터만 로드)
            MonsterData monsterData = MonsterDataManager.Singleton.monstersData[0];
            Debug.Log(monsterData.name);  // 몬스터 ID 출력
            
            _currentTime = Time.realtimeSinceStartup;  // 시작 시간 기록

            _animator = GetComponent<Animator>();  // 애니메이터 컴포넌트 초기화

            _direction = new Vector3(1, 0, 0);  // 초기 이동 방향 (x 축 양의 방향)

            _spriteRenderer.flipX = !(_direction.x > 0);  // 이동 방향에 따라 스프라이트 플립

            _collider = GetComponent<Collider2D>();  // 콜라이더 컴포넌트 초기화
            
            // 몬스터 방어력 임시값 설정
            defencePower = 10f;
        }

        // 애니메이션 상태를 변경하는 메서드
        void ChangeAnimationState(string newState)
        {
            // 현재 상태와 동일한 상태일 경우, 애니메이션을 변경하지 않음
            if(currentState == newState) return;
            
            _animator.Play(newState);  // 새로운 애니메이션 상태 실행
        }
        
        // 매 프레임 호출됩니다.
        void Update()
        {
            // TODO: 시야를 벗어났으면 idle 상태로 전환하는 기능 추가 예정
            
            // 몬스터가 걷고 있는 경우
            if (_isWalking)
            {
                transform.Translate(_direction * speed * Time.deltaTime);  // 몬스터를 이동시킴

                // 걷기 시간을 초과하면 대기 상태로 전환
                if (Time.realtimeSinceStartup - _currentTime >= walkingTime)
                {
                    _isWalking = false;
                    ChangeAnimationState(AniIdle);  // 애니메이션을 Idle로 변경
                    _currentTime = Time.realtimeSinceStartup;  // 시간 갱신
                }
            }
            else
            {
                // 대기 시간이 초과되면 걷기 시작
                if (Time.realtimeSinceStartup - _currentTime >= _idleTime)
                {
                    _currentTime = Time.realtimeSinceStartup;

                    // 걷기 상태가 아니라면 방향을 반대로 바꿔서 걷기 시작
                    if (_isWalking == false)
                    {
                        _direction *= -1;  // 이동 방향 반전
                        _spriteRenderer.flipX = !(_direction.x > 0);  // 스프라이트 방향 반전
                    }

                    _isWalking = true;
                    ChangeAnimationState(AniWalk);  // 애니메이션을 Walk로 변경
                }
            }

            // 대상을 인식하고 있으면 공격 또는 추적 처리
            if (_isDetect && _target != null)
            {
                // 몬스터와 타겟 간의 거리 계산
                float distance = Vector3.Distance(transform.position, _target.transform.position);

                if (distance < 1.5f)  // 일정 거리 이내에서 공격
                {
                    Attack(_target);
                }
                else
                {
                    Detect(_target);  // 타겟이 멀어지면 추적
                }
            }
        }

        // 피해를 적용하는 메서드
        public void ApplyDamage(float damage)
        {
            // 몬스터의 체력을 감소시키고, 죽었을 경우 파괴 처리
            float prevHP = currentHP;
            currentHP -= damage;

            if (prevHP > 0 && currentHP <= 0)
            {
                // 몬스터가 죽었을 때 처리 (죽는 애니메이션은 주석 처리됨)
                Destroy(gameObject);  // 몬스터 파괴
            }
            else if (prevHP > 0 && currentHP > 0)
            {
                // 피격 애니메이션은 주석 처리됨 (필요시 활성화)
            }
        }
        
        // 타겟을 공격하는 메서드
        public void Attack(GameObject player)
        {
            ChangeAnimationState(AniAttack);  // 공격 애니메이션으로 변경
            
            // 데미지 계산 (현재 임시 값)
            DamageCalculator damageCalculator = new DamageCalculator();

            float baseAttack = 30f;  // 기본 공격력
            float equipmentAttack = 10f;  // 장비 공격력
            float defense = 0.1f;  // 방어력 비율

            // 기본 데미지 계산 (치명타 없음)
            float damage = damageCalculator.CalculateBaseDamage(baseAttack, equipmentAttack, defense);

            Debug.Log($" 몬스터 공격! 최종 데미지: {damage}");  // 데미지 출력
        }

        // 타겟을 추적하는 메서드
        public void Detect(GameObject target)
        {
            if (target == null) return;  // 타겟이 없으면 리턴

            UnityEngine.Vector2 direction = target.transform.position - transform.position;  // 타겟과의 방향 계산

            if (direction.magnitude > 0)  // 타겟이 존재하면 이동
            {
                transform.Translate(direction.normalized * speed * Time.deltaTime);  // 타겟 방향으로 이동
                ChangeAnimationState(AniWalk);  // 걷기 애니메이션으로 변경
        
                // 타겟 방향에 따라 스프라이트 방향 즉시 변경
                _spriteRenderer.flipX = direction.x < 0;  // 타겟이 왼쪽에 있으면 반전, 오른쪽에 있으면 정방향
            }
        }
    }
}
