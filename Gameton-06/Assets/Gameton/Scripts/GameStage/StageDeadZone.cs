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
                CharacterBase character = collision.gameObject.GetComponentInChildren<CharacterBase>();
                character.Dead();

                character.OnDeathCompleted += ShowGameOverUI; // 이벤트 구독

            }
        }

        private void ShowGameOverUI()
        {
            UIManager.Show<GameoverUI>(UIList.GameOverUI);
        }

    }
}
