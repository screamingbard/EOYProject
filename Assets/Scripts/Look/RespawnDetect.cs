using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//+=+=+=+=+=+=+=+=+=+=+
//Edward Ladyzhenskii
//+=+=+=+=+=+=+=+=+=+=+

//+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
//This class makes the respawn points detect and spawn only one bird as the player 
//gets to each point.
//
//This script should be put onto the "respawn points" prefab 
//with a 2D box collider that has OnTrigger active.
//+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
public class RespawnDetect : MonoBehaviour {

    //Accesses this script in order to add more birds as the player colides
    BirdbackgroundSummon bSummon;

    //variable to check if the spawn can still create a bird
    private bool CanSummon = true;

    void Awake()
    {
        //presets the variables, allows for resetting of the scene
        CanSummon = true;
        bSummon = GetComponentInParent<BirdbackgroundSummon>();
    }

    //Checks for a trigger in order to allow for spawning more birds
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == bSummon.playerTag && CanSummon)
        {
            //Accesses the variable and allows for spawning of the next bird
            bSummon.AddBird();
            //Makes sure that the spawn points do not continue to create more spawn points
            CanSummon = false;
        }
    }
}
