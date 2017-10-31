using System.Collections;
using System.Collections.Generic;
using XboxCtrlrInput;
using UnityEngine;

public class FollowArrow : MonoBehaviour {

    public GameObject Parent;
    public Quaternion store;

    [HideInInspector]
    public Vector3 JoystickStore = new Vector3();
	
	// Update is called once per frame
	void Update ()
    {
        if (JoystickStore.x != 0 || JoystickStore.y != 0)
            transform.position = Parent.transform.position + JoystickStore * 3;

        Vector3 holdPos = Parent.transform.position - transform.position;
        holdPos.Normalize();

        Quaternion pos = new Quaternion();
        transform.LookAt(transform.position + JoystickStore);
        

        //float rot = Quaternion.EulerAngles();
        //pos.x = holdPos.x;
        //pos.y = holdPos.y;
        //pos.z = holdPos.z;
        //transform.rotation = pos;
    }
}
