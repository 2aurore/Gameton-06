using System.Collections;
using System.Collections.Generic;
using Assets.PixelFantasy.PixelMonsters.Common.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TON
{
    public class MonsterSpawner : MonoBehaviour
    {
        private MonsterBase _monsterBase;
        public Transform[] spawnPoints; // 스폰 위치 배열
        
        private List<GameObject> monsterPool; // 몬스터 오브젝트 풀
        private List<int> availableSpawnPoints; // 스폰 가능한 위치 인덱스 리스트
        
        public int currentWave = 0;
        private const int TOTAL_WAVES = 10;
        private const int NORMAL_MONSTER_COUNT = 6;
    
        private float initialDelay = 5f; // 게임 시작 후 첫 웨이브 시작까지의 대기 시간
        private bool gameStarted = false;
        
        private float nextWaveDelay = 5f; // 다음 웨이브 시작 전 대기 시간
        private bool isWaitingForNextWave = false;
        
        [System.Serializable]
        public class WaveData
        {
            public GameObject monsterPrefabA; // 첫 번째 일반 몬스터 프리팹
            public GameObject monsterPrefabB; // 두 번째 일반 몬스터 프리팹
            public GameObject bossPrefab;     // 보스 몬스터 프리팹
        }
    
        public WaveData[] waveDataArray; // 크기 4로 설정 (1-3웨이브, 4-6웨이브, 7-9웨이브, 10웨이브)
        
        private List<GameObject> activeMonsters; // 현재 활성화된 몬스터 리스트
        
        [SerializeField]
        public TextMeshProUGUI waveCounter;
        
        // Start is called before the first frame update
        void Start()
        {
            monsterPool = new List<GameObject>();
            availableSpawnPoints = new List<int>();
            activeMonsters = new List<GameObject>();
        
            // 5초 후에 첫 웨이브 시작
            StartCoroutine(StartGameWithDelay());
        }
        
        private IEnumerator StartGameWithDelay()
        {
            // 초기 카운트다운 표시
            float timer = initialDelay;
            while (timer > 0)
            {
                if (waveCounter != null)
                {
                    waveCounter.text = Mathf.CeilToInt(timer).ToString();
                }
                timer -= Time.deltaTime;
                yield return null;
            }
        
            if (waveCounter != null)
            {
                waveCounter.text = null;
            }
        
            gameStarted = true;
            StartNextWave();
        }
        
        // Update is called once per frame
        void Update()
        {
            // 게임이 시작되지 않았다면 업데이트 건너뛰기
            if (!gameStarted) return;
             
            // 활성화된 몬스터 리스트에서 파괴된 몬스터 제거
            activeMonsters.RemoveAll(monster => monster == null);
        
            // 모든 몬스터가 죽었는지 확인하고 다음 웨이브 준비
            if (activeMonsters.Count == 0 && currentWave > 0 && !isWaitingForNextWave)
            {
                isWaitingForNextWave = true;
                StartCoroutine(StartNextWaveWithDelay());
            }
        }
        
        private void SpawnBossMonster()
        {
            // 랜덤한 스폰 포인트 선택
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            GameObject bossPrefab = GetBossPrefabForWave(currentWave);
        
            GameObject boss = Instantiate(bossPrefab, spawnPoints[spawnPointIndex].position, Quaternion.identity);
            monsterPool.Add(boss);
            activeMonsters.Add(boss);
        
            // 보스 웨이브에서는 자동으로 다음 웨이브로 넘어가지 않음
            // 보스가 죽으면 Update에서 체크하여 다음 웨이브로 넘어감
        }
        
        private void StartNextWave()
        {
            StageManager.Singleton.SetWaveData(currentWave);    // 웨이브 정보 전달.
            
            currentWave++;
            
            if (currentWave > TOTAL_WAVES)
            {
                // Debug.Log("모든 웨이브 완료!");
                return;
            }

            // 스폰 포인트 초기화
            availableSpawnPoints.Clear();
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                availableSpawnPoints.Add(i);
            }

            // 현재 웨이브에 따른 몬스터 스폰
            if (IsBossWave(currentWave))
            {
                SpawnBossMonster();
            }
            else
            {
                SpawnNormalMonsters();
            }
        }
        
        private bool IsBossWave(int wave)
        {
            return wave == 3 || wave == 6 || wave == 9 || wave == 10;
        }
        
        private GameObject GetBossPrefabForWave(int wave)
        {
            switch (wave)
            {
                case 3: return waveDataArray[0].bossPrefab;  // 첫 번째 보스
                case 6: return waveDataArray[1].bossPrefab;  // 두 번째 보스
                case 9: return waveDataArray[2].bossPrefab;  // 세 번째 보스
                case 10: return waveDataArray[3].bossPrefab; // 최종 보스
                default: return null;
            }
        }
        
        private void SpawnNormalMonsters()
        {
            if (currentWave == 7 || currentWave == 8)
            {
                for (int i = 0; i < NORMAL_MONSTER_COUNT + 2; i++)
                {
                    foreach (Transform spawnPoint in spawnPoints)
                    {
                        GameObject monsterPrefab = GetNormalMonsterPrefab();
                        Vector3 spawnPosition = spawnPoint.position;
            
                        GameObject monster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
                        monster.transform.parent = transform;
            
                        // MonsterBase 컴포넌트 가져오기
                        MonsterBase monsterBase = monster.GetComponent<MonsterBase>();
            
                        // Attack과 Eyesight 컴포넌트 찾아서 MonsterBase 참조 설정
                        Attack attackComponent = monster.GetComponentInChildren<Attack>();
                        Eyesight eyesightComponent = monster.GetComponentInChildren<Eyesight>();
            
                        if (attackComponent != null)
                            attackComponent.SetMonsterBase(monsterBase);
                        if (eyesightComponent != null)
                            eyesightComponent.SetMonsterBase(monsterBase);
            
                        monsterPool.Add(monster);
                        activeMonsters.Add(monster);
                    }
                }
            }
            else
            {
                // 각 스폰 포인트에 일반 몬스터 6마리씩 한 번만 스폰
                for (int i = 0; i < NORMAL_MONSTER_COUNT; i++)
                {
                    foreach (Transform spawnPoint in spawnPoints)
                    {
                        GameObject monsterPrefab = GetNormalMonsterPrefab();
                        Vector3 spawnPosition = spawnPoint.position;
            
                        GameObject monster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
                        monster.transform.parent = transform;
            
                        // MonsterBase 컴포넌트 가져오기
                        MonsterBase monsterBase = monster.GetComponent<MonsterBase>();
            
                        // Attack과 Eyesight 컴포넌트 찾아서 MonsterBase 참조 설정
                        Attack attackComponent = monster.GetComponentInChildren<Attack>();
                        Eyesight eyesightComponent = monster.GetComponentInChildren<Eyesight>();
            
                        if (attackComponent != null)
                            attackComponent.SetMonsterBase(monsterBase);
                        if (eyesightComponent != null)
                            eyesightComponent.SetMonsterBase(monsterBase);
            
                        monsterPool.Add(monster);
                        activeMonsters.Add(monster);
                    }
                }  
            }
            
            // 일반 웨이브에서는 자동으로 다음 웨이브로 넘어가지 않음
            // 몬스터가 모두 죽으면 Update에서 체크하여 다음 웨이브로 넘어감
        }

        private GameObject GetNormalMonsterPrefab()
        {
            // 현재 웨이브에 해당하는 일반 몬스터 프리팹 반환
            int waveSetIndex = (currentWave - 1) / 3; // 웨이브 세트 인덱스 (0-2)
            
            // 각 세트의 두 번째 웨이브인 경우 monsterPrefabB 반환
            bool isSecondWave = (currentWave % 3) == 2;
            
            return isSecondWave ? 
                waveDataArray[waveSetIndex].monsterPrefabB : 
                waveDataArray[waveSetIndex].monsterPrefabA;
        }
        
        private IEnumerator StartNextWaveWithDelay()
        {
            float timer = nextWaveDelay;
            
            while (timer > 0)
            {
                waveCounter.text = Mathf.CeilToInt(timer).ToString(); // 남은 시간 표시
                timer -= Time.deltaTime;
                yield return null;
            }

            // waveCounter.text = "0";
            waveCounter.text = null;
            
            isWaitingForNextWave = false;
            StartNextWave();
        }
    }
}
