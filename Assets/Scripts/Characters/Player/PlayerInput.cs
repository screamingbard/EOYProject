using UnityEngine;
using System.Collections;
using XboxCtrlrInput;

[RequireComponent (typeof (Player))]
public class PlayerInput : MonoBehaviour {
	public bool twoStickControls;
    public float stopJumpTime;

	private Player player;
	private bool hasReset;
	private float aimAngle;

	private Vector2 facingDirection;
	private Vector2 oldFacingDirection;

    private bool recentlyPaused;
    private float pauseCount;

	void Start () {
		Cursor.visible = false;
		player = GetComponent<Player> ();
	}

	void Update () {
		if (Time.timeScale == 0) {
            recentlyPaused = true;
            return;
        }
           
        else if (recentlyPaused) {
            pauseCount += Time.deltaTime;

            if (pauseCount >= stopJumpTime) {
                pauseCount = 0f;
                recentlyPaused = false;
            }
            else
                return;
        }

		UpdateAimAngle();

		if (XCI.GetNumPluggedCtrlrs() == 0)
			OnInputKeyboard();
		else
			OnInputController();
	}

	private void UpdateAimAngle() {
		oldFacingDirection = facingDirection;


		if (XCI.GetNumPluggedCtrlrs() == 0) {
			Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
			facingDirection = worldMousePosition - transform.position;
		} else {
			if (twoStickControls)
				facingDirection = new Vector2(XCI.GetAxis(XboxAxis.RightStickX), XCI.GetAxis(XboxAxis.RightStickY));
			else
				facingDirection = new Vector2(XCI.GetAxis(XboxAxis.LeftStickX), XCI.GetAxis(XboxAxis.LeftStickY));

			if (facingDirection == Vector2.zero)
				facingDirection = oldFacingDirection;
		}

		aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);

		if (aimAngle < 0f) {
			aimAngle = Mathf.PI * 2 + aimAngle;
		}

		player.UpdateCrosshair(aimAngle);
	}

	private void OnInputController() {
        //Calculate movement direction based on player input
        Vector2 inputDirection = new Vector2(XCI.GetAxisRaw(XboxAxis.LeftStickX), XCI.GetAxisRaw(XboxAxis.LeftStickY));
        if (inputDirection.x > 0)
            inputDirection.x = 1f;
        else if (inputDirection.x < 0)
            inputDirection.x = -1f;

        if (inputDirection.y > 0)
            inputDirection.y = 1f;
        else if (inputDirection.y < 0)
            inputDirection.y = -1f;

        player.SetDirectionalInput(inputDirection);

		//Check for grapple input
		if(XCI.GetAxisRaw(XboxAxis.RightTrigger) != 0f) {
			OnGrappleInput();
		}

		//Check if the grapple has been released
		else if (XCI.GetAxisRaw(XboxAxis.RightTrigger) == 0f && hasReset == false) {
			OnGrappleInputRelease();
		}

		//Check if the player is reeling the grapple in
		if (player.IsGrappling) {
			OnGrappleReel(XCI.GetAxis(XboxAxis.RightStickY));
		}

		//Check for jump input
		if (XCI.GetButtonDown(XboxButton.A)) {
			player.OnJumpInputDown();
		}

		//Check for jump input being held
		if (XCI.GetButton(XboxButton.A)) {
			player.OnJumpInput();
		}

		//Check if jump input has finished to see if jumping needs to be cancelled early
		else if (XCI.GetButtonUp(XboxButton.A)) {
			player.OnJumpInputUp();
		}
	}

	private void OnInputKeyboard() {
        Vector2 inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (inputDirection.x > 0)
            inputDirection.x = 1f;
        else if (inputDirection.x < 0)
            inputDirection.x = -1f;

        if (inputDirection.y > 0)
            inputDirection.y = 1f;
        else if (inputDirection.y < 0)
            inputDirection.y = -1f;
        //Calculate movement direction based on player input
        player.SetDirectionalInput(inputDirection);

		//Check for grapple input
		if (Input.GetMouseButton(0)) {
			OnGrappleInput();
		}

		//Check if the grapple has been released
		else if (Input.GetMouseButtonUp(0)) {
			OnGrappleInputRelease();
		}

		//Check if the player is reeling the grapple in
		if (player.IsGrappling) {
			OnGrappleReel(Input.GetAxis("Vertical"));
		}

		//Check for jump input
		if (Input.GetKeyDown(KeyCode.Space)) {
			player.OnJumpInputDown();
		}

		//Check for jump input being held
		if (Input.GetKey(KeyCode.Space)) {
			player.OnJumpInput();
		}

		//Check if jump input has finished to see if jumping needs to be cancelled early
		else if (Input.GetKeyUp(KeyCode.Space)) {
			player.OnJumpInputUp();
		}
	}
	
	private void OnGrappleInput() {
		if (hasReset)
			hasReset = false;

		var aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;

		player.OnGrappleInput(aimDirection);
	}

	private void OnGrappleInputRelease() {
		if (hasReset == false)
			hasReset = true;

		player.OnGrappleUp();
	}

	private void OnGrappleReel(float direction) {
		player.OnGrappleReel(direction);
	}
}
