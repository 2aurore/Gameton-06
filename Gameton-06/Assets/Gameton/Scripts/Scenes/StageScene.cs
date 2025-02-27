using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace TON
{
    public class StageScene : SceneBase
    {

        public override IEnumerator OnStart()
        {
            string stageId = PlayerPrefs.GetString("StageId", null);
            Assert.IsNotNull(stageId, "StageScene:: 스테이지 진입 실패!");

            // 선택한 스테이지에 맞는 씬을 로드한다
            AsyncOperation asyncLoad = null;
            switch (stageId)
            {
                case "STG001":
                    asyncLoad = SceneManager.LoadSceneAsync("Stage 1", LoadSceneMode.Single);
                    break;
                case "STG002":
                    asyncLoad = SceneManager.LoadSceneAsync("Stage 2", LoadSceneMode.Single);
                    break;
                case "STG003":
                    asyncLoad = SceneManager.LoadSceneAsync("Stage 3", LoadSceneMode.Single);
                    break;
                case "STG004":
                    asyncLoad = SceneManager.LoadSceneAsync("Stage 4", LoadSceneMode.Single);
                    break;
                default:
                    Assert.IsTrue(false, "StageScene:: 스테이지 진입 실패!");
                    break;
            }

            // 로드가 완료될 때 까지 yield return null 을 하면서 기다린다
            while (asyncLoad != null && !asyncLoad.isDone)
            {
                yield return null;
            }

            // 선택된 캐릭터에 맞는 오브젝트를 생성하거나 적용하는 코드 작성
            if (!PlayerSpawner.SpawnPlayerCharacter())
            {
                Debug.LogError("Failed to spawn player character!");
                // 에러 UI를 표시하거나 씬을 다시 로드하는 등의 처리
                // UIManager.Show<ErrorUI>(UIList.ErrorUI);
                // 또는 SceneManager.LoadScene("ErrorScene"); 등                
                yield break;
            }


            SkillDataManager.Singleton.Initalize();
            // StageManager.Singleton.StartStage(stageId);
            StageManager.Singleton.StartStage();
            UIManager.Show<IngameUI>(UIList.IngameUI);
            UIManager.Show<OptionUI>(UIList.OptionUI);
            UIManager.Show<ControllerUI>(UIList.ControllerUI);

        }
        public override IEnumerator OnEnd()
        {
            yield return null;

            UIManager.Hide<IngameUI>(UIList.IngameUI);
            UIManager.Hide<OptionUI>(UIList.OptionUI);
            UIManager.Hide<ControllerUI>(UIList.ControllerUI);

            // UIManager.Hide<GameoverUI>(UIList.GameOverUI);
            UIManager.Hide<GameWinUI>(UIList.GameWinUI);
            UIManager.Hide<PauseUI>(UIList.PauseUI);

        }

    }
}
