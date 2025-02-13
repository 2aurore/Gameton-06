using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace TON
{
    public class PlayerDataManager : SingletonBase<PlayerDataManager>
    {
        // 사용자가 생성해둔 플레이어 데이터를 싱글톤으로 전역 사용하기 위함
        public List<PlayerData> playersData { get; private set; }

        public PlayerData player { get; private set; }

        [SerializeField]
        private int expVariable = 50; // 경험치 변수 (조정 가능)
        [SerializeField]
        private int attackGrowthFactor = 50; // 공격력 성장 변수 (조정 가능)
        [SerializeField]
        private int defensiveGrowthFactor = 200; // 방어력 성장 변수 (조정 가능)


        public void Initalize()
        {
            LoadPlayerData();
            // 현재 플레이어 1개만 사용하므로 0번째 인덱스의 플레이어 데이터를 사용
            PlayerPrefs.SetInt("SelectedPlayerIndex", 0);
        }

        private void LoadPlayerData()
        {
            if (playersData != null)
            {
                playersData.Clear();
            }

            JSONLoader.SaveJsonToPersistentData("player");
            playersData = JSONLoader.LoadJsonFromPersistentData<List<PlayerData>>("player");
            if (playersData == null)
            {
                playersData = new List<PlayerData>();
            }
        }

        // 공격력과 방어력 업데이트
        private void UpdateStats(int currentLevel)
        {
            player.attackPower *= 1 + (currentLevel - 1) / attackGrowthFactor;
            player.defensivePower *= 1 + (currentLevel - 1) / defensiveGrowthFactor;
        }

        // 현재 레벨에서 다음 레벨까지 필요한 경험치 계산
        private int GetRequiredExp(int currentLevel)
        {
            return (6 * currentLevel * currentLevel) + (currentLevel * expVariable);
        }

        public bool UpdateExpericence(int amount)
        {
            // 경험치 추가
            player.experience += amount;

            // 추가된 경험치로 인한 현재 경험치가 레벨업에 필요한 경험치보다 크거나 같다면 레벨업
            int requireLevelUpExp = GetRequiredExp(player.level);
            if (player.experience >= requireLevelUpExp)
            {
                // 레벨업 후 초과된 경험치를 반영하기 위해 다시 계산
                player.experience -= requireLevelUpExp;
                // 레벨업으로 인한 공격력/방어력 업데이트
                UpdateStats(player.level);
                // 레벨 증가
                player.level++;
                return true;
            }
            return false;
        }

        public void UpdatePlayerData()
        {
            int index = playersData.FindIndex(x => x.id == player.id);
            if (index > -1)
            {
                playersData[index] = player;
                Assert.IsTrue(JSONLoader.SaveUpdatedJsonToPersistentData(playersData, "player"));
                Initalize();
            }
        }

        public void SetCurrentUserData()
        {
            int characterId = PlayerPrefs.GetInt("SelectedPlayerIndex", -1);
            if (characterId > -1)
            {
                player = playersData[characterId];
                player.level = 15;
            }
            else
            {
                Debug.LogError("유효하지 않은 캐릭터 정보 입니다.");
            }
        }

    }
}
