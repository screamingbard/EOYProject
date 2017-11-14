using UnityEngine;
using System.Collections;
using XboxCtrlrInput;

[RequireComponent (typeof (Player))]
public class PlayerInput : MonoBehaviour {

	private Player player;

	void Start () {
		player = GetComponent<Player> ();
	}

	void Update () {
		if (Time.timeScale == 0)
			return;

		if (XCI.GetNumPluggedCtrlrs() == 0)
			HandleInputKeyboard();
		else
			HandleInputController();
	}

	private void HandleInputController() {
		//Calculate movement direction based on player input
		player.SetDirectionalInput(new Vector2(XCI.GetAxisRaw(XboxAxis.LeftStickX), XCI.GetAxisRaw(XboxAxis.LeftStickY)));

		//Check for jump input
		if (XCI.GetButton(XboxButton.A)) {
			player.OnJumpInputDown();
		}

		//Check if jump input has finished to see if jumping needs to be cancelled early
		if (XCI.GetButtonUp(XboxButton.A)) {
			player.OnJumpInputUp();
		}
	}

	private void HandleInputKeyboard() {
		//Calculate movement direction based on player input
		player.SetDirectionalInput(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));

		//Calculate where the mouse is

		//Check for jump input
		if (Input.GetKeyDown(KeyCode.Space)) {
			player.OnJumpInputDown();
		}

		//Check if jump input has finished to see if jumping needs to be cancelled early
		if (Input.GetKeyUp(KeyCode.Space)) {
			player.OnJumpInputUp();
		}
	}
	
}
