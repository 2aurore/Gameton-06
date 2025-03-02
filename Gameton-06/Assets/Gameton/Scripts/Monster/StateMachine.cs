using TMPro;
using UnityEngine;

namespace TON
{
    public class StateMachine
    {
        private Animator animator; 
        private IState _state;
        private MonsterBase _monsterBase;
        private TextMeshProUGUI _textState;
        
        public StateMachine(IState state, MonsterBase monsterBase)
        {
            // 초기 상태 객체 생성
            _monsterBase = monsterBase;;
            
            _state = state;
            _state.Enter(_monsterBase);
        }

        public void Update()
        {
            _state.Update();

            var newState = _state.CheckTransition();

            if (_state != newState)
            {
                _state.Exit();
                SetTransition(newState);
            }
        }

        public void SetTransition(IState state)
        {
            // 다음음 상태로 전환
            _state = state;
            _state.Enter(_monsterBase);
        }
    }

    public interface IState
    {
        void Enter(MonsterBase monsterBase);
        
        void Update();

        void Exit();
        
        // 트리거 조건일 경우 다음 상태로 전환
        IState CheckTransition();
    }

    public class IdleState : IState
    {
        private const string AniIdle = "Idle";  // 대기 애니메이션
        private const string AniWalk = "Walk";  // 걷기 애니메이션
        
        private MonsterBase _monsterBase;
        private float _currentTime; 
        private float _idleTime = 2;
        
        private bool _isWalking;
        private int _walkingTime = 2;  // 걷기 시간 

        private float _duration = 2;
        private float _currentDuration;
        
        public void Enter(MonsterBase monsterBase)
        {
            _monsterBase = monsterBase;
            _currentTime = Time.realtimeSinceStartup;
            _monsterBase.ChangeAnimationState(AniIdle);

            _currentDuration = 0;
        }

        public void Update()
        {
            // Idle
            if (_isWalking == false)
            {
                // 대기 시간이 초과되면 걷기 시작
                if (Time.realtimeSinceStartup - _currentTime >= _idleTime)
                {
                    _currentTime = Time.realtimeSinceStartup;

                    // 걷기 상태가 아니라면 방향을 반대로 바꿔서 걷기 시작
                    _monsterBase.SetOppositionDirection();
                    _monsterBase.ChangeAnimationState(AniWalk);
                    
                    _isWalking = true;
                }
            }
            // 패트롤
            else
            {
                _monsterBase.Move();  // 몬스터를 이동시킴
                
                // 대기 시간이 초과되면 걷기 시작
                if (Time.realtimeSinceStartup - _currentTime >= _walkingTime)
                {
                    _currentTime = Time.realtimeSinceStartup;
                    
                    _isWalking = false;
                    _monsterBase.ChangeAnimationState(AniIdle);
                }
            }
            
            _currentDuration += Time.deltaTime;
        }

        public void Exit()
        {
            
        }

        public IState CheckTransition()
        {
            // duration 동안에는 항상 idle = 공격 쿨타운
            if (_currentDuration < _duration)
                return this;
            // TODO : 데미지 받을 때
            
            //  추적 범위에 들어왔을 때
            if (_monsterBase.IsDetect)
            {
                return new ChasingState();
            }

            return this;
        }
    }

    public class ChasingState : IState
    {
        private const string AniWalk = "Walk";  // 걷기 애니메이션
        
        private MonsterBase _monsterBase;
        
        public void Enter(MonsterBase monsterBase)
        {
            // 추적 상태 초기화
            
            _monsterBase = monsterBase;
            _monsterBase.ChangeAnimationState(AniWalk);
        }

        public void Update()
        {
            // 추적
            _monsterBase.Chasing();
        }

        public void Exit()
        {
            // 추적 끝났을때
        }

        public IState CheckTransition()
        {
            // Idle로 변경
            if(_monsterBase.IsDetect== false)
                return new IdleState();
            
            // Attack으로 변경
            if (_monsterBase.IsAttacking)
                return new AttackState();
            
            // 추적 상태
            return this;
        }
    }
    
    public class AttackState : IState
    {
        private const string AniAttack = "Attack"; // 공격 애니메이션
        private MonsterBase _monsterBase;
        private float _attackDelayTime = 2f;  // 공격 딜레이 시간
        private float _lastAttackTime;  // 마지막 공격 시간
        private float _attackAnimationDuration = 0.5f; // 공격 애니메이션 지속 시간
        private bool _isAttacking = false;
        
        public void Enter(MonsterBase monsterBase)
        {
            _monsterBase = monsterBase;
            _monsterBase.IsFinishAttack = false;
            
            _lastAttackTime = -_attackDelayTime; // 처음 진입시 바로 공격하도록 설정
        }

        public void Update()
        {
            if (!_isAttacking && Time.time >= _lastAttackTime + _attackDelayTime)
            {
                Attack();
                
                _lastAttackTime = Time.time;
                _isAttacking = true;
            }
        
            // 공격 애니메이션 종료 체크
            if (_isAttacking && Time.time >= _lastAttackTime + _attackAnimationDuration)
            { 
                _isAttacking = false;
            }
        }
        
        private void Attack()
        {
            _monsterBase.ChangeAnimationState(AniAttack);
            _monsterBase.Attack();
        }

        public void Exit()
        {
            _isAttacking = false;
        }

        public IState CheckTransition()
        {
            if(_monsterBase.IsFinishAttack == true)
                return new IdleState();
            
            return this;
        }
    }
    
    public class HitState : IState
    {
        private const string AniHit = "Hit";
        private MonsterBase _monsterBase;
        private float _hitDuration = 0.5f;
        private float _hitStartTime;

        public void Enter(MonsterBase monsterBase)
        {
            _monsterBase = monsterBase;
            _monsterBase.ChangeAnimationState(AniHit);
            _hitStartTime = Time.time;
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
        }

        public IState CheckTransition()
        {
            if (Time.time >= _hitStartTime + _hitDuration)
                return new IdleState();

            return this;
        }
    }

    public class DeathState : IState
    {
        private const string AniDeath = "Death";
        private MonsterBase _monsterBase;
        private float _deathDuration = 1f;
        private float _deathStartTime;
        private bool _deathAnimationStarted = false;

        public void Enter(MonsterBase monsterBase)
        {
            _monsterBase = monsterBase;
            _monsterBase.ChangeAnimationState(AniDeath);
            _deathStartTime = Time.time;
            _deathAnimationStarted = true;
        }

        public void Update()
        {
            if (_deathAnimationStarted && Time.time >= _deathStartTime + _deathDuration)
            {
                _monsterBase.DestroyMonster();
            }
        }

        public void Exit()
        {
        }

        public IState CheckTransition()
        {
            return this; // Death는 다른 상태로 전환되지 않음
        }
    }
}
