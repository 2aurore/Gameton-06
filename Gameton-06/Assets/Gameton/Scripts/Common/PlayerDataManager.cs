using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class PlayerDataManager : SingletonBase<PlayerDataManager>
    {
        // 사용자가 생성해둔 플레이어 데이터를 싱글톤으로 전역 사용하기 위함
        public List<PlayerData> playersData { get; private set; }

        public PlayerData player { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            LoadPlayerData();
        }

        private void LoadPlayerData()
        {
            playersData = JSONLoader.LoadFromResources<List<PlayerData>>("Player");
            if (playersData == null)
            {
                playersData = new List<PlayerData>();
            }
        }

        public void SetCurrentUserData()
        {
            int characterId = PlayerPrefs.GetInt("SelectedPlayerIndex", -1);
            if (characterId > -1)
            {
                player = playersData[characterId];
            }
            else
            {
                Debug.LogError("유효하지 않은 캐릭터 정보 입니다.");
            }
        }

    }
}
