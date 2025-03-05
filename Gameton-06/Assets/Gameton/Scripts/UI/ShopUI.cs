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
        
        [SerializeField] private int hpPotionPrice = 200; // HP 포션 가격
        [SerializeField] private int mpPotionPrice = 400; // MP 포션 가격
        
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
        public void OnClickBuyHpPotionButton()
        {
            if ((playerDataManager.goldAmount) >= hpPotionPrice)
            {
                // 골드 차감 및 HP 포션 증가
                playerDataManager.UseGold(hpPotionPrice);
                playerDataManager.userItem.hpPotion += 1;

                Debug.Log($"HP 포션 구매 성공! 남은 골드: {playerDataManager.goldAmount}, HP 포션 수량: {playerDataManager.userItem.hpPotion}");
            }
            else
            {
                Debug.Log("골드가 부족합니다!");
                // 골드 부족 팝업 표시 가능
                UIManager.Show<GoldPopup>(UIList.GoldPopup);
            }
        }

        // MP 포션 구매 버튼 클릭 시 호출
        public void OnClickBuyMpPotionButton()
        {
            if (playerDataManager.goldAmount >= mpPotionPrice)
            {
                // 골드 차감 및 MP 포션 증가
                playerDataManager.UseGold(mpPotionPrice);
                playerDataManager.userItem.mpPotion += 1;

                Debug.Log($"MP 포션 구매 성공! 남은 골드: {playerDataManager.goldAmount}, MP 포션 수량: {playerDataManager.userItem.mpPotion}");
            }
            else
            {
                Debug.Log("골드가 부족합니다!");
                // 골드 부족 팝업 표시 가능
                UIManager.Show<GoldPopup>(UIList.GoldPopup);
            }
        }
    }
}
