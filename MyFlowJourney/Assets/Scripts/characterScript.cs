using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterScript : MonoBehaviour
{
	public Rigidbody2D Rigidbody;
	private float HorizontalSpeed = 500;
	private float VerticalSpeed = 250;
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
		isGrounded = CheckIfGrounded();

		if(Input.GetKeyDown(KeyCode.Space))
		{
			if(isGrounded && !hasJumped)
			{
				Rigidbody.velocity = Vector2.up * VerticalSpeed * Time.deltaTime;
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
			//Rigidbody.velocity = Vector2.up * VerticalSpeed;
			//Rigidbody.rotation += RotationalSpeed;
		}

		if(Input.GetKeyDown(KeyCode.D))
			Rigidbody.velocity += Vector2.right * HorizontalSpeed * Time.deltaTime;
		if(Input.GetKeyDown(KeyCode.A))
			Rigidbody.velocity += Vector2.left * HorizontalSpeed * Time.deltaTime;
	}

	private bool CheckIfGrounded()
	{
		var answer = Physics.Raycast(transform.position, -Vector3.up, 3);
		return answer;
	}
}
