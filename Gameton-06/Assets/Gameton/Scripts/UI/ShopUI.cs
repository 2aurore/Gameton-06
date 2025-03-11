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
        
        void Start()
        {
            // 싱글톤으로 PlayerDataManager 접근
            playerDataManager = PlayerDataManager.Singleton;

            if (playerDataManager == null)
            {
                Debug.LogError("PlayerDataManager가 초기화되지 않았습니다.");
            }
        }
        
        public void OnClickLobbyButton()
        {
            Main.Singleton.ChangeScene(SceneType.Lobby);
        }
        
        // 포션 구매 메서드
        private void BuyPotion(int price, string potionType, int quantity)
        {
            if (playerDataManager.goldAmount >= price)
            {
                playerDataManager.UseGold(price, (isSuccess) =>
                {
                    if (isSuccess)
                    {
                        if (potionType == "hp")
                        {
                            playerDataManager.userItem.hpPotion += quantity;
                        }
                        else if (potionType == "mp")
                        {
                            playerDataManager.userItem.mpPotion += quantity;
                        }
                        
                        UIManager.Singleton.UpdateCashData();
                    }
                });
            }
        }
        
        // HP 포션 구매 버튼 클릭 시 호출
        public void OnClickBuyHpPotion1Button()
        {
            BuyPotion(hpPotionPrice1, "hp", 1);
        }

        public void OnClickBuyHpPotion5Button()
        {
            BuyPotion(hpPotionPrice5, "hp", 5);
        }

        public void OnClickBuyHpPotion20Button()
        {
            BuyPotion(hpPotionPrice20, "hp", 20);
        }

        // MP 포션 구매 버튼 클릭 시 호출
        public void OnClickBuyMpPotion1Button()
        {
            BuyPotion(mpPotionPrice1, "mp", 1);
        }

        public void OnClickBuyMpPotion5Button()
        {
            BuyPotion(mpPotionPrice5, "mp", 5);
        }

        public void OnClickBuyMpPotion20Button()
        {
            BuyPotion(mpPotionPrice20, "mp", 20);
        }
    }
}
