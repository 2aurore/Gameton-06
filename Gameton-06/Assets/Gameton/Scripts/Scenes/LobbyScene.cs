using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TON
{
    public class LobbyScene : SceneBase
    {

        public override IEnumerator OnStart()
        {
            // Lobby 씬을 비동기로 로드한다.
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Lobby", LoadSceneMode.Single);

            // 로드가 완료될 때 까지 yield return null 을 하면서 기다린다
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            SkillDataManager.Singleton.Initalize();


            UIManager.Show<LobbyUI>(UIList.LobbyUI);
            UIManager.Show<OptionUI>(UIList.OptionUI);
        }


        public override IEnumerator OnEnd()
        {
            yield return null;

            UIManager.Hide<LobbyUI>(UIList.LobbyUI);
            UIManager.Hide<OptionUI>(UIList.OptionUI);
        }

    }
}
