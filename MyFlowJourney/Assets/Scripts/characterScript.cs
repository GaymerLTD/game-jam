using System;
using UnityEngine;

public class characterScript : MonoBehaviour
{
	public Rigidbody2D Rigidbody;
	public float VerticalSpeed = 1500;
	public float MaxY = 3500;

	public float HorizontalSpeed = 100;
	public float MinX = 50;
	public float MaxX = 200;

	public float RotationalSpeed = 325;

	public bool isGrounded = false;

	// Start is called before the first frame update
	void Start()
	{
		Rigidbody.velocity = Vector2.right * HorizontalSpeed;
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			if(isGrounded)
			{
				Debug.Log("Jumping");
				var newY = Math.Min(Rigidbody.velocity.y + VerticalSpeed * Time.deltaTime, MaxY * Time.deltaTime);
				Rigidbody.velocity += Vector2.up * newY;
				isGrounded = false;
			}
		}

		if(!isGrounded)
		{
			if(Input.GetKey(KeyCode.Space))
			{
				Debug.Log("Flipping");
				Rigidbody.rotation += RotationalSpeed * Time.deltaTime;
				// Check if we have flipped over here. If so add to a counter and then when we land add a boost
			}
		}

		var xVelocity = Math.Min(Rigidbody.velocity.x, MaxX * Time.deltaTime);
		xVelocity = Math.Max(xVelocity, HorizontalSpeed * Time.deltaTime);
		Rigidbody.velocity = Rigidbody.velocity.y * Vector2.up + xVelocity * Vector2.right;
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.CompareTag("ground"))
		{
			isGrounded = true;
			Debug.Log("Hit the ground!");
		}

	}

	public void OnCollisionExit2D(Collision2D collision)
	{
		if(collision.gameObject.CompareTag("ground"))
		{
			isGrounded = false;
			Debug.Log("Lifted from the ground!");
		}
	}
}
