using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class PlayerDataManager : SingletonBase<PlayerDataManager>
    {
        // 사용자가 생성해둔 플레이어 데이터를 싱글톤으로 전역 사용하기 위함
        public List<PlayerData> playersData { get; private set; }

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

    }
}
