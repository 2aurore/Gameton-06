using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace TON
{
    public class StageManager : SingletonBase<StageManager>
    {

        // 현재 플레이 시간을 초 단위로 반환하는 프로퍼티
        public float PlayTime => Time.time - stageStartTime;
        public int goldReward { get; private set; } = 0;  // 골드 획득 보상
        public int expReward { get; private set; } = 0;  // 경험치 획득 보상
        public int waveCount { get; private set; }   // 클리어한 웨이브 넘버
        public int gameScore { get; private set; } = 0;  // 몬스터 처치로 얻은 점수 보상

        private float stageStartTime; // 스테이지 시작 시간

        public ClearData TOP_RECORD { get; private set; }    // lobby 화면에 기록 세팅할때 사용할 변수
        public List<ClearData> RankList { get; private set; }    // 전체 랭킹 적용할 리스트

        private BackendClearDataManager clearDataManager;
        private BackendRankDataManager rankDataManager;

        public void Initialize()
        {
            clearDataManager = new BackendClearDataManager();
            rankDataManager = new BackendRankDataManager();

            GetRankData();
        }

        public void GetRankData()
        {
            // 서버에서 내 클리어 데이터를 가져오고 가장 기록이 높은 정보를 세팅
            rankDataManager.LoadMyRankData(rankData =>
            {
                TOP_RECORD.UpdateClearData(rankData.nickname, rankData.wave, rankData.playTime, rankData.score);
            });
        }

        // 스테이지 시작 시 시작 정보 저장
        public void StartStage()
        {
            stageStartTime = Time.time;
        }

        /// <summary>
        /// 게임 웨이브 진행 시 획득한 골드와 경험치 정보를 저장하게 함
        /// </summary>
        public void SetRewardData(int gold, int exp, int score)
        {
            goldReward += gold;
            expReward += exp;
            gameScore += score;
        }
        public void SetWaveData(int wave)
        {
            waveCount = wave;
        }

        /// <summary>
        /// 게임 플레이 종료 시 클리어 정보 저장 로직 수행
        /// </summary>
        public void StageClear()
        {
            float clearTime = PlayTime;

            ClearData clearData = new ClearData();
            clearData.UpdateClearData(PlayerDataManager.Singleton.player.name, waveCount, clearTime, gameScore);
            // clearData 저장
            clearDataManager.InsertInitData(clearData);

            // rankData 조회 후 비교 하여 저장
            rankDataManager.CheckUpdateRankData(clearData, () =>
            {
                TOP_RECORD.UpdateClearData(clearData.nickname, clearData.wave, clearData.playTime, clearData.score);
            });
        }


    }
}
