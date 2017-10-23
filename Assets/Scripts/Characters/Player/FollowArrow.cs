using System.Collections;
using System.Collections.Generic;
using XboxCtrlrInput;
using UnityEngine;

public class FollowArrow : MonoBehaviour {

    public GameObject Parent;
    public Quaternion store;

    public bool Riggedcontrols = false;

    Vector3 JoystickStore = new Vector3();
	
	// Update is called once per frame
	void Update ()
    {
        if(Riggedcontrols == true)
        {
            JoystickStore.x = XCI.GetAxis(XboxAxis.RightStickX);
            JoystickStore.y = XCI.GetAxis(XboxAxis.RightStickY);
        }
        else
        {
            JoystickStore.x = XCI.GetAxis(XboxAxis.LeftStickX);
            JoystickStore.y = XCI.GetAxis(XboxAxis.LeftStickY);
        }

        //Shoots the grapple in the last given direction if no direction is given
        if (JoystickStore.x != 0 || JoystickStore.y != 0)
            transform.position = Parent.transform.position + JoystickStore;
        //else if(JoystickStore.x == 0 && JoystickStore.y == 0)
        //    transform.position = Parent.transform.position + Vector3.up;
    }
}
