using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class DarknessBall : PoolAble
    {
        private float elapsedTime; // 경과 시간 저장 변수
        public float destoryTime = 2f;

        [SerializeField]
        private float damage;
        private string id = "K0003";

        private DamageCalculator damageCalculator = new DamageCalculator();
        private PlayerData playerData;


        void OnEnable()
        {
            elapsedTime = 0f; // 오브젝트가 활성화될 때 초기화
            damage = SkillDataManager.Singleton.GetSkillData(id).damage;
            playerData = PlayerDataManager.Singleton.player;
        }

        void Update()
        {
            elapsedTime += Time.deltaTime; // 경과 시간 누적

            // 2초가 지나면 오브젝트 풀에 반환
            if (elapsedTime >= destoryTime)
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
