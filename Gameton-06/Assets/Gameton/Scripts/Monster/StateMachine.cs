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
            // Walking
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
        }
    }

    public class ChasingState : State
    {
        public void Enter(MonsterBase _monsterBase)
        {
            // 추적 상태 초기화
        }

        public void Update()
        {
            // 추적
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
        public void Enter(MonsterBase _monsterBase)
        {
            
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            
        }

        public void CheckTransition()
        {
            // 공격이 끝나면 다음 상태 전환
        }
    }
}
