using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

namespace TON
{
    public class StateMachine
    {
        private Animator animator; 
        private State _state;
        private MonsterBase _monsterBase;

        public StateMachine(State state, MonsterBase monsterBase)
        {
            // 초기 상태 객체 생성
            _monsterBase = monsterBase;;
            
            _state = state;
            _state.Enter(_monsterBase);
        }

        public void Update()
        {
            _state.Update();
        }

        public void SetTransition(State state)
        {
            // 다음음 상태로 전환
            _state.Exit();
            
            _state = state;
            _state.Enter(_monsterBase);
        }
    }

    public interface State
    {
        void Enter(MonsterBase _monsterBase);
        
        void Update();

        void Exit();
        void CheckTransition();
        // TODO : 트리거 조건일 경우 다음 상태로 전환
        
    }

    public class IdleState : State
    {
        private const string AniIdle = "Idle";  // 대기 애니메이션
        private const string AniWalk = "Walk";  // 걷기 애니메이션
        
        private MonsterBase _monsterBase;
        private float _currentTime;
        private float _idleTime = 2;
        
        private bool _isWalking;
        private int _walkingTime = 2;  // 걷기 시간 
        
        public void Enter(MonsterBase monsterBase)
        {
            _monsterBase = monsterBase;
            _currentTime = Time.realtimeSinceStartup;
            _monsterBase.ChangeAnimationState(AniIdle);
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
        }

        public void Exit()
        {
            
        }

        public void CheckTransition()
        {
            // TODO : 데미지 받을 때
            // TODO : 추적 범위에 들어왔을 때  
            // chasingState로 가면 됨
        }
    }

    public class ChasingState : State
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

        public void CheckTransition()
        {
            // Idle로 변경
        }
    }

    public class AttackState : State
    {
        private const string AniAttack = "Attack"; // 공격 애니메이션
        private const string AniIdle = "Idle";  // 대기 애니메이션
        
        private MonsterBase _monsterBase;
        private float _attackDelayTime = 2f;  // 공격 딜레이 시간
        private float _lastAttackTime;  // 마지막 공격 시간
        private float _attackAnimationDuration = 0.5f; // 공격 애니메이션 지속 시간
        private bool _isAttacking = false;
        
        public void Enter(MonsterBase monsterBase)
        {
            _monsterBase = monsterBase;
            _lastAttackTime = -_attackDelayTime; // 처음 진입시 바로 공격하도록 설정
            Attack();
        }

        public void Update()
        {
            // 현재 공격 중이 아니고, 쿨다운이 지났다면 공격
            if (!_isAttacking && Time.time >= _lastAttackTime + _attackDelayTime)
            {
                Attack();
            }
        
            // 공격 애니메이션 종료 체크
            if (_isAttacking && Time.time >= _lastAttackTime + _attackAnimationDuration)
            {
                _isAttacking = false;
                _monsterBase.ChangeAnimationState(AniIdle);
            }
        }
        
        private void Attack()
        {
            _monsterBase.ChangeAnimationState(AniAttack);
            _monsterBase.PlayerAttack();
            _lastAttackTime = Time.time;
            _isAttacking = true;
        }

        public void Exit()
        {
            _isAttacking = false;
        }

        public void CheckTransition()
        {
            // if (_monsterBase.attackState)
            // {
            //     _stateMachine.Update();
            // }
        }
    }

    public class MonsterSkillState : State
    {
        private const string AniAttack = "Attack"; // 공격 애니메이션
        public GameObject smallFirePrefab;
        private MonsterBase _monsterBase;
        
        public void Enter(MonsterBase monsterBase)
        {
            smallFirePrefab = _monsterBase.smallFirePrefab;
            
        }

        public void Update()
        {
            _monsterBase.ChangeAnimationState(AniAttack);
            
        }

        public void Exit()
        {
            
        }

        public void CheckTransition()
        {
            
        }
    }
    //
    // public class HitState : State
    // {
    //     private const string AniHit = "Hit"; // 피격 애니메이션
    //     private MonsterBase _monsterBase;
    //     private float _hitStunDuration = 0.5f; // 피격 경직 시간
    //     private float _hitStartTime;
    //
    //     public void Enter(MonsterBase monsterBase)
    //     {
    //         _monsterBase = monsterBase;
    //         _hitStartTime = Time.time;
    //         _monsterBase.ChangeAnimationState(AniHit);
    //
    //         Debug.Log($"피격 상태 진입! 현재 HP: {_monsterBase.currentHP}");
    //     }
    //
    //     public void Update()
    //     {
    //         // 피격 경직 시간이 지나면 이전 상태로 복귀
    //         if (Time.time >= _hitStartTime + _hitStunDuration)
    //         {
    //             // HP가 0 이하면 죽음 상태로 전환
    //             if (_monsterBase.currentHP <= 0)
    //             {
    //                 _monsterBase.SetTransition(new DeadState());
    //             }
    //             else
    //             {
    //                 // 플레이어가 공격 범위 내에 있으면 공격 상태로, 아니면 추적 상태로
    //                 var target = GameObject.FindGameObjectWithTag("Player");
    //                 if (target != null)
    //                 {
    //                     float distanceToTarget =
    //                         Vector2.Distance(_monsterBase.transform.position, target.transform.position);
    //                     if (distanceToTarget <= _monsterBase.attackRange)
    //                     {
    //                         _monsterBase.SetTransition(new AttackState());
    //                     }
    //                     else
    //                     {
    //                         _monsterBase.SetTransition(new ChasingState());
    //                     }
    //                 }
    //             }
    //         }
    //     }
    //
    //     public void Exit()
    //     {
    //     }
    //
    //     public void CheckTransition()
    //     {
    //     }
    // }
    //
    // public class DeadState : State
    // {
    //     private const string AniDead = "Death"; // 죽음 애니메이션
    //     private MonsterBase _monsterBase;
    //     private float _deathAnimationDuration = 1f; // 죽음 애니메이션 재생 시간
    //     private float _deathStartTime;
    //     private bool _hasTriggeredDeath = false;
    //
    //     public void Enter(MonsterBase monsterBase)
    //     {
    //         _monsterBase = monsterBase;
    //         _deathStartTime = Time.time;
    //         _monsterBase.ChangeAnimationState(AniDead);
    //
    //         // 죽음 처리 시작
    //         StartDeathProcess();
    //     }
    //
    //     private void StartDeathProcess()
    //     {
    //         if (_hasTriggeredDeath) return;
    //
    //         _hasTriggeredDeath = true;
    //
    //         // 콜라이더 비활성화
    //         if (_monsterBase.GetComponent<Collider2D>() != null)
    //         {
    //             _monsterBase.GetComponent<Collider2D>().enabled = false;
    //         }
    //
    //         Debug.Log($"몬스터 사망: {_monsterBase.name}");
    //
    //         // 여기에 경험치 드롭, 아이템 드롭 등의 로직 추가 가능
    //     }
    //
    //     public void Update()
    //     {
    //         // 죽음 애니메이션이 끝나면 오브젝트 제거
    //         if (Time.time >= _deathStartTime + _deathAnimationDuration)
    //         {
    //             GameObject.Destroy(_monsterBase.gameObject);
    //         }
    //     }
    //
    //     public void Exit()
    //     {
    //     }
    //
    //     public void CheckTransition()
    //     {
    //         // 죽음 상태에서는 다른 상태로 전환되지 않음
    //     }
    // }
}
