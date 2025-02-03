using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace TON
{
    [ExecuteInEditMode]
    public class SpriteOutline : MonoBehaviour
    {
        public Color color = Color.white;

        [Range(0, 16)]
        public int outlineSize = 1;

        private SpriteRenderer spriteRenderer;

        void OnEnable()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            // UpdateOutline(true);
        }

        // void OnDisable()
        // {
        //     UpdateOutline(false);
        // }

        // void Update()
        // {
        //     UpdateOutline(true);
        // }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))  // 플레이어가 접근하면
            {
                UpdateOutline(true);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))  // 플레이어가 나가면
            {
                UpdateOutline(false);
            }
        }

        void UpdateOutline(bool outline)
        {
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            spriteRenderer.GetPropertyBlock(mpb);
            mpb.SetFloat("_Outline", outline ? 1f : 0);
            mpb.SetColor("_OutlineColor", color);
            mpb.SetFloat("_OutlineSize", outlineSize);
            spriteRenderer.SetPropertyBlock(mpb);
        }
    }
}
