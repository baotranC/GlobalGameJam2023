using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cursor : MonoBehaviour
{

	public GameObject[] darkness;

	Rigidbody2D rb;
	public TrailRenderer tr;
	float moveInput;
	Vector3 initialPos;
	[HideInInspector] public float horizontalSpeed = 2f;
	[HideInInspector] public float verticalSpeed = 4f;


	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		initialPos = transform.position;
	}

	public void Reset()
	{
		transform.position = initialPos;
		tr.Clear();
        rb.velocity = Vector2.zero;
	}

	public void Play()
	{
		rb.velocity = Vector2.right * horizontalSpeed;
	}

	public void PlayInput(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			Play();
		}
	}

	public void MoveInput(InputAction.CallbackContext context)
	{
		moveInput = context.ReadValue<float>();
		rb.velocity = new Vector2(rb.velocity.x, moveInput * verticalSpeed);
	}

	public void SetDarkness(bool value)
	{
		foreach (GameObject go in darkness)
		{
			go.SetActive(value);
		}
	}
}
