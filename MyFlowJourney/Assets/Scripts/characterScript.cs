using System;
using UnityEngine;

public class characterScript : MonoBehaviour
{
	public Rigidbody2D Rigidbody;
	public float VerticalSpeed = 1500;
	public float MaxY = 3500;
	public float MaxVerticalSpeed;
	public float antiGravity;

	public float MinX = 800;
	public float HorizontalSpeed = 175;
	public float MaxX = 1000;

	public float RotationalSpeed = 325;

	public bool IsGrounded = false;

	// Start is called before the first frame update
	void Start()
	{
		VerticalSpeed = 1500;
		MaxY = 1000;
		antiGravity = 10f;

		MinX = 2000;
		HorizontalSpeed = 1750;
		MaxX = 3500;

		RotationalSpeed = 325;

		IsGrounded = false;

		Rigidbody.velocity = Vector2.right * (float)(HorizontalSpeed/100.0);
	}

	// Update is called once per frame
	void Update()
  {
    float currentDeltaTime = Time.deltaTime;
    if (Input.GetKeyDown(KeyCode.Space))
		{
			if(IsGrounded)
			{
				double ratio = 1 / 100.0;
				currentDeltaTime = (float) (ratio);
				Debug.Log(Time.deltaTime+" vs "+currentDeltaTime);
        if (currentDeltaTime >= 1 / 50.0)
				{
					currentDeltaTime = (float) (ratio);
				}

				float possibleY = Rigidbody.velocity.y + 0.1f*Rigidbody.velocity.x + antiGravity * currentDeltaTime;
				float maxY =  MaxY * currentDeltaTime;
				Debug.Log(currentDeltaTime+" - one:"+possibleY+", two:" +maxY);
				var newY = Math.Max(possibleY,maxY);

				Rigidbody.velocity += Vector2.up * newY;
				IsGrounded = false;
			}
		}

		if(!IsGrounded)
		{
			if(Input.GetKey(KeyCode.Space))
			{
				Rigidbody.rotation += RotationalSpeed * Time.deltaTime;
				// Check if we have flipped over here. If so add to a counter and then when we land add a boost
			}
		}

		var xVelocity = Math.Min(Rigidbody.velocity.x, MaxX * currentDeltaTime);
		xVelocity = Math.Max(xVelocity, HorizontalSpeed * currentDeltaTime);
		Rigidbody.velocity = Rigidbody.velocity.y * Vector2.up + xVelocity * Vector2.right;
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.CompareTag("ground"))
		{
			Rigidbody.gravityScale = 10;
			IsGrounded = true;
		}

	}

	public void OnCollisionExit2D(Collision2D collision)
 	{
		if(collision.gameObject.CompareTag("ground"))
		{
			Rigidbody.gravityScale = 1;
			IsGrounded = false;
		}
	}
}
