using System.Collections;
using System.Collections.Generic;
using XboxCtrlrInput;
using UnityEngine;

public class FollowArrow : MonoBehaviour {

    public GameObject Parent;
    public Quaternion store;
    float rotAngle = 0;

    Vector3 JoystickStore = new Vector3();
    Vector3 defaultshoot = new Vector3(0, 1, 0);
	
	// Update is called once per frame
	void Update ()
    {   
        JoystickStore.x = XCI.GetAxis(XboxAxis.RightStickX);
        JoystickStore.y = XCI.GetAxis(XboxAxis.RightStickY);

        //Shoots the grapple upwards if no direction is given
        if (JoystickStore.x == 0 && JoystickStore.y == 0)
            transform.position = Parent.transform.position +  defaultshoot;
        else
            transform.position = Parent.transform.position + JoystickStore;
    }
}
