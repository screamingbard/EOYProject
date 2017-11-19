using UnityEngine;

[RequireComponent(typeof(GrappleSystem))]
[RequireComponent(typeof(PlayerCrosshair))]
public class Player : MonoBehaviour {
	[Header("Ground Movement")]
	public float moveSpeed = 1f;
	public float pullFromGround;
	public float groundedRayLength;
	public LayerMask groundMask;

	[Header("Grappling")]
	public float swingForce = 4f;
	public Vector2 ropeHook;

	[Header("Jumping")]
	public float jumpForce = 3f;
	public float jumpForceLoss = 2f;

	[Header("Velocity Control")]
	public float fallSpeedForce = 10f;
	public float maxVelocity;

	[Header("Sprite")]
	public Transform playerSprite;

	public bool IsGrappling { get; private set; }
	public bool IsGrounded { get; private set; }
	public bool IsJumping { get; private set; }
	public bool IsFalling { get { return rBody.velocity.y < 0 ? true : false; }}

	private Vector2 directionalInput;

	private PlayerCrosshair crosshair;
	private Collider2D playerCollider;
	private Rigidbody2D rBody;
	private GrappleSystem grappleSystem;

	private float currentJumpForce;

	void Awake() {
		rBody = GetComponent<Rigidbody2D>();

		crosshair = GetComponent<PlayerCrosshair>();
		playerCollider = GetComponent<Collider2D>();
		grappleSystem = GetComponent<GrappleSystem>();
	}

	void Update() {
		CheckGrounded();
	}

	void FixedUpdate() {
		UpdateHorizontalVelocity();
		UpdateVerticalVelocity();
		ClampVelocity();
	}

	public void SetDirectionalInput(Vector2 input) {
		directionalInput = input;
		UpdateSpiteFacing(input.x);
	}

	public void OnGrappleInput(Vector2 aimDirection) {
		grappleSystem.OnGrappleInput(aimDirection);
	}

	public void OnGrappleUp() {
		grappleSystem.OnGrappleInputUp();
	}

	public void OnGrappleReel(float direction) {
		grappleSystem.AdjustRopeLength(direction);
	}

	public void OnJumpInputDown() {
		currentJumpForce = jumpForce;
		IsJumping = true;
	}

	public void OnJumpInput() {
		currentJumpForce -= jumpForceLoss * Time.deltaTime;

		if (currentJumpForce < 0) {
			IsJumping = false;
		}
	}

	public void OnJumpInputUp() {
		IsJumping = false;
	}

	public void UpdateCrosshair(float aimAngle) {
		crosshair.SetCrosshair(aimAngle);
	}

	public void SetRopeHook(Vector2 ropePosition) {
		ropeHook = ropePosition;
	}

	public void SetGrappling(bool isGrappling) {
		IsGrappling = isGrappling;
	}

	private void CheckGrounded() {
		float halfHeight = playerCollider.bounds.extents.y;
		IsGrounded = Physics2D.Raycast(new Vector2(transform.position.x + playerCollider.offset.x, (transform.position.y + playerCollider.offset.y) - halfHeight - 0.04f), Vector2.down, groundedRayLength, groundMask);

		if(IsGrappling && IsGrounded) {
			grappleSystem.RemoveRopeLength(pullFromGround);
		}
	}

	private void UpdateHorizontalVelocity() {
		if (directionalInput.x < 0f || directionalInput.x > 0f) {
			if (IsGrappling) {
				// Get normalized direction vector from player to the hook point
				Vector2 playerToHookDirection = (ropeHook - (Vector2)transform.position).normalized;

				// Inverse the direction to get a perpendicular direction
				Vector2 perpendicularDirection;
				if (directionalInput.x < 0) {
					perpendicularDirection = new Vector2(-playerToHookDirection.y, playerToHookDirection.x);
					Vector2 leftPerpPos = (Vector2)transform.position - perpendicularDirection * -2f;
					Debug.DrawLine(transform.position, leftPerpPos, Color.green, 0f);
				} else {
					perpendicularDirection = new Vector2(playerToHookDirection.y, -playerToHookDirection.x);
					Vector2 rightPerpPos = (Vector2)transform.position + perpendicularDirection * 2f;
					Debug.DrawLine(transform.position, rightPerpPos, Color.green, 0f);
				}

				Vector2 force = perpendicularDirection * swingForce;
				rBody.AddForce(force, ForceMode2D.Force);
			} else {
				if (IsGrounded) {
					rBody.AddForce(new Vector2((directionalInput.x * moveSpeed - rBody.velocity.x) * moveSpeed, 0));
					rBody.velocity = new Vector2(rBody.velocity.x, rBody.velocity.y);
				}
			}
		} else if (directionalInput.x == 0f && IsGrappling == false && IsGrounded == true) {
			rBody.velocity = new Vector2(0f, rBody.velocity.y);
		}
	}

	private void UpdateVerticalVelocity() {
		if (IsGrappling == false) {
			if (IsGrounded == false && IsFalling) {
				rBody.AddForce(new Vector2(0, -fallSpeedForce));
				rBody.velocity = new Vector2(rBody.velocity.x, rBody.velocity.y);
			} 
			else if (IsJumping) {
				rBody.velocity = new Vector2(rBody.velocity.x + directionalInput.x, currentJumpForce);
			}
			else if (IsGrounded) {
				rBody.AddForce(new Vector2(0, -fallSpeedForce));
			}
		}
	}

	private void ClampVelocity() {
		if (rBody.velocity.magnitude > maxVelocity) {
			rBody.velocity = Vector2.ClampMagnitude(rBody.velocity, maxVelocity);
		}
	}

	private void UpdateSpiteFacing(float xDirection) {
		if(xDirection < 0 && playerSprite.localScale.x < 0) {
			playerSprite.localScale = Vector3.one;
		}
		else if(xDirection > 0 && playerSprite.localScale.x > 0) {
			playerSprite.localScale = new Vector3(-1, 1, 1);
		}
	}
}
