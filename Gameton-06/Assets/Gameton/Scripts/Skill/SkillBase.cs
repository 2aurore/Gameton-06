using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    [System.Serializable]
    public class SkillBase
    {
        public SkillBase(SkillData skillData)
        {
            SkillData = skillData;
            CurrentCoolDown = 0f;
        }
        public float SkillCoolDown => SkillData.coolDown;
        [field: SerializeField] public SkillData SkillData { get; private set; }
        [field: SerializeField] public float CurrentCoolDown { get; protected set; }


        public System.Action OnSkillExecuted;
        public event System.Action OnCooldownCompleted;

        public void SetCurrentCoolDown()
        {
            CurrentCoolDown = SkillData.coolDown; // 쿨타임 시작
        }

        public void UpdateSkill(float deltaTime)
        {
            float before = CurrentCoolDown;
            CurrentCoolDown -= deltaTime;
            CurrentCoolDown = Mathf.Max(0, CurrentCoolDown);

            if (before > 0f && CurrentCoolDown <= 0f)
            {
                OnCooldownCompleted?.Invoke();
            }
        }


    }
}
