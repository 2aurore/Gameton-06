using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    public class MonsterDataManager : SingletonBase<MonsterDataManager>
    {
        // 전체 몬스터 데이터 리스트
        public List<MonsterData> monstersData { get; private set; }

        // 현재 선택된 몬스터 데이터
        public MonsterData currentMonster { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            LoadMonsterData();
        }

        private void LoadMonsterData()
        {
            monstersData = JSONLoader.LoadFromResources<List<MonsterData>>("Monster");
            if (monstersData == null)
            {
                monstersData = new List<MonsterData>();
                Debug.LogError("몬스터 데이터 로드 실패");
            }
        }

        public MonsterData GetMonsterData(int monsterId)
        {
            if (monsterId >= 0 && monsterId < monstersData.Count)
            {
                currentMonster = monstersData[monsterId];
                return currentMonster;
            }
            else
            {
                Debug.LogError($"유효하지 않은 몬스터 ID입니다: {monsterId}");
                return null;
            }
        }

        public List<MonsterData> GetAllMonsterData()
        {
            return monstersData;
        }
    }
}