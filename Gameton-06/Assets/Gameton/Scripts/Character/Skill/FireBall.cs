using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class FireBall : PoolAble
    {

        private float elapsedTime; // 경과 시간 저장 변수

        void OnEnable()
        {
            elapsedTime = 0f; // 오브젝트가 활성화될 때 초기화
        }

        void Update()
        {
            elapsedTime += Time.deltaTime; // 경과 시간 누적

            // 2초가 지나면 오브젝트 풀에 반환
            if (elapsedTime >= 2f)
            {
                ReleaseObject();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Monster")) // 적과 충돌 시 제거
            {
                ReleaseObject();
            }
        }
    }
}
