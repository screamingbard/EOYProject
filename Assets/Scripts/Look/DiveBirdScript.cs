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
    Vector2 v2PlayerDirection = new Vector2();

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

    //The timer variable that is stored
    float timer = 0;

    //This checks to see if the timer should be enabled/disabled
    bool StartTiming = false;

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
        if(this.gameObject.activeInHierarchy == false && timer >= 2)
        {
            StartTiming = false;
            timer = 0;
            this.gameObject.SetActive(true);
        }
        if (StartTiming)
        {
            timer += Time.deltaTime;
        }
        //Gets the direction it should fly in
        v2PlayerDirection = dkpScript.GetComponent<DiveKillPlayer>().playerDirection;
        //Moves the player towards the desired location, preferably fast
        transform.position += (Vector3)v2PlayerDirection * fDiveSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D col2d)
    {
        if(col2d.gameObject.tag == playerTag)
        {
            this.gameObject.SetActive(false);
            StartTiming = true;
        }
    }

}
