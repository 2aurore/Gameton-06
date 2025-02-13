using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    [System.Serializable]
    public class StageClearData
    {
        // 스테이지 클리어 데이터의 고유 id
        public string id;
        // 클리어한 캐릭터 아이디
        public string characterId;
        // 클리어한 스테이지 아이디
        public string stageId;
        // 클리어에 소요된 시간
        public int clearTime;
        // 클리어 시간에 따른 별점
        public int starRating;
        // 클리어한 날짜와 시간 정보
        public string dateTime;

        public StageClearData(string characterId, string stageId, int clearTime, int starRating)
        {
            id = $"SC{DateTime.UtcNow:yyyyMMddHHmmss}-{characterId}-{stageId}";
            this.characterId = characterId;
            this.stageId = stageId;
            this.clearTime = clearTime;
            this.starRating = starRating;
            dateTime = DateTime.UtcNow.ToString();
        }
    }
}
