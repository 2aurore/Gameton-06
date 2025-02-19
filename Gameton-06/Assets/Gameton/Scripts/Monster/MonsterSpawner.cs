using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class MonsterSpawner : MonoBehaviour
    {
        [SerializeField]
        private List<Transform> _monsterLocations = new List<Transform>();
        [SerializeField]
        private List<int> _monsterId = new List<int>();
        [SerializeField] 
        private float _spawnDistance = 10f;

        private CharacterBase _player;
        private List<MonsterBase> _spawnedMonsters = new List<MonsterBase>();
        private Dictionary<Transform, bool> _spawnPoints = new Dictionary<Transform, bool>();
        
        private void Start()
        {
            InitializeSpawner();
        }

        private void InitializeSpawner()
        {
            // 플레이어 찾기
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                _player = playerObj.GetComponent<CharacterBase>();
                Debug.Log("플레이어 찾음: " + playerObj.name);
            }
            
            // 스폰 포인트 초기화
            foreach (var location in _monsterLocations)
            {
                if (location != null)
                {
                    _spawnPoints[location] = false;
                    Debug.Log($"스폰 포인트 초기화: {location.name}");
                }
            }
        }

        private void Update()
        {
            if (_player == null) return;

            CheckSpawnConditions();
        }

        private void CheckSpawnConditions()
        {
            for (int i = 0; i < _monsterLocations.Count; i++)
            {
                Transform spawnPoint = _monsterLocations[i];
                if (spawnPoint == null || _spawnPoints[spawnPoint]) continue;

                float distance = Vector2.Distance(_player.transform.position, spawnPoint.position);
                if (distance <= _spawnDistance)
                {
                    SpawnMonster(i);
                    _spawnPoints[spawnPoint] = true;
                }
            }
        }

        private void SpawnMonster(int index)
        {
            if (index >= _monsterId.Count)
            {
                Debug.LogError($"유효하지 않은 인덱스: {index}");
                return;
            }

            try
            {
                int monsterId = _monsterId[index];
                
                // MonsterDataManager에서 데이터 가져오기
                MonsterData monsterData = MonsterDataManager.Singleton.GetMonsterData(monsterId);
                if (monsterData == null)
                {
                    Debug.LogError($"몬스터 ID {monsterId}에 대한 데이터를 찾을 수 없습니다.");
                    return;
                }

                // 몬스터 프리팹 경로 결정
                string prefabPath = GetMonsterPrefabPath(monsterData);
                
                // 프리팹 로드
                MonsterBase monsterPrefab = Resources.Load<MonsterBase>(prefabPath);
                if (monsterPrefab == null)
                {
                    Debug.LogError($"몬스터 프리팹을 찾을 수 없습니다: {prefabPath}");
                    return;
                }

                // 몬스터 생성
                Vector3 spawnPosition = _monsterLocations[index].position;
                MonsterBase newMonster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
                
                // 몬스터 초기화
                newMonster.id = monsterId;
                _spawnedMonsters.Add(newMonster);
                
                Debug.Log($"몬스터 생성 완료: ID {monsterId}, 위치 {spawnPosition}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"몬스터 생성 중 오류 발생: {e.Message}");
            }
        }

        private string GetMonsterPrefabPath(MonsterData monsterData)
        {
            // 몬스터 데이터에 따른 프리팹 경로 결정
            string basePath = "MonsterPrefabs/";
            
            // 일반 몬스터인 경우
            if (monsterData.id == 6)
            {
                return basePath + "Nomal/BlackTroll";
            }
            
            // 기타 몬스터들
            Dictionary<int, string> monsterNames = new Dictionary<int, string>
            {
                { 1, "BlueDragon" },
                { 2, "BrownWerewolf" },
                { 3, "GreenDragon" },
                { 4, "PurpleOgre" },
                { 5, "RedWerewolf" }
            };

            if (monsterNames.TryGetValue(monsterData.id, out string monsterName))
            {
                return basePath + monsterName;
            }

            throw new System.Exception($"알 수 없는 몬스터 ID: {monsterData.id}");
        }

        public void OnMonsterDestroyed(MonsterBase monster)
        {
            if (_spawnedMonsters.Contains(monster))
            {
                _spawnedMonsters.Remove(monster);
            }
        }
    }
}
