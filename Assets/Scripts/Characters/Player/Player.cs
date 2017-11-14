using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour {

	[Header("Grounded Movement")]
	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float timeToJumpApex = 0.4f;
	public float accelerationTimeAirborne = 0.2f;
	public float accelerationTimeGrounded = 0.1f;
	public float moveSpeed = 6;

	[Header("Grapple Movement")]
	public float swingForce = 4;
	
	public bool IsSwinging { get; private set; }
	public bool IsGrounded { get { return controller.collisions.below; } }
	//public bool IsJumping { get { return } }

	private float gravity;
	private float maxJumpVelocity;
	private float minJumpVelocity;
	private Vector3 velocity;
	private float velocityXSmoothing;

	private SpriteRenderer playerSprite;
	private Controller2D controller;
	private Vector2 directionalInput;

	private Vector2 ropeHook;
	private Rigidbody2D rBody;

	void Start() {
		controller = GetComponent<Controller2D> ();

		gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);

		playerSprite = GetComponent<SpriteRenderer>();
		rBody = GetComponent<Rigidbody2D>();
	}

	void Update() {
		CalculateVelocity ();

		if (IsSwinging)
			velocity.y = 0f;

		controller.Move(velocity * Time.deltaTime, directionalInput);

		CheckVerticalCollisions();
		CheckSpriteDirection();
	}

	public void SetDirectionalInput (Vector2 input) {
		//Flips the player sprite depending on input direction
		playerSprite.flipX = input.x < 0f;

		directionalInput = input;
	}

	public void OnJumpInputDown() {
		if (controller.collisions.below) {
			if (controller.collisions.slidingDownMaxSlope) {
				if (directionalInput.x != -Mathf.Sign (controller.collisions.slopeNormal.x)) { // not jumping against max slope
					velocity.y = maxJumpVelocity * controller.collisions.slopeNormal.y;
					velocity.x = maxJumpVelocity * controller.collisions.slopeNormal.x;
				}
			} else {
				velocity.y = maxJumpVelocity;
			}
		}
	}

	public void OnJumpInputUp() {
		if (velocity.y > minJumpVelocity) {
			velocity.y = minJumpVelocity;
		}
	}

	public void SetSwinging(bool isSwinging) {
		IsSwinging = isSwinging;
	}

	public void SetRopeHook(Vector2 ropeHook) {
		this.ropeHook = ropeHook;
	}

	private void CalculateVelocity() {
		if (directionalInput.x > 0 && IsSwinging || directionalInput.x < 0 && IsSwinging) {
			// Get normalized direction vector from player to the hook point
			var playerToHookDirection = (ropeHook - (Vector2)transform.position).normalized;

			// Inverse the direction to get a perpendicular direction
			Vector2 perpendicularDirection;
			if (directionalInput.x < 0) {
				perpendicularDirection = new Vector2(-playerToHookDirection.y, playerToHookDirection.x);
				var leftPerpPos = (Vector2)transform.position - perpendicularDirection * -2f;
				Debug.DrawLine(transform.position, leftPerpPos, Color.green, 0f);
			} else {
				perpendicularDirection = new Vector2(playerToHookDirection.y, -playerToHookDirection.x);
				var rightPerpPos = (Vector2)transform.position + perpendicularDirection * 2f;
				Debug.DrawLine(transform.position, rightPerpPos, Color.green, 0f);
			}

			var force = perpendicularDirection * swingForce;
			rBody.AddForce(force, ForceMode2D.Force);

		} else {
			float targetVelocityX = directionalInput.x * moveSpeed;
			velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
			velocity.y += gravity * Time.deltaTime;
		}
	}

	private void CheckVerticalCollisions() {
		if (controller.collisions.above || controller.collisions.below) {
			if (controller.collisions.slidingDownMaxSlope) {
				velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
			} else {
				velocity.y = 0;
			}
		}
	}

	private void CheckSpriteDirection() {
		if(directionalInput.x > 0 || directionalInput.x < 0) {
			playerSprite.flipX = directionalInput.x < 0f;
		}
	}
}
