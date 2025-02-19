using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class CharacterController : MonoBehaviour
    {
        private CharacterBase linkedCharactor;

        public LayerMask groundLayer;

        private void Awake()
        {
            linkedCharactor = GetComponent<CharacterBase>();
        }

        private void Start()
        {
            IngameUI.Instance.SetHP(linkedCharactor.currentHP, linkedCharactor.maxHP);
            IngameUI.Instance.SetSP(linkedCharactor.currentSP, linkedCharactor.maxSP);
            IngameUI.Instance.SetPlayerImage(PlayerDataManager.Singleton.player.type);
        }

    }
}
