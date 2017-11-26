using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+
//This scruip belongs on the bird that 
//will dive down and kill the player.
//
//On the GameObject:
//  - It will require a tag on the object that spawns this
//  - The Object MUST have a collider on it in order to check whether it shoudl respawn or not
//=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+

public class DiveBirdScript : MonoBehaviour {

    //-----------------------------
    //The Players Direction.
    //-----------------------------
    Vector2 v2PlayerDirection = new Vector2();

    //--------------
    //Diving Speed.
    //--------------
    float fDiveSpeed = 30.0f;

    //-----------------------------------------
    //Holds the tags for the divebird to use.
    //-----------------------------------------
    public string managertag = "SwooperSpawner";

    GameObject dkpScript;

    void Awake()
    {
        //Gets access to the manager for this object
        dkpScript = GameObject.FindGameObjectWithTag(managertag);
    }

    //---------------------------------------------------------------------------------
    //This moves he player in the same direction as the given direction of the player.
    //---------------------------------------------------------------------------------
    void FixedUpdate()
    {
        //Gets the direction it should fly in
        v2PlayerDirection = dkpScript.GetComponent<DiveKillPlayer>().playerDirection;
        //Moves the player towards the desired location, preferably fast
        transform.position += (Vector3)v2PlayerDirection * fDiveSpeed * Time.deltaTime;
    }

}
