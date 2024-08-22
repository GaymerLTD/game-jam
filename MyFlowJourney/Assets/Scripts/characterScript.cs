using System;
using UnityEngine;
using UnityEngine.UI;

public class characterScript : MonoBehaviour
{
	public Camera cameraComponent;

	public Rigidbody2D Rigidbody;
	public float VerticalSpeed;
	public float MaxY;
	public float antiGravity;

	public float minXAcceleration;
	public float xAcceleration;
	public float maxXAcceleration;

	public float minXVelocity;
	public float maxXVelocity;

	public float RotationalSpeed = 270;

	public bool IsGrounded = false;
	public int NumFlips;
	public int MaxFlips = 10;
	public float FlipBoost;

	public Text ScoreDisplay;
	private int score;
	private readonly string GROUND_TAG = "ground";

	private float prevScoreDistance;

	public GameObject Ground;
	// Start is called before the first frame update

	[SerializeField] private AudioClip deathSound;
	[SerializeField] private AudioClip landingSound;
	[SerializeField] private AudioClip backgroundMusic;
	[SerializeField] private AudioSource primaryAudioSource;
	[SerializeField] private AudioSource landingAudioSource;
	[SerializeField] private AudioSource secondaryAudioSource;
	private Boolean GAME_OVER;
	void Start()
	{
		Ground = GameObject.FindGameObjectWithTag(GROUND_TAG);

		VerticalSpeed = 1500;
		MaxY = 1000;
		antiGravity = 10f;

		minXAcceleration = 250;
		xAcceleration = 400;
		maxXAcceleration = 500;
		minXVelocity = 1000;
		maxXVelocity = 7500;
		FlipBoost = 3000;

		IsGrounded = false;

		Rigidbody.velocity = Vector2.right * (float)(xAcceleration);

		playClip(secondaryAudioSource, backgroundMusic, true, 0.1f);
		score = 0;
		prevScoreDistance = 0;
	}

	// Update is called once per frame
	void Update()
	{
		if (!GAME_OVER)
		{
			float currentDeltaTime = Time.deltaTime;
			if (Input.GetKeyDown(KeyCode.Space))
			{
				if (IsGrounded)
				{
					double ratio = 1 / 100.0;
					currentDeltaTime = (float)(ratio);
					if (currentDeltaTime >= 1 / 50.0)
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

			if (!IsGrounded)
			{
				if (Input.GetKey(KeyCode.Space))
				{
					var prevRotation = Rigidbody.rotation;
					Rigidbody.rotation += RotationalSpeed * Time.deltaTime;
					Rigidbody.rotation = WrapAngle(Rigidbody.rotation);
					if (prevRotation < -45 && Rigidbody.rotation > -45)
					{
						if (NumFlips < MaxFlips) NumFlips++;
						Debug.Log($"Flipped {NumFlips} times.");
					}
				}
			}
			cameraComponent.transform.rotation = new Quaternion() { eulerAngles = new Vector3(0, 0, 0) };

			BoostX(xAcceleration, currentDeltaTime);
		}
		else
		{ //ITS GAME OVER
			Rigidbody.velocity = Vector2.zero;
		}

		if (!IsGrounded)
		{
			if (Input.GetKey(KeyCode.Space))
			{
				var prevRotation = Rigidbody.rotation;
				Rigidbody.rotation += RotationalSpeed * Time.deltaTime;
				Rigidbody.rotation = WrapAngle(Rigidbody.rotation);
				if (prevRotation < -45 && Rigidbody.rotation > -45)
				{
					if (NumFlips < MaxFlips) NumFlips++;
					Debug.Log($"Flipped {NumFlips} times.");
				}
			}
		}
		cameraComponent.transform.rotation = new Quaternion() { eulerAngles = new Vector3(0, 0, 0) };

		BoostX(xAcceleration, currentDeltaTime);

		if (transform.position.x - prevScoreDistance > 20)
		{
			score++;
			prevScoreDistance = transform.position.x;
		}


		ScoreDisplay.text = score.ToString();
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag(GROUND_TAG))
		{
			if (NumFlips > 0)
				playClip(landingAudioSource, landingSound, volume: 0.2f);
			else
				playClip(landingAudioSource, landingSound, volume: 0.005f);
			Rigidbody.gravityScale = 10;
			IsGrounded = true;
			var boost = FlipBoost * NumFlips;
			var deltaT = Time.deltaTime;
			if (boost > 0) Debug.Log($"Boosting with {boost}, {NumFlips}");
			BoostX(boost, deltaT);
			score += NumFlips;
			NumFlips = 0;
		}
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (deathSound != null)
		{
			playClip(primaryAudioSource, deathSound, volume: 0.8f);
			deathSound = null;
		}
		Debug.Log("Game over!");
		GAME_OVER = true;
		Rigidbody.velocity = Vector2.zero;
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
		if (collision.gameObject.CompareTag(GROUND_TAG))
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

	private void playClip(AudioSource source, AudioClip clip, Boolean loop = false, float volume = 1f)
	{
		source.clip = clip;
		source.volume = volume;
		source.Play();
		source.loop = loop;
	}
}
