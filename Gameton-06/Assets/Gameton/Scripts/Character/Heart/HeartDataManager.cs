using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class HeartDataManager : SingletonBase<HeartDataManager>
    {
        public List<HeartData> heartDatas { get; private set; }
        public HeartData currentHeartData { get; private set; }
        private int characterId;

        public int GetCurrentHearts() => currentHeartData.currentHearts;
        public int GetMaxHearts() => currentHeartData.maxHearts;
        public int GetHeartRechargeTime() => currentHeartData.heartRechargeTime;
        public DateTime GetLastHeartTime() => DateTime.Parse(currentHeartData.lastHeartTime);


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

        // 캐릭터 생성 시 하트 초기값 생성
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

        // 하트 데이터 저장
        public void SaveHeartData()
        {
            heartDatas[characterId] = currentHeartData;
            JSONLoader.SaveToFile(heartDatas, "heart");
        }

        // 게임이 다시 실행될때 마지막 하트 소모 시간과 현재 시간을 계산해서 하트 충전량을 반영
        public void RechargeHearts()
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

        // 하트 소모 시 데이터 및 화면 UI를 업데이트트
        public void UseHeart()
        {
            if (currentHeartData.currentHearts > 0)
            {
                currentHeartData.currentHearts--;
                currentHeartData.lastHeartTime = DateTime.Now.ToString();
                SaveHeartData();

                FindObjectOfType<HeartSystem>().UpdateHeartUI(); // UI 업데이트
            }
        }

        // 데이터 파일에 저장된 마지막 하트 사용시간과 동기화화 
        public int GetRemainingRechargeTime()
        {
            if (currentHeartData.currentHearts >= currentHeartData.maxHearts) return 0;

            DateTime lastTime = DateTime.Parse(currentHeartData.lastHeartTime);
            TimeSpan timePassed = DateTime.Now - lastTime;
            int timeLeft = currentHeartData.heartRechargeTime - (int)timePassed.TotalSeconds;

            return Mathf.Max(0, timeLeft);
        }

    }
}
