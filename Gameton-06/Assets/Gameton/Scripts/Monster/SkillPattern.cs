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
        
        public abstract bool IsSkillInCooldown();

        public abstract void Attack(GameObject target);
        
        public abstract void Update();
    }

    public class MonsterSkillPattern : SkillPattern 
    {
        private float _lastSkillTime;
        private MonsterSkillData _monsterSkillData;
        private MonsterSkill _skill;
        private Vector3 _skillOffset = new Vector3(0, -0.5f, 0);
        
        public MonsterSkillPattern(MonsterData monsterData, MonsterBase monsterBase) : base(monsterData, monsterBase)
        {
            _monsterSkillData = MonsterSkillDataManager.Singleton.GetMonsterSkillData(_monsterData.monsterSkillID);
            _lastSkillTime = -_monsterSkillData.cooldown; // 시작시 스킬 사용 가능하도록
            IsAttackable = true;
            
            if (_monsterSkillData != null)
            {
                // Debug.Log($"몬스터 {_monsterSkillData.skillName} 데이터 로드 완료");
                _skill = Resources.Load<MonsterSkill>($"MonsterSkillPrefabs/{_monsterSkillData.skillName}");
            }
        }

        public override void Attack(GameObject target)
        {
            if (target == null) return;
            
            _monsterBase.GetComponent<SpriteRenderer>().flipX = target.transform.position.x < _monsterBase.transform.position.x;
            Vector3 spawnPosition = _monsterBase.transform.position - _skillOffset;
            
            Object.Instantiate(_skill, spawnPosition, Quaternion.identity);
            SkillAttack(_monsterSkillData.damage);
            
            // 스킬 사용 후 쿨다운 시작
            _lastSkillTime = Time.time;
            IsAttackable = false;
            
            // Debug.Log($"스킬 사용, 쿨다운 시작: {_monsterSkillData.cooldown}초");
        }

        public void SkillAttack(float skillDamage)
        {
            // 데미지 계산 (현재 임시 값)
            DamageCalculator damageCalculator = new DamageCalculator();

            float baseAttack = _monsterData.attackPower * skillDamage; // 기본 공격력 * 스킬 데미지
            float equipmentAttack = 0; // 장비 공격력
            float defense = PlayerDataManager.Singleton.player.defensivePower / (PlayerDataManager.Singleton.player.defensivePower + PlayerDataManager.Singleton.defensiveIntention); // 캐릭터 방어력
            
            // 기본 데미지 계산 (치명타 없음)
            float damage = damageCalculator.CalculateBaseDamage(baseAttack, equipmentAttack, defense);
            
            _skill.SetSkillDamage(damage);
            
            // Debug.Log($" 몬스터 스킬 공격! 최종 데미지: {damage}"); // 데미지 출력
        }

        public override void Update()
        {
            // 스킬 쿨다운 체크
            if (!IsAttackable && Time.time - _lastSkillTime >= _monsterSkillData.cooldown)
            {
                IsAttackable = true;
                // Debug.Log("스킬 쿨다운 완료");
            }
        }

        public override bool IsSkillInCooldown()
        {
            return !IsAttackable;
        }
    }
}
