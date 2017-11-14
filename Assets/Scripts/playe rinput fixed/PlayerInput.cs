using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerInput : MonoBehaviour {

    public bool riggedcontrolsXbox = false;

    bool KeyboardInput = true;
    public bool isMoving = false;

    [HideInInspector]
    public Vector3 joystickStore = new Vector3(0, 1, 0);

    bool SingleShoot = false;

    PlayerMovement pMove;
    RopeSystem rSyst;

    //GameObject goPlayer;

    // Use this for initialization
    void Start () {
        //goPlayer = gameObject;
        pMove = GetComponent<PlayerMovement>();
        rSyst = GetComponent<RopeSystem>();
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
            pMove.horizontalInput = Input.GetAxisRaw("Horizontal");//new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            joystickStore.x = Input.GetAxisRaw("Horizontal");
            joystickStore.y = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(KeyCode.W))
                pMove.jumpInput = 1;
            if (Input.GetKeyUp(KeyCode.W))
                pMove.jumpInput = 0;

            //This checks if any input is used
            if (pMove.horizontalInput != 0)
                isMoving = true;
            else
                isMoving = false;

            if (Input.GetAxis("Jump") > 0)
                rSyst.IsShooting = true;
            else
                rSyst.IsShooting = false;
        }
        
        //-------------------------------
        //Xbox Controller Input
        //-------------------------------
        if (KeyboardInput == false)
        {
                
            
            //PlayerMovement
            pMove.horizontalInput = XCI.GetAxisRaw(XboxAxis.LeftStickX);

           

            if (XCI.GetButtonDown(XboxButton.LeftBumper) || XCI.GetButtonDown(XboxButton.A))
                    pMove.jumpInput = 1;
            if (XCI.GetButtonUp(XboxButton.LeftBumper) || XCI.GetButtonUp(XboxButton.A))
                    pMove.jumpInput = 0;

            if (riggedcontrolsXbox)
            {
                joystickStore.x = XCI.GetAxisRaw(XboxAxis.RightStickX);
                joystickStore.y = XCI.GetAxisRaw(XboxAxis.RightStickY);
            }
            else
            {
                joystickStore.x = XCI.GetAxisRaw(XboxAxis.LeftStickX);
                joystickStore.y = XCI.GetAxisRaw(XboxAxis.LeftStickY);
            }

            if (XCI.GetAxis(XboxAxis.RightTrigger) > 0)
                rSyst.IsShooting = true;
            else
                rSyst.IsShooting = false;

        }


    }
}
