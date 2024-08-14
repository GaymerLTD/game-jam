using System;
using UnityEngine;

public class characterScript : MonoBehaviour
{
	public Rigidbody2D Rigidbody;
	public float VerticalSpeed = 1500;
	public float MaxY = 3500;

	public float MinX = 1500;
	public float HorizontalSpeed = 1750;
	public float MaxX = 3500;

	public float RotationalSpeed = 325;

	public bool IsGrounded = false;

	// Start is called before the first frame update
	void Start()
	{
		VerticalSpeed = 1500;
		MaxY = 3500;

		MinX = 1500;
		HorizontalSpeed = 1750;
		MaxX = 3500;

		RotationalSpeed = 325;

		IsGrounded = false;


		Rigidbody.velocity = Vector2.right * (float)(HorizontalSpeed/100.0);
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			if(IsGrounded)
			{
				var newY = Math.Min(Rigidbody.velocity.y + VerticalSpeed * Time.deltaTime, MaxY * Time.deltaTime);
				Debug.Log($"Jumping: {newY}");
				Debug.Log($"Pre-jump velocity: {Rigidbody.velocity.ToString("F4")}");
				Rigidbody.velocity += Vector2.up * newY;
				Debug.Log($"Postjump velocity: {Rigidbody.velocity.ToString("F4")}");
				IsGrounded = false;
			}
		}

		if(!IsGrounded)
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
		//Rigidbody.velocity = Rigidbody.velocity.y * Vector2.up + xVelocity * Vector2.right;
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.CompareTag("ground"))
		{
			IsGrounded = true;
			Debug.Log("Hit the ground!");
		}

	}

	public void OnCollisionExit2D(Collision2D collision)
 	{
		if(collision.gameObject.CompareTag("ground"))
		{
			IsGrounded = false;
			Debug.Log("Lifted from the ground!");
		}
	}
}
