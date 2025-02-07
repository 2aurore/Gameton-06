using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    [System.Serializable]
    public class HeartData
    {
        public int characterId;
        public int currentHearts;
        public int maxHearts = 3;
        public int heartRechargeTime = 600; // 10분
        public string lastHeartTime;

        // 캐릭터 생성시 하트 데이터 생성자
        public HeartData(int characterId)
        {
            this.characterId = characterId;
            currentHearts = maxHearts;
        }
        // 하트 소모 시 변경할 객체 생성자자
        public void UseHeart()
        {
            if (currentHearts > 0)
            {
                currentHearts--;
                lastHeartTime = DateTime.UtcNow.ToString(); // 마지막 사용 시간 갱신
            }
            else
            {
                Debug.Log("하트가 부족합니다!");
            }
        }
    }
}
