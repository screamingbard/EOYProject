using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//+=+=+=+=+=+=+=+=+=+=+
//Edward Ladyzhenskii
//+=+=+=+=+=+=+=+=+=+=+

//=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+
//This script belongs on the bird that 
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
    Vector3 v3PlayerDirection = new Vector3();

    //--------------
    //Diving Speed.
    //--------------
    public float fDiveSpeed = 30.0f;

    //-----------------------------------------
    //Holds the tags for the divebird to use.
    //-----------------------------------------
    public string managertag = "SwooperSpawner";

    //-------------------------------------------------------------------------
    //The players tag so that the bird can stiop drawing after a cetain point.
    //-------------------------------------------------------------------------
    public string playerTag = "Player";

    //-----------------------------------
    //Reference to the dive bird script.
    //-----------------------------------
    GameObject dkpScript;

    void Awake()
    {
        //Gets access to the manager for this object
        dkpScript = GameObject.FindGameObjectWithTag(managertag);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    //---------------------------------------------------------------------------------
    //This moves he player in the same direction as the given direction of the player.
    //---------------------------------------------------------------------------------
    void FixedUpdate()
    {
        //Gets the direction it should fly in
        v3PlayerDirection = dkpScript.GetComponent<DiveKillPlayer>().v2StorePlayerDirection;
        //Moves the player towards the desired location, preferably fast
        transform.position += v3PlayerDirection * fDiveSpeed * Time.deltaTime;
    }

    //--------------------------------------------------------------------
    //makes sure that the bird disappears as it collides with the player.
    //--------------------------------------------------------------------
    void OnTriggerEnter2D(Collider2D col2d)
    {
        if(col2d.gameObject.tag == playerTag)
        {
            //deactivates the bird as soon as it colldies with the payer
            this.gameObject.SetActive(false);
        }
    }

}
