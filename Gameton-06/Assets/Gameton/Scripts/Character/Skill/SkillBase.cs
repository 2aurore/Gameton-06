using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    [System.Serializable]
    public abstract class SkillBase : PoolAble
    {
        public float SkillCoolDown => SkillData.coolDown;
        public float CurrentCoolDown { get; protected set; }

        public SkillData SkillData { get; private set; }
        public PlayerData playerData;

        private DamageCalculator damageCalculator = new DamageCalculator();

        private float elapsedTime; // 경과 시간 저장 변수
        public float destoryTime = 2f;


        public void Init(SkillData skillData)
        {
            SkillData = skillData;
            playerData = PlayerDataManager.Singleton.player;
        }

        void OnEnable()
        {
            elapsedTime = 0f; // 오브젝트가 활성화될 때 초기화
        }

        void Update()
        {
            UpdateSkill(Time.deltaTime);
            elapsedTime += Time.deltaTime; // 경과 시간 누적

            // 2초가 지나면 오브젝트 풀에 반환
            if (elapsedTime >= destoryTime)
            {
                ReleaseObject();
            }
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

