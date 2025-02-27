using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    [System.Serializable]
    public class ClearData
    {
        public string nickname;
        // 클리어한 웨이브 넘버
        public int wave;
        // 클리어에 소요된 시간
        public float playTime;
        // 획득 점수
        public int score;

        public ClearData()
        {
            nickname = "";
            wave = 0;
            playTime = 0f;
            score = 0;
        }

        public void UpdateClearData(string nickname, int wave, float playTime, int score)
        {
            this.nickname = nickname;
            this.wave = wave;
            this.playTime = playTime;
            this.score = score;
        }
    }
}
