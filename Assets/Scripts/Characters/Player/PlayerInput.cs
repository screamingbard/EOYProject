using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerInput : MonoBehaviour {

    bool KeyboardInput = true;

    public bool isMoving = false;

    GameObject goPlayer;

	// Use this for initialization
	void Start () {
        goPlayer = gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
        //-------------------------------------------
        //Checks if the controllers are plugged in.
        //-------------------------------------------
        if(XCI.GetNumPluggedCtrlrs() == 0)
            KeyboardInput = true;
        else
            KeyboardInput = false;


        //-------------------------------
        //keyboard Controller Input
        //-------------------------------
        if (KeyboardInput == true)
        {
            //PlayerMovement
            goPlayer.GetComponent<Player>().input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if(Input.GetKeyDown(KeyCode.W))
                goPlayer.GetComponent<Player>().CanJump = true;
            if (Input.GetKeyUp(KeyCode.W))
                goPlayer.GetComponent<Player>().CanJump = false;
            //Shooting
            goPlayer.GetComponentInChildren<FollowArrow>().JoystickStore.x = Input.GetAxis("Horizontal");
            goPlayer.GetComponentInChildren<FollowArrow>().JoystickStore.y = Input.GetAxis("Vertical");

            if (goPlayer.GetComponent<Player>().input.x != 0 || goPlayer.GetComponent<Player>().input.y != 0)
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                goPlayer.GetComponentInChildren<ShootOBJ>().IsShooting = true;
            }
            else
            {
                goPlayer.GetComponentInChildren<ShootOBJ>().IsShooting = false;
            }
        }
        
        //-------------------------------
        //Xbox Controller Input
        //-------------------------------
        if (KeyboardInput == false)
        {
            //PlayerMovement
            goPlayer.GetComponent<Player>().input = new Vector2(XCI.GetAxisRaw(XboxAxis.LeftStickX), XCI.GetAxisRaw(XboxAxis.LeftStickY));

            if (XCI.GetButton(XboxButton.RightBumper) || XCI.GetButton(XboxButton.A))
                goPlayer.GetComponent<Player>().CanJump = true;
            if (XCI.GetButtonUp(XboxButton.RightBumper) || XCI.GetButtonUp(XboxButton.A))
                goPlayer.GetComponent<Player>().CanJump = false;

            //Shooting
            goPlayer.GetComponentInChildren<FollowArrow>().JoystickStore.x = XCI.GetAxis(XboxAxis.LeftStickX);
            goPlayer.GetComponentInChildren<FollowArrow>().JoystickStore.y = XCI.GetAxis(XboxAxis.LeftStickY);

            if (XCI.GetAxis(XboxAxis.RightTrigger) > 0)
            {
                goPlayer.GetComponentInChildren<ShootOBJ>().IsShooting = true;
            }
            else
            {
                goPlayer.GetComponentInChildren<ShootOBJ>().IsShooting = false;
            }
        }


    }
}
