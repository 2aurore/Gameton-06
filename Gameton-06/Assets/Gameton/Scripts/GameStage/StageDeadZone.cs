using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class StageDeadZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                // 캐릭터가 StageDeadZone에 들어옴과 동시에 게임오버 정보를 저장하도록 함
                // StageManager.Singleton.StageGameOver();

                // 캐릭터 Dead 로직 실행
                CharacterBase character = collision.gameObject.GetComponentInChildren<CharacterBase>();
                character.Dead();

                // character.OnDeathCompleted += ShowGameOverUI; // 이벤트 구독

            }
        }

        private void ShowGameOverUI()
        {
            // 게임 오버 UI 조건 삭제로 해당 코드 주석처리함
            // UIManager.Show<GameoverUI>(UIList.GameOverUI);
        }

    }
}
