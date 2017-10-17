using System.Collections;
using System.Collections.Generic;
using XboxCtrlrInput;
using UnityEngine;

public class FollowArrow : MonoBehaviour {

    public GameObject Parent;
    public Quaternion store;
    float rotAngle = 0;

    Vector3 JoystickStore = new Vector3();

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //store = Quaternion.Euler(0, 0, Mathf.Atan2(XCI.GetAxis(XboxAxis.RightStickY), XCI.GetAxis(XboxAxis.RightStickX)) * Mathf.Rad2Deg);
        //rotAngle = store.z;
        // Quaternion.Angle
        
        JoystickStore.x = XCI.GetAxis(XboxAxis.RightStickX);
        JoystickStore.y = XCI.GetAxis(XboxAxis.RightStickY);

        transform.position = Parent.transform.position + JoystickStore;

        //transform.RotateAround(Parent.transform.position, Vector3.forward, rotAngle);
	}
}
