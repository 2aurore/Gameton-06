using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TON
{
    public class IngameScene : SceneBase
    {

        public override IEnumerator OnStart()
        {
            // Ingame 씬을 비동기로 로드한다.
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Ingame", LoadSceneMode.Single);

            // 로드가 완료될 때 까지 yield return null 을 하면서 기다린다
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            UIManager.Show<IngameUI>(UIList.IngameUI);
            UIManager.Show<ControllerUI>(UIList.ControllerUI);


            string selectedCharacter = PlayerPrefs.GetString("SelectedCharacter", "DefaultCharacter");
            Debug.Log("선택된 캐릭터: " + selectedCharacter);

            // 선택된 캐릭터에 맞는 오브젝트를 생성하거나 적용하는 코드 작성
        }
        public override IEnumerator OnEnd()
        {
            yield return null;

            UIManager.Hide<IngameUI>(UIList.IngameUI);
            UIManager.Hide<ControllerUI>(UIList.ControllerUI);
        }

        void OnEscapeExecute()
        {
            // TODO : 게임 일시정지 UI 노출 시 수행
            // Time.timeScale = 0f;
            // UIManager.Show<PausePopupUI>(UIList.PausePopupUI);
        }
    }
}
