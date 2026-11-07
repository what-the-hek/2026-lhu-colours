using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HeneGames.DialogueSystem
{
    public class Movement2D : MonoBehaviour
    {
        float horizontal;
        Rigidbody2D rb2D;

        private void Start()
        {
            rb2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            horizontal = 0f;

            if (Keyboard.current.aKey.isPressed)
                horizontal = -1f;

            if (Keyboard.current.dKey.isPressed)
                horizontal = 1f;
        }

        private void FixedUpdate()
        {
            rb2D.linearVelocity = new Vector2(horizontal * 10f, rb2D.linearVelocity.y);
        }
    }
}