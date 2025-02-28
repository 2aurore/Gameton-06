using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace TON
{
    public class HeartDataManager : SingletonBase<HeartDataManager>
    {
        public List<HeartData> heartDatas { get; private set; }
        public HeartData currentHeartData { get; private set; }
        private int characterId;

        public int maxHearts = 5;
        public int heartRechargeTime = 360;     // 하트 충전 시간 6분

        public int GetCurrentHearts() => currentHeartData.currentHearts;
        public DateTime GetLastHeartTime() => DateTime.Parse(currentHeartData.lastHeartTime);


        protected override void Awake()
        {
            base.Awake();
            LoadHeartData();
        }

        private void LoadHeartData()
        {
            if (heartDatas != null)
            {
                heartDatas.Clear();
            }

            JSONLoader.SaveJsonToPersistentData("heart");
            heartDatas = JSONLoader.LoadJsonFromPersistentData<List<HeartData>>("Heart");
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
            Assert.IsTrue(JSONLoader.SaveUpdatedJsonToPersistentData(heartDatas, "heart"));
            LoadHeartData();
        }

        public void SetCurrentUserHeart()
        {
            characterId = PlayerPrefs.GetInt("SelectedPlayerIndex", -1);
            if (characterId > -1 && heartDatas.Count > 0)
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
            Assert.IsTrue(JSONLoader.SaveUpdatedJsonToPersistentData(heartDatas, "heart"));
            LoadHeartData();
        }

        // 게임이 다시 실행될때 마지막 하트 소모 시간과 현재 시간을 계산해서 하트 충전량을 반영
        public void RechargeHearts()
        {
            if (currentHeartData.currentHearts >= maxHearts) return;

            DateTime lastTime;
            bool hasValidTime = DateTime.TryParse(currentHeartData.lastHeartTime, out lastTime);

            if (!hasValidTime)
            {
                lastTime = DateTime.Now; // 기본값 설정
                currentHeartData.lastHeartTime = lastTime.ToString();
            }

            TimeSpan timePassed = DateTime.Now - lastTime;
            int heartsToRecover = (int)(timePassed.TotalSeconds / heartRechargeTime);

            if (heartsToRecover > 0)
            {
                int newHearts = Mathf.Min(currentHeartData.currentHearts + heartsToRecover, maxHearts);

                // 충전된 하트 수만큼 lastHeartTime을 재조정
                currentHeartData.currentHearts = newHearts;

                // 남은 충전 시간 유지: 충전된 만큼 lastHeartTime을 앞으로 이동
                currentHeartData.lastHeartTime = lastTime.AddSeconds(heartsToRecover * heartRechargeTime).ToString();
                SaveHeartData();
            }
        }

        // 하트 소모 시 데이터 및 화면 UI를 업데이트
        public void UseHeart()
        {
            if (currentHeartData.currentHearts > 0)
            {
                currentHeartData.currentHearts--;
                // 하트를 사용한 후 하트가 최대 개수보다 적을 때만 타이머 시작/업데이트
                if (currentHeartData.currentHearts < 5)
                {
                    // lastHeartTime이 비어있거나 잘못된 경우를 대비하여 기본값 설정
                    DateTime lastTime;
                    bool hasValidTime = DateTime.TryParse(currentHeartData.lastHeartTime, out lastTime);

                    TimeSpan timePassed = DateTime.Now - lastTime;

                    // 재충전 시간이 지났거나, 비어있거나, 하트가 최대치였다가 감소했을 때 lastHeartTime 갱신
                    if (!hasValidTime || timePassed.TotalSeconds >= heartRechargeTime || string.IsNullOrEmpty(currentHeartData.lastHeartTime))
                    {
                        currentHeartData.lastHeartTime = DateTime.Now.ToString();
                    }
                }

                SaveHeartData();
                UpdateHeartSystem();
            }
        }

        public static void UpdateHeartSystem()
        {
            HeartSystem heartSystem = FindObjectOfType<HeartSystem>();
            if (heartSystem != null)
            {
                heartSystem.UpdateHeartUI(); // UI 업데이트

            }
        }

        // 사용자의 광고 시청, 생선 소모 등으로 하트 충전하는 경우 호출출
        public void AddHeart(int amount)
        {
            int previousHearts = currentHeartData.currentHearts;
            currentHeartData.currentHearts += amount;

            // 하트가 5개 미만에서 5개 이상으로 변경되었을 때 타이머 초기화
            if (previousHearts < 5 && currentHeartData.currentHearts >= 5)
            {
                // 하트가 가득 찼으므로 lastHeartTime을 빈 문자열이나 특정 값으로 설정하여 타이머 중지
                currentHeartData.lastHeartTime = "";
            }

            SaveHeartData();
            UpdateHeartSystem();
        }

        // 데이터 파일에 저장된 마지막 하트 사용시간과 동기화
        public int GetRemainingRechargeTime()
        {
            if (currentHeartData.currentHearts >= maxHearts) return 0;

            DateTime lastTime;
            bool hasValidTime = DateTime.TryParse(currentHeartData.lastHeartTime, out lastTime);

            if (!hasValidTime)
            {
                lastTime = DateTime.Now; // 기본값 설정
                currentHeartData.lastHeartTime = lastTime.ToString();
            }

            TimeSpan timePassed = DateTime.Now - lastTime;
            int timeLeft = heartRechargeTime - (int)timePassed.TotalSeconds;

            return Mathf.Max(0, timeLeft);
        }

    }
}
