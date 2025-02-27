using TMPro;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace TON
{
    public partial class MonsterBase : MonoBehaviour, IDamage
    {
        [SerializeField] private GameObject _target; // 몬스터의 타겟
        [SerializeField] private Collider2D _collider; // 몬스터의 콜라이더
        [SerializeField] private SpriteRenderer _spriteRenderer; // 몬스터의 스프라이트 렌더러
        // [SerializeField] private TextMeshProUGUI _textState;
        [SerializeField] public int id; // 몬스터의 ID
        public float defencePower;
        public float defenceIntention = 30;     // 몬스터 방어력 변수

        public GameObject _hpBarImage; // HP 바 이미지
        private float _maxHP;
        public float currentHP { get; private set; }    // 몬스터 현재 체력
        private float hpMaxWidth;

        private PlayerData _playerData;
        private MonsterData _monsterData;
        private Animator _animator; // 몬스터 애니메이터
        private string currentAnimationState; // 현재 애니메이션 상태
        private float moveSpeed = 2f;
        
        StateMachine _stateMachine;
        private SkillPattern _skillPattern;

        private Vector3 _direction; // 몬스터의 이동 방향
        public bool IsDetect { get; set; } // 몬스터가 대상을 인식했는지 여부
        public bool IsAttacking { get; set; } // 몬스터가 공격했는지 여부
        public bool IsFinishAttack { get; set; } // 몬스터 공격 모션이 끝났는지 여부
        public bool IsSkillAttackable => _skillPattern.IsAttackable;

        public int Gold = 0;
        public int Exp = 0;
        public int Score = 0;

        private void Start()
        {
            _animator = GetComponent<Animator>(); // 애니메이터 컴포넌트 초기화

            // TODO : 추후 제거 예정
            // _stateMachine = new StateMachine(new IdleState(), this, _textState);
            _stateMachine = new StateMachine(new IdleState(), this);

            InitializeMonsterData();    // 몬스터 데이터 로드 및 적용

            _skillPattern = new Monster1SkillPattern(_monsterData, this);

            _direction = new Vector3(1, 0, 0); // 초기 이동 방향 (x 축 양의 방향)

            _spriteRenderer.flipX = !(_direction.x > 0); // 이동 방향에 따라 스프라이트 플립

            _collider = GetComponent<Collider2D>(); // 콜라이더 컴포넌트 초기화
        }

        private void InitializeMonsterData()
        {
            _monsterData = MonsterDataManager.Singleton.GetMonsterData(id);

            if (_monsterData != null)
            {
                currentHP = _monsterData.hp;
                defencePower = _monsterData.defencePower;
                Gold = _monsterData.Gold;
                Exp = _monsterData.Exp;
                Score = _monsterData.Score;

                // Debug.Log($"몬스터 {_monsterData.name} 데이터 로드 완료");
            }
            else
            {
                Debug.LogError($"몬스터 ID {id}에 대한 데이터를 찾을 수 없습니다.");
            }
        }

        public void RewardData()
        {
            StageManager.Singleton.SetRewardData(Gold, Exp, Score);
        }

        public void ChangeAnimationState(string newState)
        {
            if (currentAnimationState == newState) return;  // 현재 상태와 동일한 상태일 경우, 애니메이션을 변경하지 않음

            _animator.Play(newState); // 새로운 애니메이션 상태 실행
        }

        private void Update()
        {
            _stateMachine.Update();
            _skillPattern.Update();
        }

        public void FinishAttack()
        {
            IsFinishAttack = true;
        }

        public void ApplyDamage(float damage)
        {
            float prevHP = currentHP;   // 몬스터의 체력을 감소시키고, 죽었을 경우 파괴 처리
            currentHP -= damage;

            UpdateHPBar(currentHP);

            if (prevHP > 0 && currentHP <= 0)
            {
                _stateMachine.SetTransition(new DeathState());
                //TODO : 현재 웨이브 값, 경험치, 골드 MonsterSpawner.WaveData;
            }
            else if (prevHP > 0 && currentHP > 0)
            {
                _stateMachine.SetTransition(new HitState());
            }
        }

        private void UpdateHPBar(float currentHP)
        {
            if (_hpBarImage != null)
            {
                float minHPBarWidth = 5f; // 최소 HP 바 길이 (원하는 값으로 설정)
                float hpBarWidth = Mathf.Max(currentHP / _maxHP * hpMaxWidth, minHPBarWidth); // 최소 길이 적용

                _hpBarImage.GetComponent<RectTransform>().sizeDelta = new Vector2(hpBarWidth, _hpBarImage.GetComponent<RectTransform>().sizeDelta.y);
            }
        }

        public void Attack()
        {
            // 데미지 계산 (현재 임시 값)
            DamageCalculator damageCalculator = new DamageCalculator();

            float baseAttack = _monsterData.attackPower; // 기본 공격력
            float equipmentAttack = 0; // 장비 공격력
            float defense = PlayerDataManager.Singleton.player.defensivePower / (PlayerDataManager.Singleton.player.defensivePower + PlayerDataManager.Singleton.defensiveIntention); // 캐릭터 방어력
            
            // 기본 데미지 계산 (치명타 없음)
            float damage = damageCalculator.CalculateBaseDamage(baseAttack, equipmentAttack, defense);

            Debug.Log($" 몬스터 공격! 최종 데미지: {damage}"); // 데미지 출력
        }

        public void SetOppositionDirection()
        {
            _direction *= -1; // 이동 방향 반전
            _spriteRenderer.flipX = !(_direction.x > 0); // 스프라이트 방향 반전
        }

        public void Move()
        {
            transform.Translate(_direction * moveSpeed * Time.deltaTime);  // 몬스터를 이동시킴
        }

        public void Chasing()
        {
            var target = GameObject.FindGameObjectWithTag("Player");
            UnityEngine.Vector2 direction = target.transform.position - transform.position; // 타겟과의 방향 계산
            _spriteRenderer.flipX = target.transform.position.x < transform.position.x; // 타겟이 왼쪽에 있으면 스프라이트를 왼쪽으로, 오른쪽에 있으면 오른쪽으로 바라보도록 설정

            transform.Translate(direction.normalized * moveSpeed * Time.deltaTime); // 타겟 방향으로 이동
        }

        public void MonsterSkillLaunch()
        {
            if (_monsterData.monsterSkillID != 0)
            {
                var target = GameObject.FindGameObjectWithTag("Player");
                _skillPattern.Attack(target);
            }
        }

        public void DestroyMonster()
        {
            RewardData();
            Destroy(gameObject); // 몬스터 파괴
        }
    }
}