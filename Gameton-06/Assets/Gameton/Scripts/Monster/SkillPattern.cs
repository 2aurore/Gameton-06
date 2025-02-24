using UnityEngine;

namespace TON
{
    public abstract class SkillPattern
    {
        protected MonsterData _monsterData;
        protected MonsterBase _monsterBase;
        
        protected SkillPattern(MonsterData monsterData, MonsterBase monsterBase)
        {
            _monsterData = monsterData;
            _monsterBase = monsterBase;
        }

        public bool IsAttackable { get; set; }

        public abstract void Attack(GameObject target);
        

        public abstract void Update();
    }

    public class Monster1SkillPattern : SkillPattern 
    {
        private float _skill1CoolTime;
        private float _skill2CoolTime;
        
        private MonsterSkillData _monsterSkillData;
        private MonsterSkillData _monsterSkillDataTwo;

        private MonsterSkill _skill1;
        private MonsterSkill _skill2;
        
        private Vector3 _skillOffset = new Vector3(0, -0.5f, 0); // 스킬 생성 위치 조정값
        
        public Monster1SkillPattern(MonsterData monsterData, MonsterBase monsterBase) : base(monsterData, monsterBase)
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
                
                // 프리팹을 연결한 코드
                _skill1 = Resources.Load<MonsterSkill>($"MonsterSkillPrefabs/{_monsterSkillData.skillName}");
                _skill2 = Resources.Load<MonsterSkill>($"MonsterSkillPrefabs/{_monsterSkillDataTwo.skillName}");
            }
            else
            {
                Debug.LogError($"몬스터 스킬 ID {_monsterSkillData.skillId}에 대한 데이터를 찾을 수 없습니다.");
                Debug.LogError($"몬스터 스킬 ID {_monsterSkillDataTwo.skillId}에 대한 데이터를 찾을 수 없습니다.");
            }

            _skill1CoolTime = Time.realtimeSinceStartup;
            _skill2CoolTime = Time.realtimeSinceStartup;
            
        }

        public override void Attack(GameObject target)
        {
            if (target == null) return;
            
            // 스킬 스프라이트 방향 플레이어 바라보게
            _monsterBase.GetComponent<SpriteRenderer>().flipX = target.transform.position.x < _monsterBase.transform.position.x;
            // 몬스터의 현재 위치에서 offset만큼 아래에 스킬 생성
            Vector3 spawnPosition = _monsterBase.transform.position - _skillOffset;
    
            // 프리팹을 지정된 위치에 생성
            Object.Instantiate(_skill1, spawnPosition, Quaternion.identity);
        }

        public override void Update()
        {
            if (Time.realtimeSinceStartup - _skill1CoolTime >= _monsterSkillData.cooldown)
            {
                // TODO : 범위 체크
                IsAttackable = true;
            }
            
            if (Time.realtimeSinceStartup - _skill2CoolTime >= _monsterSkillDataTwo.cooldown)
            {
                // TODO : 범위 체크
                IsAttackable = true;
            }
        }
    }
    
    // public class Monster2AttackPattern : AttackPattern
    // {
    //     
    //
    //     public Monster2AttackPattern(MonsterBase monsterBase) : base(monsterBase)
    //     {
    //         
    //     }
    //
    //     public override void Attack()
    //     {
    //
    //         Skill1();
    //         
    //         Skill2();
    //         
    //         MeleeAttack();
    //
    //     }
    //
    //     private void Skill1()
    //     {
    //         
    //     }
    //
    //     private void Skill2()
    //     {
    //         
    //     }
    //
    //     private void MeleeAttack()
    //     {
    //         _monsterBase.PlayerAttack();
    //     }
    // }
}
