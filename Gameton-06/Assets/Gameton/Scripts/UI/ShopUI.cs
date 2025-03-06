using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TON
{
    public class ShopUI : UIBase
    {
        [SerializeField] private GameObject HeartPopUp;
        [SerializeField] private GameObject PositionPopUp;
        
        [SerializeField] private int hpPotionPrice1 = 200; // HP 포션 가격
        [SerializeField] private int hpPotionPrice5 = 1000; // HP 포션 가격
        [SerializeField] private int hpPotionPrice20 = 3600; // HP 포션 가격
        [SerializeField] private int mpPotionPrice1 = 400; // MP 포션 가격
        [SerializeField] private int mpPotionPrice5 = 2000; // MP 포션 가격
        [SerializeField] private int mpPotionPrice20 = 7600; // MP 포션 가격
        
        private PlayerDataManager playerDataManager;
        
        // Start is called before the first frame update
        void Start()
        {
            InitPopUpActive();

            // 싱글톤으로 PlayerDataManager 접근
            playerDataManager = PlayerDataManager.Singleton;

            if (playerDataManager == null)
            {
                Debug.LogError("PlayerDataManager가 초기화되지 않았습니다.");
            }
        }
        
        public void InitPopUpActive()
        {
            HeartPopUp.SetActive(false);
            PositionPopUp.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        
        
        public void OnClickLobbyButton()
        {
            Main.Singleton.ChangeScene(SceneType.Lobby);
        }

        public void OnClickHeartPopUpButton()
        {
            HeartPopUp.SetActive(true);
        }

        public void OnClickHeartCloseButton()
        {
            HeartPopUp.SetActive(false);
        }
        
        public void OnClickPositionPopUpButton()
        {
            PositionPopUp.SetActive(true);
        }

        public void OnClickPositionCloseButton()
        {
            PositionPopUp.SetActive(false);
        }
        
        // HP 포션 구매 버튼 클릭 시 호출
        public void OnClickBuyHpPotion1Button()
        {
            if ((playerDataManager.goldAmount) >= hpPotionPrice1)
            {
                // 골드 차감 및 HP 포션 증가
                playerDataManager.UseGold(hpPotionPrice1);
                playerDataManager.userItem.hpPotion += 1;

                // Debug.Log($"HP 포션 구매 성공! 남은 골드: {playerDataManager.goldAmount}, HP 포션 수량: {playerDataManager.userItem.hpPotion}");
            }
        }        // HP 포션 구매 버튼 클릭 시 호출
        public void OnClickBuyHpPotion5Button()
        {
            if ((playerDataManager.goldAmount) >= hpPotionPrice5)
            {
                // 골드 차감 및 HP 포션 증가
                playerDataManager.UseGold(hpPotionPrice5);
                playerDataManager.userItem.hpPotion += 5;

                // Debug.Log($"HP 포션 구매 성공! 남은 골드: {playerDataManager.goldAmount}, HP 포션 수량: {playerDataManager.userItem.hpPotion}");
            }
        }        // HP 포션 구매 버튼 클릭 시 호출
        public void OnClickBuyHpPotion20Button()
        {
            if ((playerDataManager.goldAmount) >= hpPotionPrice20)
            {
                // 골드 차감 및 HP 포션 증가
                playerDataManager.UseGold(hpPotionPrice20);
                playerDataManager.userItem.hpPotion += 20;

                // Debug.Log($"HP 포션 구매 성공! 남은 골드: {playerDataManager.goldAmount}, HP 포션 수량: {playerDataManager.userItem.hpPotion}");
            }
        }

        // MP 포션 구매 버튼 클릭 시 호출
        public void OnClickBuyMpPotion1Button()
        {
            if (playerDataManager.goldAmount >= mpPotionPrice1)
            {
                // 골드 차감 및 MP 포션 증가
                playerDataManager.UseGold(mpPotionPrice1);
                playerDataManager.userItem.mpPotion += 1;

                // Debug.Log($"MP 포션 구매 성공! 남은 골드: {playerDataManager.goldAmount}, MP 포션 수량: {playerDataManager.userItem.mpPotion}");
            }
        }        // MP 포션 구매 버튼 클릭 시 호출
        public void OnClickBuyMpPotion5Button()
        {
            if (playerDataManager.goldAmount >= mpPotionPrice5)
            {
                // 골드 차감 및 MP 포션 증가
                playerDataManager.UseGold(mpPotionPrice5);
                playerDataManager.userItem.mpPotion += 1;

                // Debug.Log($"MP 포션 구매 성공! 남은 골드: {playerDataManager.goldAmount}, MP 포션 수량: {playerDataManager.userItem.mpPotion}");
            }
        }        // MP 포션 구매 버튼 클릭 시 호출
        public void OnClickBuyMpPotion20Button()
        {
            if (playerDataManager.goldAmount >= mpPotionPrice20)
            {
                // 골드 차감 및 MP 포션 증가
                playerDataManager.UseGold(mpPotionPrice20);
                playerDataManager.userItem.mpPotion += 1;

                // Debug.Log($"MP 포션 구매 성공! 남은 골드: {playerDataManager.goldAmount}, MP 포션 수량: {playerDataManager.userItem.mpPotion}");
            }
        }
    }
}
