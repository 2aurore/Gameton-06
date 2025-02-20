using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class WallChecker : MonoBehaviour
    {
        public bool IsWallTouching { get; private set; }

        private void OnTriggerStay2D(Collider2D other)
        {
            IsWallTouching = other.gameObject.layer == LayerMask.NameToLayer("Ground");
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                IsWallTouching = false;
            }
        }
    }
}
