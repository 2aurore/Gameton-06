using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class AttackPattern
    {
        protected MonsterBase _monsterBase;


        public AttackPattern(MonsterBase monsterBase)
        {
            _monsterBase = monsterBase;
        }

        public virtual void Attack()
        {
        }
    }

    public class Monster1AttackPattern : AttackPattern 
    {
        public Monster1AttackPattern(MonsterBase monsterBase) : base(monsterBase)
        {
        }

        public override void Attack()
        {

            Skill1();

            MeleeAttack();
            
        }

        private void Skill1()
        {
            _monsterBase.MonsterSkillLaunch();
        }

        private void MeleeAttack()
        {
            _monsterBase.PlayerAttack();
        }
    }

    public class Monster2AttackPattern : AttackPattern
    {
        
    
        public Monster2AttackPattern(MonsterBase monsterBase) : base(monsterBase)
        {
            
        }

        public override void Attack()
        {

            Skill1();
            
            Skill2();
            
            MeleeAttack();

        }
    
        private void Skill1()
        {
            
        }
    
        private void Skill2()
        {
            
        }

        private void MeleeAttack()
        {
            _monsterBase.PlayerAttack();
        }
    }
}
