using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TON
{
    public class IntroScene : SceneBase
    {
        public override IEnumerator OnStart()
        {
            // Intro 씬을 비동기로 로드한다.
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Intro", LoadSceneMode.Single);

            // 로드가 완료될 때 까지 yield return null 을 하면서 기다린다
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            UIManager.Show<IntroUI>(UIList.IntroUI);

        }


        public override IEnumerator OnEnd()
        {
            yield return null;

            UIManager.Hide<IntroUI>(UIList.IntroUI);
        }
    }
}
