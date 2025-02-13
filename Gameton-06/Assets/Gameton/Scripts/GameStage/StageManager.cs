using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace TON
{
    public class StageManager : SingletonBase<StageManager>
    {
        public List<StageClearData> stageClearDatas { get; private set; }

        private float stageStartTime; // 스테이지 시작 시간

        public void Initialize()
        {
            LoadStageClearData();
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

        private void SaveStageClearData()
        {
            Assert.IsTrue(JSONLoader.SaveUpdatedJsonToPersistentData(stageClearDatas, "stageClear"));
            Initialize();
        }

        public void StartStage()
        {
            stageStartTime = Time.time;
        }

        public int GetStarCount(float elapsedTime)
        {
            if (elapsedTime <= 180f)
                return 3;
            else if (elapsedTime <= 300f)
                return 2;
            else
                return 1;
        }

        public void StageClear(string characterId, string stageId)
        {
            float clearTime = Time.time - stageStartTime;
            int starCount = GetStarCount(clearTime);
            Debug.Log($"스테이지 클리어! 별 개수: {starCount}");

            // UI 업데이트, 데이터 저장 로직 추가
            StageClearData stageClearData = new StageClearData(characterId, stageId, clearTime, starCount);
            stageClearDatas.Add(stageClearData);
            SaveStageClearData();
        }

    }
}
