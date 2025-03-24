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
        private float damage;   // 스킬 데미지 계수
        private int maxHitCount = 1; // 최대 타격 가능한 몬스터 수
        private int currentHitCount = 0; // 현재까지 타격한 몬스터 수
        private HashSet<Collider2D> hitMonsters = new HashSet<Collider2D>(); // 이미 타격한 몬스터 추적


        public void Init(float damage, int maxHitCount)
        {
            this.damage = damage;
            this.maxHitCount = Mathf.Max(1, maxHitCount);   // 데이터 오류 보정을 위한 Max 적용
            playerData = PlayerDataManager.Singleton.player;

            ResetProjectile();
        }

        // 투사체 초기화 메서드 (투사체가 재사용될 때 호출)
        public void ResetProjectile()
        {
            currentHitCount = 0;
            hitMonsters.Clear();
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

                // 이미 타격한 몬스터인지 확인
                if (hitMonsters.Contains(collision))
                    return;

                MonsterBase monsterBase = collision.GetComponent<MonsterBase>();

                // 몬스터가 이전 피격으로 이미 죽은 경우우
                if (monsterBase.currentHP <= 0)
                    return;

                // 타격한 몬스터 목록에 추가
                hitMonsters.Add(collision);

                // 몬스터 방어력 의도값 계산
                float calcMonsterDefence = monsterBase.defencePower / (monsterBase.defencePower + monsterBase.defenceIntention);
                float damage = damageCalculator.CalculateBaseDamage(playerData.attackPower * this.damage, 0, calcMonsterDefence);

                // 치명타 적용
                damage = damageCalculator.ApplyCriticalDamage(damage, collision.transform.position);
                collision.GetComponent<IDamage>().ApplyDamage(damage);

                // 타격 카운트 증가
                currentHitCount++;

                // 최대 타격 수에 도달하면 투사체 제거
                if (currentHitCount >= maxHitCount)
                {
                    ReleaseObject();
                }
            }
        }

    }
}

