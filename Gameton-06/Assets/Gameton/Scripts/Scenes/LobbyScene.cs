using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TON
{
    public class LobbyScene : SceneBase
    {
        public SerializableDictionary<string, Sprite> CharacterSpriteDict = new SerializableDictionary<string, Sprite>();


        public override IEnumerator OnStart()
        {
            // Lobby 씬을 비동기로 로드한다.
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Lobby", LoadSceneMode.Single);

            // 로드가 완료될 때 까지 yield return null 을 하면서 기다린다
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            PlayerSpawner.SpawnPlayerCharacter();
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
