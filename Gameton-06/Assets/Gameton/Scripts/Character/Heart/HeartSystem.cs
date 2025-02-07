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
        private HeartData heartData;

        public TextMeshProUGUI timerText;           // 하트 충전 타이머 UI


        private void Start()
        {
            UpdateHeartUI();  // 시작 시 UI 갱신
            InvokeRepeating(nameof(UpdateTimer), 0, 1f);  // 1초마다 업데이트
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
            if (HeartDataManager.Singleton.GetCurrentHearts() >= HeartDataManager.Singleton.GetMaxHearts())
            {
                timerText.text = "0:00";
                return;
            }

            int timeLeft = HeartDataManager.Singleton.GetRemainingRechargeTime();

            if (timeLeft <= 0)
            {
                HeartDataManager.Singleton.RechargeHearts();
                UpdateHeartUI();
                return;
            }

            timerText.text = $"{timeLeft / 60:D2}:{timeLeft % 60:D2}"; // mm:ss 포맷
        }
    }
}