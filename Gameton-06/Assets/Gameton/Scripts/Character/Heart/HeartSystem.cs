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


        private void OnEnable()
        {
            UpdateHeartUI();  // 시작 시 UI 갱신
        }

        private void FixedUpdate()
        {
            UpdateTimer();
        }

        public void UpdateHeartUI()
        {
            int currentHearts = HeartDataManager.Singleton.GetCurrentHearts();

            for (int i = 0; i < heartImages.Count; i++)
            {
                heartImages[i].sprite = (i < currentHearts) ? fullHeartSprite : emptyHeartSprite;
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