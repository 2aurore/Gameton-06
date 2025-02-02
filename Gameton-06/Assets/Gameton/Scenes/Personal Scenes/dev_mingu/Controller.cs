using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class Controller : MonoBehaviour
    {
        private Transform _cachedTransform;
        private Animator _anmator;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _cachedTransform = transform;
            _anmator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            bool isMoving = false;

            if (Input.GetKey(KeyCode.RightArrow))
            {
                var position = _cachedTransform.position;
                position.x += 5.0f * Time.deltaTime;
                _cachedTransform.position = position;

                isMoving = true;

                _spriteRenderer.flipX = false;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                var position = _cachedTransform.position;
                position.x -= 5.0f * Time.deltaTime;
                _cachedTransform.position = position;

                isMoving = true;

                _spriteRenderer.flipX = true;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                var position = _cachedTransform.position;
                position.y += 10.0f * Time.deltaTime;
                _cachedTransform.position = position;

                isMoving = true;

                _spriteRenderer.flipX = true;
            }

            // _anmator.SetBool("IsMoving", isMoving);
        }
    }
}
