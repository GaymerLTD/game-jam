using System;
using UnityEngine;
using UnityEngine.UI;

public class characterScript : MonoBehaviour
{
	public Camera cameraComponent;

	public Rigidbody2D Rigidbody;
	public float VerticalSpeed = 1500;
	public float MaxY = 3500;
	public float MaxVerticalSpeed;
	public float antiGravity;

	public float minXAcceleration = 250;
	public float xAcceleration = 400;
	public float maxXAcceleration = 500;

	public float minXVelocity = 1000;
	public float maxXVelocity = 7500;

	public float RotationalSpeed = 270;

	public bool IsGrounded = false;
	public int NumFlips = 0;
	public int MaxFlips = 10;
	public float FlipBoost = 1000;

	public Text ScoreDisplay;
	private int score;

	private float prevScoreDistance;

	public GameObject Ground;
	// Start is called before the first frame update
	void Start()
	{
		Ground = GameObject.FindGameObjectWithTag("ground");
		Ground.transform.position = Vector3.forward + 25 * Vector3.down;

		VerticalSpeed = 1500;
		MaxY = 1000;
		antiGravity = 10f;

		minXAcceleration = 250;
		xAcceleration = 400;
		maxXAcceleration = 500;
		minXVelocity = 1000;
		maxXVelocity = 7500;
		FlipBoost = 1000;

		IsGrounded = false;

		Rigidbody.velocity = Vector2.right * (float)(xAcceleration);
		score = 0;
		prevScoreDistance = 0;
	}

	// Update is called once per frame
	void Update()
	{
		float currentDeltaTime = Time.deltaTime;
		if(Input.GetKeyDown(KeyCode.Space))
		{
			if(IsGrounded)
			{
				double ratio = 1 / 100.0;
				currentDeltaTime = (float)(ratio);
				if(currentDeltaTime >= 1 / 50.0)
				{
					currentDeltaTime = (float)(ratio);
				}

				float possibleY = Rigidbody.velocity.y + 0.1f * Rigidbody.velocity.x + antiGravity * currentDeltaTime;
				float maxY = MaxY * currentDeltaTime;
				var newY = Math.Max(possibleY, maxY);

				Rigidbody.velocity += Vector2.up * newY;
				IsGrounded = false;
			}
		}

		if(!IsGrounded)
		{
			if(Input.GetKey(KeyCode.Space))
			{
				var prevRotation = Rigidbody.rotation;
				Rigidbody.rotation += RotationalSpeed * Time.deltaTime;
				Rigidbody.rotation = WrapAngle(Rigidbody.rotation);
				if(prevRotation < -45 && Rigidbody.rotation > -45)
				{
					if(NumFlips < MaxFlips) NumFlips++;
					Debug.Log($"Flipped {NumFlips} times.");
				}
			}
		}
		cameraComponent.transform.rotation = new Quaternion() { eulerAngles = new Vector3(0, 0, 0) };

		BoostX(xAcceleration, currentDeltaTime);

		if(transform.position.x - prevScoreDistance > 20)
		{
			score++;
			prevScoreDistance = transform.position.x;
		}


		ScoreDisplay.text = score.ToString();
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.CompareTag("ground"))
		{
			Rigidbody.gravityScale = 10;
			IsGrounded = true;
			var boost = FlipBoost * NumFlips;
			var deltaT = Time.deltaTime;
			if(boost > 0) Debug.Log($"Boosting with {boost}");
			BoostX(boost, deltaT);
			score += NumFlips;
			NumFlips = 0;
		}
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("Game over");
	}

	public void BoostX(float a_x, float currentDeltaTime)
	{
		// Clamp
		a_x = Clamp(a_x, maxXAcceleration, minXAcceleration);
		// Get new Vx
		var changeInVx = a_x * currentDeltaTime;
		var newVx = Rigidbody.velocity.x + changeInVx;

		// Re-compute with the change
		newVx = Clamp(newVx, maxXVelocity * currentDeltaTime, minXVelocity * currentDeltaTime);

		// Apply the new velocity
		Rigidbody.velocity = Rigidbody.velocity.y * Vector2.up + newVx * Vector2.right;
	}

	public float Clamp(float x, float upper, float lower)
	{
		var answer = Math.Max(Math.Min(x, upper), lower);
		return answer;
	}

	public void OnCollisionExit2D(Collision2D collision)
	{
		if(collision.gameObject.CompareTag("ground"))
		{
			Rigidbody.gravityScale = 1;
			IsGrounded = false;
		}
	}

	private float WrapAngle(float angle)
	{
		var answer = ((angle + 180) % 360) - 180;
		return answer;
	}
}
