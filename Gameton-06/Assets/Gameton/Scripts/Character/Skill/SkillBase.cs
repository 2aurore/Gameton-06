using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    [System.Serializable]
    public abstract class SkillBase
    {
        public float SkillCoolDown => SkillData.coolDown;
        public float CurrentCoolDown { get; protected set; }

        public SkillData SkillData { get; private set; }


        public void Init(SkillData skillData)
        {
            SkillData = skillData;
        }

        public void UpdateSkill(float deltaTime)
        {
            CurrentCoolDown -= deltaTime;
            CurrentCoolDown = Mathf.Max(0, CurrentCoolDown);
        }


        public abstract void ExecuteSkill(CharacterBase actor);
    }
}

