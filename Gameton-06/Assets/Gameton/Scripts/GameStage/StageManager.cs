using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class StageManager : SingletonBase<StageManager>
    {

        private float stageStartTime; // 스테이지 시작 시간

        public void StartStage()
        {
            stageStartTime = Time.time;
        }

        public int GetStarCount()
        {
            float elapsedTime = Time.time - stageStartTime;

            if (elapsedTime <= 180f)
                return 3;
            else if (elapsedTime <= 300f)
                return 2;
            else
                return 1;
        }

        public void StageClear()
        {
            int starCount = GetStarCount();
            Debug.Log($"스테이지 클리어! 별 개수: {starCount}");
            // UI 업데이트, 데이터 저장 로직 추가 가능
        }

    }
}
