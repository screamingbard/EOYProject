using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
[RequireComponent(typeof(RopeSystem))]
[RequireComponent(typeof(PlayerCrosshair))]
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
	
	public bool IsGrappling { get; private set; }
	public bool IsGrounded { get { return controller.collisions.below; } }

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
	private RopeSystem ropeSystem;
	private PlayerCrosshair crosshair;

	void Start() {
		controller = GetComponent<Controller2D>();
		ropeSystem = GetComponent<RopeSystem>();

		gravity = -(2 * maxJumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt (2 * Mathf.Abs (gravity) * minJumpHeight);

		playerSprite = GetComponent<SpriteRenderer>();
		rBody = GetComponent<Rigidbody2D>();

		crosshair = GetComponent<PlayerCrosshair>();
	}

	void Update() {
		CalculateVelocity ();

		if (IsGrappling)
			velocity.y = 0f;

		if (IsGrounded && rBody.velocity != Vector2.zero)
			rBody.velocity = Vector2.zero;

		controller.Move(velocity * Time.deltaTime, directionalInput);

		CheckVerticalCollisions();
		CheckSpriteDirection();
	}

	public void SetDirectionalInput(Vector2 input) {
		//Flips the player sprite depending on input direction
		if(input.x != 0f) {
			Vector3 facing = new Vector3(input.x > 0 ? 1 : -1, 1f, 1f);
			transform.localScale = facing;
		}

		directionalInput = input;
	}

	public void OnGrappleInput(Vector2 aimDirection) {
		ropeSystem.OnGrappleInput(aimDirection);
	}

	public void UpdateCrosshair(float aimAngle) {
		crosshair.SetCrosshair(aimAngle);
	}

	public void OnGrappleUp() {
		ropeSystem.OnGrappleInputUp();
	}

	public void OnGrappleReel(float direction) {
		ropeSystem.HandleRopeLength(direction);
	}

	public void OnJumpInput() {
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

	public void SetGrappling(bool isGrappling) {
		IsGrappling = isGrappling;
	}

	public void SetRopeHook(Vector2 ropeHook) {
		this.ropeHook = ropeHook;
	}

	private void CalculateVelocity() {
		if (directionalInput.x > 0 && IsGrappling || directionalInput.x < 0 && IsGrappling) {
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
