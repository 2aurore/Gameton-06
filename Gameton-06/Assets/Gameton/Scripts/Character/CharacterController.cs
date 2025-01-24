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

    }
}
