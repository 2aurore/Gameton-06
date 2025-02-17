using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    [System.Serializable]
    public class HeartData
    {
        // 캐릭터 아이디
        public int characterId;
        // 현재 하트 개수
        public int currentHearts;
        public string lastHeartTime;

        // 캐릭터 생성시 하트 데이터 생성자
        public HeartData(int characterId)
        {
            this.characterId = characterId;
            currentHearts = 3;
            lastHeartTime = null;
        }

    }
}
