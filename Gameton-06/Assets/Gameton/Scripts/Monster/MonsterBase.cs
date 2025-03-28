using TMPro;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace TON
{
    public partial class MonsterBase : MonoBehaviour, IDamage
    {
        [SerializeField] private SpriteRenderer _spriteRenderer; // 몬스터의 스프라이트 렌더러
        [SerializeField] public int id; // 몬스터의 ID
        public float defencePower;
        public float defenceIntention = 100;     // 몬스터 방어력 변수

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

        private Vector3 _direction; // 몬스터의 이동 방향
        public bool IsDetect { get; set; } // 몬스터가 대상을 인식했는지 여부
        public bool IsAttacking { get; set; } // 몬스터가 공격했는지 여부
        public bool IsFinishAttack { get; set; } // 몬스터 공격 모션이 끝났는지 여부

        public int Gold = 0;
        public int Exp = 0;
        public int Score = 0;

        private CharacterBase _characterBase;

        // public AudioClip _attackSound;
        public AudioClip _deathSound;
        public AudioClip _hitSound;

        private void Start()
        {
            _animator = GetComponent<Animator>(); // 애니메이터 컴포넌트 초기화

            _stateMachine = new StateMachine(new IdleState(), this);

            InitializeMonsterData();    // 몬스터 데이터 로드 및 적용

            _direction = new Vector3(1, 0, 0); // 초기 이동 방향 (x 축 양의 방향)

            _spriteRenderer.flipX = !(_direction.x > 0); // 이동 방향에 따라 스프라이트 플립

            // CharacterBase 참조 설정
            _characterBase = GameObject.Find("TON.Player").GetComponentInChildren<CharacterBase>();

            // HP 바 초기화
            if (_hpBarImage != null)
            {
                RectTransform rectTransform = _hpBarImage.GetComponent<RectTransform>();
                hpMaxWidth = rectTransform.sizeDelta.x;  // 초기 최대 너비 저장
                _maxHP = _monsterData.hp;
                currentHP = _maxHP;
            }
        }

        private void InitializeMonsterData()
        {
            _monsterData = MonsterDataManager.Singleton.GetMonsterData(id);

            if (_monsterData != null)
            {
                _maxHP = _monsterData.hp;
                currentHP = _maxHP;
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
        }

        public void FinishAttack()
        {
            IsFinishAttack = true;
        }

        public void ApplyDamage(float damage)
        {
            float prevHP = currentHP;   // 몬스터의 체력을 감소시키고, 죽었을 경우 파괴 처리
            currentHP -= damage;

            SoundManager.instance.SFXPlay("Hit", _hitSound);

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
                // 현재 HP가 0 이하로 내려가지 않도록 보정
                currentHP = Mathf.Max(0, currentHP);

                // HP 비율 계산 (0~1 사이 값)
                float hpRatio = currentHP / _maxHP;

                // RectTransform 컴포넌트 가져오기
                RectTransform rectTransform = _hpBarImage.GetComponent<RectTransform>();

                // 현재 크기 가져오기
                Vector2 sizeDelta = rectTransform.sizeDelta;

                // x 크기를 HP 비율에 따라 조정
                sizeDelta.x = hpMaxWidth * hpRatio;

                // 변경된 크기 적용
                rectTransform.sizeDelta = sizeDelta;
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

            _characterBase.ApplyDamage(damage);

            // SoundManager.instance.SFXPlay("Attack", _attackSound);
            // Debug.Log($" 몬스터 공격! 최종 데미지: {damage}"); // 데미지 출력
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
            var target = GameObject.Find("TON.Player").GetComponentInChildren<CharacterBase>();
            if (target != null)
            {
                Vector2 direction = target.transform.position - transform.position; // 타겟과의 방향 계산
                _spriteRenderer.flipX =
                    target.transform.position.x <
                    transform.position.x; // 타겟이 왼쪽에 있으면 스프라이트를 왼쪽으로, 오른쪽에 있으면 오른쪽으로 바라보도록 설정

                transform.Translate(direction.normalized * moveSpeed * Time.deltaTime); // 타겟 방향으로 이동
            }
        }

        public void DestroyMonster()
        {
            RewardData();
            SoundManager.instance.SFXPlay("Death", _deathSound);
            Destroy(gameObject); // 몬스터 파괴
        }
    }
}