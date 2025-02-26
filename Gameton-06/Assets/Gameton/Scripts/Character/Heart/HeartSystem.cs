using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TON
{
    public class HeartSystem : MonoBehaviour
    {
        public List<Image> heartImages;  // 하트 UI 리스트
        public Sprite fullHeartSprite;   // 채워진 하트
        public Sprite emptyHeartSprite;  // 빈 하트
        public TextMeshProUGUI timerText;           // 하트 충전 타이머 UI
        public GameObject overHeartText;           // 하트 충전 타이머 UI

        private float lastUpdateTime;

        private void OnEnable()
        {
            lastUpdateTime = Time.realtimeSinceStartup;
            UpdateHeartUI();  // 시작 시 UI 갱신
        }

        private void Update()
        {
            // 실제 시간 기반으로 일정 간격마다 업데이트 (Time.Scale = 0 에 영향받지 않도록)
            float currentTime = Time.realtimeSinceStartup;
            float deltaTime = currentTime - lastUpdateTime;

            // 1초마다 타이머 업데이트 (더 짧은 간격으로 설정 가능)
            if (deltaTime >= 1.0f)
            {
                UpdateTimer();
                lastUpdateTime = currentTime;
            }
        }

        public void UpdateHeartUI()
        {
            int currentHearts = HeartDataManager.Singleton.GetCurrentHearts();

            for (int i = 0; i < heartImages.Count; i++)
            {
                heartImages[i].sprite = (i < currentHearts) ? fullHeartSprite : emptyHeartSprite;
            }

            if (currentHearts > HeartDataManager.Singleton.maxHearts)
            {
                overHeartText.SetActive(true);
                overHeartText.GetComponent<TextMeshProUGUI>().text = $"+ {currentHearts - HeartDataManager.Singleton.maxHearts}";
            }
            else
            {
                overHeartText.SetActive(false);
            }
        }

        private void UpdateTimer()
        {
            if (HeartDataManager.Singleton.GetCurrentHearts() >= HeartDataManager.Singleton.maxHearts)
            {
                timerText.text = "6:00";
                return;
            }

            int timeLeft = HeartDataManager.Singleton.GetRemainingRechargeTime();

            if (timeLeft <= 0)
            {
                HeartDataManager.Singleton.RechargeHearts();
                UpdateHeartUI();
                return;
            }

            timerText.text = $"{timeLeft / 60}:{timeLeft % 60:D2}"; // m:ss 포맷
        }
    }
}