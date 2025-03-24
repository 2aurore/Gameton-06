using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class PlayerSpawner
    {
        public static bool SpawnPlayerCharacter()
        {
            PlayerData playerData = PlayerDataManager.Singleton.player;

            string prefabName = playerData.type == "b" ? "TON.Player_B" : "TON.Player_W";
            // Resources에서 프리팹 로드
            GameObject characterPrefab = Resources.Load<GameObject>($"Player/{prefabName}");
            if (characterPrefab == null)
            {
                Debug.LogError($"Failed to load character prefab: {playerData.type}");
                return false;
            }
            // TON.Player 오브젝트 찾기
            GameObject playerObj = GameObject.Find("TON.Player");
            if (playerObj == null)
            {
                Debug.LogError("TON.Player not found in the scene!");
                return false;
            }
            // 기존 플레이어 제거 (필요 시)
            foreach (Transform child in playerObj.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            // 캐릭터 프리팹을 TON.Player 위치에 배치
            GameObject playerInstance = GameObject.Instantiate(characterPrefab, playerObj.transform.position, Quaternion.identity);
            playerInstance.transform.SetParent(playerObj.transform);
            // 카메라가 새 플레이어를 따라가도록 설정
            CameraFollow cameraFollow = GameObject.FindObjectOfType<CameraFollow>();
            if (cameraFollow != null)
            {
                cameraFollow.SetFollowTarget(playerInstance.transform);
            }
            else
            {
                Debug.LogError("No CameraFollow script found in the scene!");
                return false;
            }

            return true;
        }
    }
}
