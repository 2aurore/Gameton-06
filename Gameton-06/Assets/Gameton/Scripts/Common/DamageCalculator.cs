using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class DamageCalculator
    {
        public float criticalChance = 0.3f;  // 캐릭터 치명타 확률 (30%)
        public float criticalMultiplier = 1.5f;  // 치명타 배율 (1.5배)

        /// <summary>
        /// 일반 공격 데미지 계산
        /// </summary>
        public float CalculateBaseDamage(float baseAttack, float equipmentAttack, float defenseValue)
        {
            // 1. 총 공격력 계산
            float totalAttackPower = baseAttack + equipmentAttack;

            // 2. 데미지 변수 (0.9 ~ 1.1 랜덤 값)
            float damageMultiplier = Random.Range(0.9f, 1.1f);

            // 3. 방어력 계산 (1 - 방어력 값)
            float defenseEffect = 1 - defenseValue;

            // 4. 기본 데미지 계산
            float baseDamage = (totalAttackPower * damageMultiplier) * defenseEffect;

            return Mathf.Round(baseDamage); // 소수점 제거 (선택 사항)
        }

        /// <summary>
        /// 치명타 적용 (치명타 확률이 0보다 클 경우에만 적용)
        /// </summary>
        public float ApplyCriticalDamage(float damage, bool canCritical)
        {
            if (canCritical && Random.value < criticalChance) // Random.value는 0.0 ~ 1.0 사이의 랜덤 값
            {
                damage *= criticalMultiplier;
                Debug.Log("💥 치명타 발생! 💥");
            }

            return Mathf.Round(damage); // 소수점 제거
        }
    }
}
