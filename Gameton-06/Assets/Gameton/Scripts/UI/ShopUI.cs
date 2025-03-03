using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class ShopUI : UIBase
    {
        // // 보유 포션 수량 
        // [SerializeField] private TextMeshProUGUI hpPotionCount;
        // [SerializeField] private TextMeshProUGUI mpPotionCount;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        private void OnEnable()
        {
            SetUserItemData();
        }
        
        private void SetUserItemData()
        {
            UserItemData userItem = PlayerDataManager.Singleton.userItem;

            // hpPotionCount.text = $"{userItem.hpPotion}";
            // mpPotionCount.text = $"{userItem.mpPotion}";
        }
        
        public void OnClickLobbyButton()
        {
            Main.Singleton.ChangeScene(SceneType.Lobby);
        }
    }
}
