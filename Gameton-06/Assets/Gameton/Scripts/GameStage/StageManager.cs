using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace TON
{
    public class StageManager : SingletonBase<StageManager>
    {
        public List<StageClearData> stageClearDatas { get; private set; }
        [SerializeField]
        public SerializableDictionary<string, StageClearData> bestStageClearDict = new SerializableDictionary<string, StageClearData>();


        // 현재 플레이 시간을 초 단위로 반환하는 프로퍼티
        public float PlayTime => Time.time - stageStartTime;
        public int goldReward { get; private set; } = 0;  // 골드 획득 보상
        public int expReward { get; private set; } = 0;  // 경험치 획득 보상
        public int waveCount { get; private set; }   // 클리어한 웨이브 넘버
        public int gameScore { get; private set; } = 0;  // 몬스터 처치로 얻은 점수 보상

        private float stageStartTime; // 스테이지 시작 시간


        public void Initialize()
        {

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

        public void StageClear()
        {
            string characterId = PlayerPrefs.GetString("BackendCustomID", string.Empty);
            float clearTime = Time.time - stageStartTime;

            // TODO: UI 업데이트, 데이터 저장 로직 추가
            // StageClearData stageClearData = new StageClearData(characterId, stageId, clearTime, starCount);
            // stageClearDatas.Add(stageClearData);
            // SaveStageClearData();
        }


    }
}
