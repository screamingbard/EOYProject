using System.Collections;
using System.Collections.Generic;
using XboxCtrlrInput;
using UnityEngine;

public class FollowArrow : MonoBehaviour {

    public GameObject Parent;
    public Quaternion store;

    Vector3 JoystickStore = new Vector3();
	
	// Update is called once per frame
	void Update ()
    {   
        JoystickStore.x = XCI.GetAxis(XboxAxis.RightStickX);
        JoystickStore.y = XCI.GetAxis(XboxAxis.RightStickY);

        //Shoots the grapple in the last given direction if no direction is given
        if(JoystickStore.x != 0 || JoystickStore.y != 0)
            transform.position = Parent.transform.position + JoystickStore;
    }
}
