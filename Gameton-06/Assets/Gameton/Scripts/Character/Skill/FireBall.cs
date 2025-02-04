using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class FireBall : PoolAble
    {
        private float elapsedTime; // 경과 시간 저장 변수
        public float destoryTime = 2f;

        public float attackPower;
        public float damage;
        public string id = "K0001";

        void OnEnable()
        {
            elapsedTime = 0f; // 오브젝트가 활성화될 때 초기화
            damage = SkillDataManager.Singleton.GetSkillData(id).damage;
            attackPower = PlayerDataManager.Singleton.player.attackPower;
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
                collision.GetComponent<IDamage>().ApplyDamage(damage * attackPower);
                ReleaseObject();
            }
        }
    }
}
