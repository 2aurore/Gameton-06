using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class HeartDataManager : SingletonBase<HeartDataManager>
    {
        public List<HeartData> heartDatas { get; private set; }

        [SerializeField]
        private HeartData currentHeartData;
        private int characterId;

        protected override void Awake()
        {
            base.Awake();
            LoadHeartData();
        }

        private void LoadHeartData()
        {
            heartDatas = JSONLoader.LoadFromResources<List<HeartData>>("Heart");
            if (heartDatas == null)
            {
                heartDatas = new List<HeartData>();
            }
        }


        public void CreateNewHeartSystem(int characterId)
        {
            HeartData heartData = new HeartData(characterId);
            heartDatas.Add(heartData);
            JSONLoader.SaveToFile(heartDatas, "heart");
            Debug.Log($"heartData test:: {heartData.currentHearts}");
        }

        public void SetCurrentUserHeart()
        {
            characterId = PlayerPrefs.GetInt("SelectedPlayerIndex", -1);
            if (characterId > -1)
            {
                currentHeartData = heartDatas[characterId];
                if (currentHeartData != null)
                {
                    RechargeHearts();
                }
                else
                {
                    Debug.Log("하트 정보 불러오기 중 오류 발생 ::: 초기값으로 재정의 합니다.");
                    CreateNewHeartSystem(characterId);
                }
            }
            else
            {
                Debug.LogError("유효하지 않은 캐릭터 정보입니다.");
            }

        }

        public void SaveHeartData()
        {
            heartDatas[characterId] = currentHeartData;
            JSONLoader.SaveToFile(heartDatas, "heart");
        }

        private void RechargeHearts()
        {
            if (currentHeartData.currentHearts >= currentHeartData.maxHearts) return;

            DateTime lastTime = DateTime.Parse(currentHeartData.lastHeartTime);
            TimeSpan timePassed = DateTime.Now - lastTime;
            int heartsToRecover = (int)(timePassed.TotalSeconds / currentHeartData.heartRechargeTime);

            if (heartsToRecover > 0)
            {
                currentHeartData.currentHearts = Mathf.Min(currentHeartData.currentHearts + heartsToRecover, currentHeartData.maxHearts);
                currentHeartData.lastHeartTime = DateTime.Now.ToString();
                SaveHeartData();
            }
        }

        public int GetCurrentHearts() => currentHeartData.currentHearts;

    }
}
