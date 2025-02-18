using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Assets.PixelFantasy.PixelMonsters.Common.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DamageCalculator = TON.DamageCalculator;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace TON
{
    public class MonsterBase : MonoBehaviour, IDamage
    {
        
        [SerializeField]
        public int id; // 몬스터의 ID
        public float defencePower;
        
        [SerializeField] private SpriteRenderer _spriteRenderer; // 몬스터의 스프라이트 렌더러
        
        private MonsterData _monsterData;
        private MonsterSkillData _monsterSkillData;
        private MonsterSkillData _monsterSkillDataTwo;
        private Animator _animator; // 몬스터 애니메이터
        
        StateMachine _stateMachine;

        private AttackPattern _attackPattern;
        
        private Vector3 _direction; // 몬스터의 이동 방향
        public bool IsDetect { get; set; } // 몬스터가 대상을 인식했는지 여부
        public bool IsAttacking { get; set; } // 몬스터가 공격했는지 여부
        public bool IsFisnishAttack { get; set; } // 몬스터 공격 모션이 끝났는지 여부

        [SerializeField] private GameObject _target; // 몬스터의 타겟

        [SerializeField] private Collider2D _collider; // 몬스터의 콜라이더

        // 애니메이션 관련 선언
        private string currentAnimationState; // 현재 애니메이션 상태
        
        // hp바
        [SerializeField] private Image _hpBarImage; // HP 바 이미지
        private float _maxHP;
        private float currentHP;
        
        // 몬스터 스킬 프리팹
        public GameObject smallFirePrefab;
        public GameObject DragonBreathPrefab;
        public GameObject IceBlastPrefab;
        public GameObject PumpkinCrashPrefab;
        public GameObject TrollChargePrefab;
        public GameObject TrollCrashPrefab;
        public GameObject TrollThundePrefab;
        public GameObject WolfEnergyWavePrefab;
        public GameObject WolfPunchPrefab;
        public GameObject DragonShockWavePrefab;
        public GameObject FireImpactPrefab;
        
        // 첫 번째 프레임 전에 호출됩니다.
        private void Start()
        {   // 전략 패턴
            // TODO : 수정중 
            // _attackPattern = new Monster1AttackPattern();
            // _attackPattern = new Monster2AttackPattern();
            
            _animator = GetComponent<Animator>(); // 애니메이터 컴포넌트 초기화

            _stateMachine = new StateMachine(new IdleState(), this);
            
            // 몬스터 데이터 로드 및 적용
            InitializeMonsterData();
            InitializeMonsterSkillData();

            id = _monsterData.id;

            _direction = new Vector3(1, 0, 0); // 초기 이동 방향 (x 축 양의 방향)

            _spriteRenderer.flipX = !(_direction.x > 0); // 이동 방향에 따라 스프라이트 플립

            _collider = GetComponent<Collider2D>(); // 콜라이더 컴포넌트 초기화
        }
        
        // TODO : 불러온 값 변수에 대응하게 수정
        private void InitializeMonsterData()
        {
            _monsterData = MonsterDataManager.Singleton.GetMonsterData(id);
            
            if (_monsterData != null)
            {
                _maxHP = _monsterData.hp;
                currentHP = _maxHP;
                
                defencePower = _monsterData.defencePower;
            
                Debug.Log($"몬스터 {_monsterData.name} 데이터 로드 완료");
            }
            else
            {
                Debug.LogError($"몬스터 ID {id}에 대한 데이터를 찾을 수 없습니다.");
            }
        }

        // TODO : 불러온 값 변수에 대응하게 수정
        private void InitializeMonsterSkillData()
        {
            _monsterSkillData = MonsterSkillDataManager.Singleton.GetMonsterSkillData(_monsterData.monsterSkillID);
            if (_monsterData.monsterSkillIDTwo > -1)
            {
                _monsterSkillDataTwo = MonsterSkillDataManager.Singleton.GetMonsterSkillData(_monsterData.monsterSkillIDTwo);
            }

            if (_monsterSkillData != null && _monsterSkillDataTwo != null)
            {
            
                Debug.Log($"몬스터 {_monsterSkillData.skillName} 데이터 로드 완료");
                Debug.Log($"몬스터 {_monsterSkillDataTwo.skillName} 데이터 로드 완료");
            }
            else
            {
                Debug.LogError($"몬스터 스킬 ID {_monsterSkillData.skillId}에 대한 데이터를 찾을 수 없습니다.");
                Debug.LogError($"몬스터 스킬 ID {_monsterSkillDataTwo.skillId}에 대한 데이터를 찾을 수 없습니다.");
            }
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

        public void FinishAttack()
        {
            IsFisnishAttack = true;
        }
        
        // 피해를 적용하는 메서드
        public void ApplyDamage(float damage)
        {
            // 몬스터의 체력을 감소시키고, 죽었을 경우 파괴 처리
            float prevHP = currentHP;
            currentHP -= damage;
            
            // HP 바 업데이트
            UpdateHPBar();
            
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
        
        // HP 바 업데이트
        private void UpdateHPBar()
        {
            if (_hpBarImage != null)
            {
                // 현재 체력 비율 계산 (0.0 ~ 1.0)
                float hpRatio = currentHP / _maxHP;
                _hpBarImage.fillAmount = hpRatio; // 이미지 크기를 체력 비율에 맞게 조절
            }
        }

        // 타겟을 공격하는 메서드
        public void Attack(GameObject player)
        {
            // ChangeAnimationState(AniAttack); // 공격 애니메이션으로 변경

            // 데미지 계산 (현재 임시 값)
            DamageCalculator damageCalculator = new DamageCalculator();

            float baseAttack = _monsterData.attackPower; // 기본 공격력
            float equipmentAttack = 0; // 장비 공격력
            float defense = _monsterData.defencePower; // 방어력 비율

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
            transform.Translate(_direction * _monsterData.moveSpeed * Time.deltaTime);  // 몬스터를 이동시킴
        }

        public void Chasing()
        {
            var target = GameObject.FindGameObjectWithTag("Player");
            UnityEngine.Vector2 direction = target.transform.position - transform.position; // 타겟과의 방향 계산
            // 타겟이 왼쪽에 있으면 스프라이트를 왼쪽으로, 오른쪽에 있으면 오른쪽으로 바라보도록 설정
            _spriteRenderer.flipX = target.transform.position.x < transform.position.x;

            transform.Translate(direction.normalized * _monsterData.moveSpeed * Time.deltaTime); // 타겟 방향으로 이동
        }

        public void MonsterSkillLaunch()
        {
            var target = GameObject.FindGameObjectWithTag("Player");
            _spriteRenderer.flipX = target.transform.position.x < transform.position.x;
            
            // TODO : 몬스터가 가지고 있는 스킬에 따라 분기되는 조건 추가
            // GameObject newSkill = Instantiate(smallFirePrefab);
            // GameObject newSkill = Instantiate(DragonBreathPrefab);
            GameObject newSkill = Instantiate(IceBlastPrefab);
            // GameObject newSkill = Instantiate(PumpkinCrashPrefab);
            // GameObject newSkill = Instantiate(TrollChargePrefab);
            // GameObject newSkill = Instantiate(TrollCrashPrefab);
            // GameObject newSkill = Instantiate(TrollThundePrefab);
            // GameObject newSkill = Instantiate(WolfEnergyWavePrefab);
            // GameObject newSkill = Instantiate(WolfPunchPrefab);
            // GameObject newSkill = Instantiate(DragonShockWavePrefab);
            // GameObject newSkill = Instantiate(FireImpactPrefab);
            

            newSkill.transform.position = transform.position + new Vector3(0, 1f, 0);
            newSkill.GetComponent<MonsterSkill>().Direction = new Vector2(0, 1);
        }
    }
}
