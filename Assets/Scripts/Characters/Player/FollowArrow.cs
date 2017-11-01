using System.Collections;
using System.Collections.Generic;
using XboxCtrlrInput;
using UnityEngine;

public class FollowArrow : MonoBehaviour {

    public GameObject Parent;
    public Quaternion store;

    public float DistFromPlayer = 5.0f;

    Vector3 Dir = new Vector3();
    Vector3 storepos = new Vector3();

    [HideInInspector]
    public Vector3 JoystickStore = new Vector3();
	
	// Update is called once per frame
	void Update ()
    {
        if (JoystickStore.x != 0 || JoystickStore.y != 0)
        {
            transform.position = Parent.transform.position + (JoystickStore * DistFromPlayer);
            storepos = transform.position;
            Dir = (Parent.transform.position + JoystickStore);
        }
        //else if (JoystickStore.x == 0 || JoystickStore.y == 0)
        //    transform.position = storepos;

        Vector3 holdPos = Parent.transform.position + JoystickStore;// - transform.position;
                                                    //holdPos.Normalize();

        //Vector3 store = Parent.transform.position + (JoystickStore * 3);

        //Dir.Normalize();

        store = Quaternion.Euler(0, 0, Mathf.Atan2(holdPos.y, holdPos.x) * Mathf.Rad2Deg);

        if (Dir.x != 0 && Dir.y != 0)
            transform.LookAt(Dir);
        //else
            //transform.LookAt();

        transform.SetPositionAndRotation((Parent.transform.position + (JoystickStore * DistFromPlayer)), store);
    }
}
