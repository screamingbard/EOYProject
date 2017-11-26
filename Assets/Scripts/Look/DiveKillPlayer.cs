using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//------------------------------------------------------------
//This script makes a bird dive down and kill the player.
//
//this script should be attached to an emty game object that
//sits just above the camera so that the bird spawns off map
//and flys off.
//Requirements:
// - Empty Gameobject
//
//------------------------------------------------------------

public class DiveKillPlayer : MonoBehaviour {

    //--------------------------------------
    //finds the direction towards the player
    //from the shoot point. 
    //--------------------------------------
    [HideInInspector]
    public Vector2 playerDirection =  new Vector2();

    //--------------------------------------
    //reference to the player.
    //Finds player based on tag
    //--------------------------------------
    GameObject player;

    //--------------------------------------
    //holds the prefab for the bird.
    //--------------------------------------
    public GameObject birdPrefab;

    //--------------------------------------
    //Stores this for future use.
    //--------------------------------------
    private GameObject StoreBird;

    //--------------------------------------
    //references the bird creation Script.
    //--------------------------------------
    BirdbackgroundSummon bSummon;

    //--------------------------------------
    //The players tag, sghould be assigned.
    //--------------------------------------
    public string playerTag;

    void Awake()
    {
        //finds the player based on tag
        player = GameObject.FindGameObjectWithTag(playerTag);
    }

    //------------------
    //Creates the bird.
    //------------------
    public void CreateBird()
    {
        //This gets the plaeyrs direction and normalises it
        playerDirection = player.transform.position - gameObject.transform.position;
        playerDirection.Normalize();

        //gets the direction as a quaternion
        Quaternion direction = Quaternion.Euler(0, 0, Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg);
        //instantiates a bird if none exist
        if (StoreBird == null)
            StoreBird = Instantiate(birdPrefab, gameObject.transform.position, direction);
        else
        {
            //teleports the existant bird to the dive location and gives it an angle to dive down at
            StoreBird.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0);
            StoreBird.transform.rotation = direction;
        }
    }
}
