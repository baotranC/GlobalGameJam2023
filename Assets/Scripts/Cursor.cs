using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour {

    [HideInInspector] public float horizontalSpeed = 2f;
    [HideInInspector] public float verticalSpeed = 4f;

    Rigidbody2D rb;
    float moveInput;
    Vector3 initialPos;
    GameManager gameManager;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
        initialPos = transform.position;
    }

    public void Reset() {
        transform.position = initialPos;
        rb.velocity = Vector2.zero;
    }

    public void Play() {
        rb.velocity = Vector2.right * horizontalSpeed;
    }

    public void PlayInput(InputAction.CallbackContext context) {
        if(context.started) {
            Play();
        }
    }

    public void MoveInput(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<float>();
        rb.velocity = new Vector2(rb.velocity.x, moveInput * verticalSpeed);
    }
}
