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
        public int id; // 몬스터의 ID
        public float currentHP = 100; // 몬스터의 현재 체력

        // public string name; // 몬스터 이름
        public string monsterType; // 몬스터의 타입 (예: melee, ranged)

        // public int damage;  // 공격력
        public float speed = 2; // 몬스터의 이동 속도

        [SerializeField] private SpriteRenderer _spriteRenderer; // 몬스터의 스프라이트 렌더러
        private Animator _animator; // 몬스터 애니메이터

        private Vector3 _direction; // 몬스터의 이동 방향
        private bool _isWalking; // 몬스터가 걷고 있는지 여부
        private bool _isHit; // 몬스터가 맞았는지 여부
        private bool _isDetect; // 몬스터가 대상을 인식했는지 여부

        // [SerializeField] private int _idleTime = 2; // 대기 시간
        //
        // [SerializeField] private int walkingTime = 2; // 걷기 시간 

        [SerializeField] private GameObject _target; // 몬스터의 타겟

        [SerializeField] private Collider2D _collider; // 몬스터의 콜라이더

        public float defencePower; // 몬스터의 방어력

        // 애니메이션 관련 선언
        private string currentAnimationState; // 현재 애니메이션 상태
        
        StateMachine _stateMachine;
        
        // 추적 관련 선언
        private float _detectStartTime; // 추적 시작 시간
        private bool _isTracking; // 추적 중인지 여부

        private const float DETECT_DURATION = 5f; // 추적 지속 시간

        
        
        public bool IsDetect
        {
            get => _isDetect;
            set => _isDetect = value;
        }

        // 첫 번째 프레임 전에 호출됩니다.
        private void Start()
        {
            _animator = GetComponent<Animator>(); // 애니메이터 컴포넌트 초기화

            _stateMachine = new StateMachine(new IdleState(), this);
            
            // 몬스터 데이터 로드 (테스트용, 첫 번째 몬스터 데이터만 로드)
            MonsterData monsterData = MonsterDataManager.Singleton.monstersData[0];
            Debug.Log(monsterData.name); // 몬스터 ID 출력

            _direction = new Vector3(1, 0, 0); // 초기 이동 방향 (x 축 양의 방향)

            _spriteRenderer.flipX = !(_direction.x > 0); // 이동 방향에 따라 스프라이트 플립

            _collider = GetComponent<Collider2D>(); // 콜라이더 컴포넌트 초기화

            // 몬스터 방어력 임시값 설정
            defencePower = 10f;
        }

        // 애니메이션 상태를 변경하는 메서드
        public void ChangeAnimationState(string newState)
        {
            // 현재 상태와 동일한 상태일 경우, 애니메이션을 변경하지 않음
            if (currentAnimationState == newState) return;

            _animator.Play(newState); // 새로운 애니메이션 상태 실행
        }
        
        private void Update()
        {
            _stateMachine.Update();
            
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
                Destroy(gameObject); // 몬스터 파괴
            }
            else if (prevHP > 0 && currentHP > 0)
            {
                // 피격 애니메이션은 주석 처리됨 (필요시 활성화)
            }
        }

        // 타겟을 공격하는 메서드
        public void Attack(GameObject player)
        {
            // ChangeAnimationState(AniAttack); // 공격 애니메이션으로 변경

            // 데미지 계산 (현재 임시 값)
            DamageCalculator damageCalculator = new DamageCalculator();

            float baseAttack = 30f; // 기본 공격력
            float equipmentAttack = 10f; // 장비 공격력
            float defense = 0.1f; // 방어력 비율

            // 기본 데미지 계산 (치명타 없음)
            float damage = damageCalculator.CalculateBaseDamage(baseAttack, equipmentAttack, defense);

            Debug.Log($" 몬스터 공격! 최종 데미지: {damage}"); // 데미지 출력
        }

        public void PlayerAttack()
        {
            var target = GameObject.FindGameObjectWithTag("Player");
            Attack(target);   
        }

        public void SetOppositionDirection()
        {
            _direction *= -1; // 이동 방향 반전
            _spriteRenderer.flipX = !(_direction.x > 0); // 스프라이트 방향 반전
        }

        public void Move()
        {
            transform.Translate(_direction * speed * Time.deltaTime);  // 몬스터를 이동시킴
        }

        public void Chasing()
        {
            var target = GameObject.FindGameObjectWithTag("Player");
            UnityEngine.Vector2 direction = target.transform.position - transform.position; // 타겟과의 방향 계산
            // 타겟이 왼쪽에 있으면 스프라이트를 왼쪽으로, 오른쪽에 있으면 오른쪽으로 바라보도록 설정
            _spriteRenderer.flipX = target.transform.position.x < transform.position.x;

            transform.Translate(direction.normalized * speed * Time.deltaTime); // 타겟 방향으로 이동
        }

        public void SetTransition(State state)
        {
            _stateMachine.SetTransition(state);
        }
    }
}
