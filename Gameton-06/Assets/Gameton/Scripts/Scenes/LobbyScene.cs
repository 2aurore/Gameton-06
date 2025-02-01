using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TON
{
    public class LobbyScene : SceneBase
    {
        public SerializableDictionary<string, Sprite> CharacterSpriteDict = new SerializableDictionary<string, Sprite>();

        [SerializeField] private List<PlayerData> playerDatas;

        public override IEnumerator OnStart()
        {
            // Lobby 씬을 비동기로 로드한다.
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Lobby", LoadSceneMode.Single);

            // 로드가 완료될 때 까지 yield return null 을 하면서 기다린다
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            SpawnPlayerCharacter();
        }

        private void SpawnPlayerCharacter()
        {
            playerDatas = PlayerDataManager.Singleton.players;
            // 저장된 인덱스 가져오기
            int selectedIndex = PlayerPrefs.GetInt("SelectedPlayerIndex", 0);

            // 인덱스가 범위를 벗어나지 않는지 확인
            if (selectedIndex < 0 || selectedIndex >= playerDatas.Count)
            {
                Debug.LogError($"Invalid player index: {selectedIndex}");
                return;
            }

            string prefabName = playerDatas[selectedIndex].type == "b" ? "TON.Player_B" : "TON.Player_W";
            // Resources에서 프리팹 로드
            GameObject characterPrefab = Resources.Load<GameObject>($"Player/{prefabName}");
            if (characterPrefab == null)
            {
                Debug.LogError($"Failed to load character prefab: {playerDatas[selectedIndex].type}");
                return;
            }

            // TON.Player 오브젝트 찾기
            GameObject playerObj = GameObject.Find("TON.Player");
            if (playerObj == null)
            {
                Debug.LogError("TON.Player not found in the scene!");
                return;
            }

            // 기존 플레이어 제거 (필요 시)
            foreach (Transform child in playerObj.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            // 캐릭터 프리팹을 TON.Player 위치에 배치
            GameObject playerInstance = Instantiate(characterPrefab, playerObj.transform.position, Quaternion.identity);
            playerInstance.transform.SetParent(playerObj.transform);
        }

        public override IEnumerator OnEnd()
        {
            // TON.Player 내부의 캐릭터 삭제
            GameObject playerObj = GameObject.Find("TON.Player");
            if (playerObj != null)
            {
                foreach (Transform child in playerObj.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }

            yield return null;
        }

    }
}
