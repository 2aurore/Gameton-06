using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    [System.Serializable]
    public class Projectile : PoolAble
    {
        public PlayerData playerData;

        public float destoryTime = 2f;
        private float activatedTime = 0f; // 경과 시간 저장 변수

        private DamageCalculator damageCalculator = new DamageCalculator();
        private float damage;

        public void Init(float damage)
        {
            this.damage = damage;
            playerData = PlayerDataManager.Singleton.player;
        }

        void OnEnable()
        {
            activatedTime = Time.time;
        }

        private void Update()
        {
            if (Time.time - activatedTime >= destoryTime)
            {
                ReleaseObject();
            }
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
                float damage = damageCalculator.CalculateBaseDamage(playerData.attackPower * this.damage, 0, monsterDefencePower);

                // 치명타 적용
                damage = damageCalculator.ApplyCriticalDamage(damage);
                collision.GetComponent<IDamage>().ApplyDamage(damage);
                ReleaseObject();
            }
        }

    }
}

