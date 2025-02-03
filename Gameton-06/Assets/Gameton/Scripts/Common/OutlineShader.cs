using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class OutlineShader : MonoBehaviour
    {
        public Material outlineMaterial;  // 아웃라인 쉐이더 머티리얼
        public Material originalMaterial;
        public SpriteRenderer spriteRenderer;

        void Start()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                originalMaterial = spriteRenderer.material;
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))  // 플레이어가 접근하면
            {
                spriteRenderer.material = outlineMaterial;
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))  // 플레이어가 나가면
            {
                spriteRenderer.material = originalMaterial;
            }
        }
    }
}
