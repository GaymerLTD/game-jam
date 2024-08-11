using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterScript : MonoBehaviour
{
	public Rigidbody2D Rigidbody;
	private float HorizontalSpeed = 500;
	private float VerticalSpeed = 1500;
	private float RotationalSpeed = 250;
	private bool hasJumped = false;
	private bool isGrounded = false;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			if(isGrounded && !hasJumped)
			{
				Rigidbody.velocity += Vector2.up * VerticalSpeed * Time.deltaTime;
				hasJumped = true;
			}
			if(!isGrounded)
			{
				Rigidbody.rotation += RotationalSpeed * Time.deltaTime;
			}
			if(isGrounded)
			{
				hasJumped = false;
			}
		}

		if(Input.GetKeyDown(KeyCode.D))
			Rigidbody.velocity += Vector2.right * HorizontalSpeed * Time.deltaTime;
		if(Input.GetKeyDown(KeyCode.A))
			Rigidbody.velocity += Vector2.left * HorizontalSpeed * Time.deltaTime;
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.CompareTag("ground"))
			isGrounded = true;
	}

	public void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("ground"))
			isGrounded = false;
	}
}
