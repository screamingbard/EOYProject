﻿using System.Collections;
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
    Vector2 playerDirection =  new Vector2();

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
    [HideInInspector]
    private GameObject StoreBird;

    //--------------------------------------
    //Birds speed
    //--------------------------------------
    public float birdSpeed = 30.0f;

    //--------------------------------------
    //references the bird creation Script.
    //--------------------------------------
    BirdbackgroundSummon bSummon;

    //--------------------------------------
    //The players tag, sghould be assigned.
    //--------------------------------------
    public string playerTag;

    //-------------------------------------------
    //vision cones tag to find all vision cones.
    //-------------------------------------------
    public string visionConetag;

    //--------------------------------------------------------------------------------------------
    //Checks if the vision cone is detecting and keeps the vision cone in a loop until it is done.
    //--------------------------------------------------------------------------------------------
    bool IsDetected = false;

    //Vision Cone death calculations
    VisionCone[] vCone;
    //PlayerRespawn pRespawn;
    float Counter = 0;
    public float delayBeforeDeath = 1.0f;

    void Awake()
    {
        //reference to the bird summoning script
        bSummon = GetComponent<BirdbackgroundSummon>();
        
        //finds the player based on tag
        player = GameObject.FindGameObjectWithTag(playerTag);
        
        //gets the vision cone script
        vCone = GameObject.FindGameObjectsWithTag(visionConetag);
        
        //Reference to the respawn script
        pRespawn = GetComponent<PlayerRespawn>();

    }

    void Update()
    {
        //loops through all vision cones and checks if any of the are meant to have killed the player
        for (int i = 0; i < vCone.Length; i++)
        {
            //checks if the player was killed by a particular vision cone
            if (vCone[i].m_bKilledByVisionCone)
            {
                //forces the while loop to activate if it is colliding
                IsDetected = true;
                while (IsDetected) {
                    //Allows for the bird to swoop down before killing the player
                    if (Counter < delayBeforeDeath)
                    {
                        //makes the bird dive down
                        Dive();

                        //Checks if the bird is existant and if it is it shoudl move towards the player
                        if (StoreBird != null)
                        {
                            StoreBird.transform.position += (Vector3)playerDirection * birdSpeed * Time.deltaTime;
                        }
                        //Increments the timer
                        Counter += Time.deltaTime;
                    }
                    else
                    {
                        //refreshes timer
                        Counter = 0;
                        //Calls the respawn script
                        pRespawn.Respawn();
                        //refreshes the IsDetected variable in order to stop infinate loops
                        IsDetected = false;
                    }
                }
            }
        }
    }

    //Bird dives down when this is called
    public void Dive()
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
            StoreBird.transform.position = gameObject.transform.position;
            StoreBird.transform.rotation = direction;
        }
    }
}
