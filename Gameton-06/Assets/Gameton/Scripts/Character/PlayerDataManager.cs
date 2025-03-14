using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace TON
{
    public class PlayerDataManager : SingletonBase<PlayerDataManager>
    {
        // 사용자가 생성해둔 플레이어 데이터를 싱글톤으로 전역 사용하기 위함
        public PlayerData player { get; private set; }
        public int goldAmount { get; private set; }
        public int fishAmount { get; private set; }
        public UserItemData userItem { get; private set; } = new UserItemData();

        public float defensiveIntention { get; private set; } = 50f; // 방어력 변수 (조정 가능)
        public int requireLevelUpExp; // 경험치 변수 (조정 가능)

        [SerializeField] private int expVariable = 50; // 경험치 변수 (조정 가능)
        [SerializeField] private float attackGrowthFactor = 50f; // 공격력 성장 변수 (조정 가능)

        private BackendPlayerDataManager playerDataManager;
        private BackendCashDataManager cashDataManager;
        private BackendItemDataManager itemDataManager;

        private void OnEnable()
        {
            playerDataManager = new BackendPlayerDataManager();
            cashDataManager = new BackendCashDataManager();
            itemDataManager = new BackendItemDataManager();
        }

        public void bootstrapper()
        {
            player = new PlayerData();
        }

        public void Initalize()
        {
            LoadPlayerData();
            LoadPlayerCashData();
            LoadPlayerItemData();
        }

        private void LoadPlayerData()
        {
            // * 사용자 정보 서버에서 조회
            playerDataManager.LoadMyPlayerData(player =>
            {
                SetPlayerData(player);
            });
        }

        private void LoadPlayerCashData()
        {
            cashDataManager.LoadMyCashData(cashData =>
            {
                // 데이터 로드 완료 후 실행될 코드
                goldAmount = cashData.gold;
                fishAmount = cashData.fish;
                Debug.Log($"로드된 골드: {cashData.gold}, 물고기: {cashData.fish}");
            });
        }

        private void LoadPlayerItemData()
        {
            itemDataManager.LoadMyItemData(userItemData =>
            {
                userItem.hpPotion = userItemData.hpPotion;
                userItem.mpPotion = userItemData.mpPotion;
                Debug.Log($"로드된 hp포션: {userItemData.hpPotion}, mp포션: {userItemData.mpPotion}");
            });
        }

        public void UsePotion(string type)
        {
            switch (type)
            {
                case "HP":
                    userItem.hpPotion -= 1;
                    itemDataManager.UpdateHpData(userItem.hpPotion);
                    break;
                case "MP":
                    userItem.mpPotion -= 1;
                    itemDataManager.UpdateMpData(userItem.mpPotion);
                    break;
            }
        }

        public void AddPotion(string type, int amount)
        {
            switch (type)
            {
                case "HP":
                    userItem.hpPotion += amount;
                    itemDataManager.UpdateHpData(userItem.hpPotion);
                    break;
                case "MP":
                    userItem.mpPotion += amount;
                    itemDataManager.UpdateMpData(userItem.mpPotion);
                    break;
            }
        }

        public void AddGold(int amount)
        {
            goldAmount += amount;
            cashDataManager.UpdateGoldData(goldAmount, updatedData =>
            {
                // TODO: UI 업데이트 로직 적용
                // UpdateUI();
            });
        }
        public void UseGold(int amount, System.Action<bool> callback = null)
        {
            if (goldAmount - amount < 0)
            {
                // 골드 재화 사용 불가 팝업
                callback?.Invoke(false);
                UIManager.Show<GoldPopup>(UIList.GoldPopup);
                return;
            }

            goldAmount -= amount;
            cashDataManager.UpdateGoldData(goldAmount, updatedData =>
            {
                callback?.Invoke(true);
            });
        }

        public void AddFish(int amount)
        {
            fishAmount += amount;
            cashDataManager.UpdateFishData(fishAmount, updatedData =>
            {
                // TODO: UI 업데이트 로직 적용
                // UpdateUI();
            });
        }
        public void UseFish(int amount, System.Action<bool> callback)
        {
            if (fishAmount - amount < 0)
            {
                callback?.Invoke(false);
                return;
            }

            fishAmount -= amount;
            cashDataManager.UpdateFishData(fishAmount, updatedData =>
            {
                callback?.Invoke(true);
            });
        }

        // 공격력과 방어력 업데이트
        private void UpdateStats(int level)
        {
            player.attackPower = player.attackPower * (1 + level / attackGrowthFactor);
            player.defensivePower = player.defensivePower * (1 + level / defensiveIntention);
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
            requireLevelUpExp = GetRequiredExp(player.level);
            if (player.experience >= requireLevelUpExp)
            {
                // 레벨업 후 초과된 경험치를 반영하기 위해 다시 계산
                player.experience -= requireLevelUpExp;
                // 레벨 증가
                player.level++;
                // 레벨업으로 인한 공격력/방어력 업데이트
                UpdateStats(player.level);
                return true;
            }
            return false;
        }

        public void UpdatePlayerData()
        {
            // 캐릭터 데이터 서버 업데이트
            playerDataManager.UpdatePlayerData(player, isSuccess =>
            {
                if (isSuccess)
                {
                    Initalize();
                }
            });
        }

        public void SetPlayerData(PlayerData playerData)
        {
            player = playerData;

            // 사용자 정보가 있을때만 레벨업 필요 초기값 세팅
            if (playerData != null)
            {
                requireLevelUpExp = GetRequiredExp(player.level);
            }
        }

        // 플레이어가 사망했을때 호출
        public void PlayerDeadEvent()
        {
            Invoke(nameof(ShowGameEndUI), 0.5f);

        }
        private void ShowGameEndUI()
        {
            UIManager.Show<GameWinUI>(UIList.GameWinUI);
        }

        public void CreateNewPlayer(PlayerData player, System.Action<bool> onComplete = null)
        {
            playerDataManager.CreateNewPlayer(player, isSuccess =>
            {
                onComplete?.Invoke(isSuccess);
            });
        }
    }
}
