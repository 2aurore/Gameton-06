using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    [System.Serializable]
    public class SkillBase : PoolAble
    {
        public float SkillCoolDown => SkillData.coolDown;
        public float CurrentCoolDown { get; protected set; }

        public SkillData SkillData { get; private set; }
        public PlayerData playerData;

        private DamageCalculator damageCalculator = new DamageCalculator();

        private float elapsedTime = 0f; // 경과 시간 저장 변수
        public float destoryTime = 2f;


        public void Init(SkillData skillData)
        {
            SkillData = skillData;
            playerData = PlayerDataManager.Singleton.player;
        }

        void OnEnable()
        {
            elapsedTime = 0f;
        }

        public void InvokeExcuteSkill()
        {
            InvokeRepeating(nameof(ExecuteSkill), 0f, 1f); // 즉시 실행 후 1초 간격 반복
        }

        void ExecuteSkill()
        {
            if (elapsedTime >= destoryTime)
            {
                CancelInvoke(nameof(ExecuteSkill)); // 반복 중지
                ReleaseObject();
                return;
            }

            Debug.Log("SkillBase:: " + SkillData.name);
            UpdateSkill(Time.deltaTime);
            elapsedTime += 1.0f;
        }

        void OnDisable()
        {
            CancelInvoke(nameof(ExecuteSkill)); // 오브젝트 비활성화 시 중지
        }


        void Update()
        {


        }

        public void SetCurrentCoolDown()
        {
            CurrentCoolDown = SkillData.coolDown; // 쿨타임 시작
        }

        public void UpdateSkill(float deltaTime)
        {
            CurrentCoolDown -= deltaTime;
            CurrentCoolDown = Mathf.Max(0, CurrentCoolDown);
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Monster")) // 적과 충돌 시 제거
            {
                // 기본 데미지 계산
                // TODO: 장비 공격력 반영 필요
                // float damage = damageCalculator.CalculateBaseDamage(playerData.attackPower, playerData.equipmentAttack, playerData.defensivePower);

                // 몬스터 방어력
                float monsterDefencePower = collision.GetComponent<MonsterBase>().defencePower;
                float damage = damageCalculator.CalculateBaseDamage(playerData.attackPower * SkillData.damage, 0, monsterDefencePower);

                // 치명타 적용
                damage = damageCalculator.ApplyCriticalDamage(damage);
                collision.GetComponent<IDamage>().ApplyDamage(damage);
                ReleaseObject();
            }
        }

    }
}

