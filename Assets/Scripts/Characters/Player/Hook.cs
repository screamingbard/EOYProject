using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour {

    public GameObject hook;
    GameObject goCurrentHook;

    public bool ropeActive;
    int ListCount;

    Vector2 v2MousePos;
    Vector2 v2PlayerPos;
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            if (ropeActive == false)
            {
                v2MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                v2PlayerPos = new Vector2(transform.position.x, transform.position.y);

                goCurrentHook = Instantiate(hook, v2PlayerPos, Quaternion.identity);
                goCurrentHook.GetComponent<RopeScript>().v2Point = v2MousePos;

                ropeActive = true;
                ListCount = goCurrentHook.GetComponent<RopeScript>().lgoRopeNodes.Count;
            }
            else
            {
                //delete Rope
                Destroy(goCurrentHook);
                ropeActive = false;
                ListCount = 0;
            }
        }
        if (Input.GetMouseButton(1) && ropeActive)
        {
            int Offset = 0;
            transform.position = Vector2.MoveTowards(transform.position, goCurrentHook.GetComponent<RopeScript>().lgoRopeNodes[ListCount - 1 - Offset].transform.position, 5.0f * Time.deltaTime);
            if (transform.position == goCurrentHook.GetComponent<RopeScript>().lgoRopeNodes[ListCount - 1 - Offset].transform.position)
                Offset++;

            //goCurrentHook.GetComponent<RopeScript>().lgoRopeNodes[ListCount - 1].transform.position
        }
    }
}
