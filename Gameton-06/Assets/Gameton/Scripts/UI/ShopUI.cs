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
        
        // // 보유 포션 수량 
        // [SerializeField] private TextMeshProUGUI hpPotionCount;
        // [SerializeField] private TextMeshProUGUI mpPotionCount;
        
        // Start is called before the first frame update
        void Start()
        {
            InitPopUpActive();
        }
        
        public void InitPopUpActive()
        {
            HeartPopUp.SetActive(false);
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
    }
}
