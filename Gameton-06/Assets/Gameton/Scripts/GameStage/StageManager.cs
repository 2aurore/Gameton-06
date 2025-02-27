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
        // private string stageId; // 스테이지 아이디


        public void Initialize()
        {
            LoadStageClearData();
            SetStageClearDataList();
        }

        private void LoadStageClearData()
        {
#if UNITY_EDITOR
            JSONLoader.SaveJsonToPersistentData("stageClear");
#endif
            stageClearDatas = JSONLoader.LoadJsonFromPersistentData<List<StageClearData>>("stageClear");
            if (stageClearDatas == null)
            {
                stageClearDatas = new List<StageClearData>();
            }
        }

        private void SetStageClearDataList()
        {
            string characterId = PlayerDataManager.Singleton.player.id;

            foreach (var data in stageClearDatas)
            {
                if (data.characterId != characterId)
                {
                    continue;
                }

                // 만약 현재 stageId가 딕셔너리에 없거나, 기존 값보다 starRating이 높다면 업데이트
                if (!bestStageClearDict.ContainsKey(data.stageId) || bestStageClearDict[data.stageId].starRating < data.starRating)
                {
                    bestStageClearDict[data.stageId] = data;
                }
            }

            // 필터링된 결과를 새로운 리스트에 저장
            List<StageClearData> filteredList = new List<StageClearData>();
            foreach (var entry in bestStageClearDict)
            {
                filteredList.Add(entry.Value);
            }

            // 결과 출력
            foreach (var data in filteredList)
            {
                Debug.Log($"StageID: {data.stageId}, StarRating: {data.starRating}, ID: {data.id}");
            }
        }


        private void SaveStageClearData()
        {
            Assert.IsTrue(JSONLoader.SaveUpdatedJsonToPersistentData(stageClearDatas, "stageClear"));
            Initialize();
        }

        public void StartStage(string stageId)
        {
            stageStartTime = Time.time;
            // this.stageId = stageId;
        }

        // public int GetStarCount(float elapsedTime)
        // {
        //     if (elapsedTime <= 180f)
        //         return 3;
        //     else if (elapsedTime <= 300f)
        //         return 2;
        //     else
        //         return 1;
        // }

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
            // int starCount = GetStarCount(clearTime);

            // TODO: UI 업데이트, 데이터 저장 로직 추가
            // StageClearData stageClearData = new StageClearData(characterId, stageId, clearTime, starCount);
            // stageClearDatas.Add(stageClearData);
            SaveStageClearData();
        }

        // public void StageGameOver()
        // {
        //     string characterId = PlayerDataManager.Singleton.player.id;
        //     float clearTime = Time.time - stageStartTime;
        //     int starCount = -1;

        //     // UI 업데이트, 데이터 저장 로직 추가
        //     StageClearData stageClearData = new StageClearData(characterId, stageId, clearTime, starCount);
        //     stageClearDatas.Add(stageClearData);
        //     SaveStageClearData();
        // }

    }
}
