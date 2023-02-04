using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour {

    public float horizontalSpeed = 2f;
    public float verticalSpeed = 4f;

    Rigidbody2D rb;
    float moveInput;
    Vector3 initialPos;
    MelodyManager melodyManager;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        melodyManager = FindObjectOfType<MelodyManager>();
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
            melodyManager.StartPlay();
        }
    }

    public void MoveInput(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<float>();
        rb.velocity = new Vector2(rb.velocity.x, moveInput * verticalSpeed);
    }
}
