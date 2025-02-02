using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class LoadingUI : UIBase
    {
        // private Animator playerAnimator;

        // private void Start()
        // {
        //     PlayerSpawner.SpawnPlayerCharacter();
        //     // TON.Player 하위의 Animator 찾기
        //     GameObject playerObj = GameObject.Find("TON.Player");
        //     if (playerObj != null)
        //     {
        //         playerAnimator = playerObj.GetComponentInChildren<Animator>();
        //         if (playerAnimator != null)
        //         {
        //             playerAnimator.SetBool("IsMoving", true);  // Loading UI에서만 달리게 설정
        //         }
        //     }
        // }

        // private void OnDisable()  // Loading UI가 사라질 때
        // {
        //     if (playerAnimator != null)
        //     {
        //         playerAnimator.SetBool("IsMoving", false);  // 인게임 시작 전 멈추도록 설정
        //     }
        // }
    }
}
