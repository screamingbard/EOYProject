using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerInput : MonoBehaviour {

    public bool riggedcontrolsXbox = false;

    bool KeyboardInput = true;

    public bool isMoving = false;

    public bool canInput = true;

    //Checks if the key was recently pressed
    int hasPressed = 0;
    //Checks if the button was just released
    public bool justReleased = false;

    [HideInInspector]
    public float reeling = 0;

    bool SingleShoot = false;

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

            if (Input.GetKeyDown(KeyCode.W))
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

            if (!canInput)
            {
                goPlayer.GetComponent<Player>().input.x = 0;
                goPlayer.GetComponent<Player>().input.y = 0;
            }

            //Assigns whether the player can reel or not
            if (Input.GetKeyDown(KeyCode.LeftShift))
                reeling = 1;
            else
                reeling = 0;

            if (Input.GetKeyDown(KeyCode.Space) && !SingleShoot)
            {
                goPlayer.GetComponentInChildren<ShootOBJ>().IsShooting = true;

                SingleShoot = true;
            }
            else if(Input.GetKeyUp(KeyCode.Space))
            {
                SingleShoot = false;
                goPlayer.GetComponentInChildren<ShootOBJ>().IsShooting = false;
            }
            else if (Input.GetKey(KeyCode.Space))
            {

                if (Input.GetKeyDown(KeyCode.LeftShift) && goPlayer.GetComponentInChildren<ShootOBJ>().IsShooting)
                {
                    goPlayer.GetComponentInChildren<ShootOBJ>().IsReeling = true;
                }
                else
                {
                    goPlayer.GetComponentInChildren<ShootOBJ>().IsReeling = false;
                }

                SingleShoot = true;
            }
        }
        
        //-------------------------------
        //Xbox Controller Input
        //-------------------------------
        if (KeyboardInput == false)
        {
                //PlayerMovement
                goPlayer.GetComponent<Player>().input = new Vector2(XCI.GetAxisRaw(XboxAxis.LeftStickX), XCI.GetAxisRaw(XboxAxis.LeftStickY));

                if (XCI.GetButton(XboxButton.LeftBumper) || XCI.GetButton(XboxButton.A))
                    goPlayer.GetComponent<Player>().CanJump = true;
                if (XCI.GetButtonUp(XboxButton.LeftBumper) || XCI.GetButtonUp(XboxButton.A))
                    goPlayer.GetComponent<Player>().CanJump = false;
            if (riggedcontrolsXbox)
            {
                goPlayer.GetComponentInChildren<FollowArrow>().JoystickStore.x = XCI.GetAxis(XboxAxis.RightStickX);
                goPlayer.GetComponentInChildren<FollowArrow>().JoystickStore.y = XCI.GetAxis(XboxAxis.RightStickY);
            }

            else
            {
                //Shooting
                goPlayer.GetComponentInChildren<FollowArrow>().JoystickStore.x = XCI.GetAxis(XboxAxis.LeftStickX);
                goPlayer.GetComponentInChildren<FollowArrow>().JoystickStore.y = XCI.GetAxis(XboxAxis.LeftStickY);
            }
            if (!canInput)
            {
                goPlayer.GetComponent<Player>().input.x = 0;
                goPlayer.GetComponent<Player>().input.y = 0;
            }

            //Assigns whether the player can reel or not
            reeling = XCI.GetAxis(XboxAxis.LeftTrigger);

            if (XCI.GetAxis(XboxAxis.RightTrigger) > 0)
            {
                goPlayer.GetComponentInChildren<ShootOBJ>().IsShooting = true;
                SingleShoot = true;
                hasPressed = 0;
                if (XCI.GetAxis(XboxAxis.LeftTrigger) > 0 && goPlayer.GetComponentInChildren<ShootOBJ>().IsShooting)
                {
                    goPlayer.GetComponentInChildren<ShootOBJ>().IsReeling = true;
                }
                else
                {
                    goPlayer.GetComponentInChildren<ShootOBJ>().IsReeling = false;
                }
            }
            else
            {
                goPlayer.GetComponentInChildren<ShootOBJ>().IsShooting = false;
                hasPressed++;
            }

            if (XCI.GetAxis(XboxAxis.RightTrigger) == 0)
                SingleShoot = false;

            if (hasPressed == 1)
            {
                justReleased = true;
            }
            else
            {
                justReleased = false;
            }
        }


    }
}
