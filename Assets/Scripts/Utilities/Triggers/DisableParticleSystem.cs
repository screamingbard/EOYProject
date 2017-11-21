using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableParticleSystem : MonoBehaviour {

    //A public reference particle system variable
    public ParticleSystem psParticleReference;

    void Awake()
    {
        //Get the particle system from the reference
        psParticleReference = GetComponent<ParticleSystem>();
        //Get a reference to the emission module of the particle system
        ParticleSystem.EmissionModule isEmmiting = psParticleReference.emission;
        //Set the emission module to enabled
        isEmmiting.enabled = true;
    }

    void OntriggerEnter2D(Collision2D collision2d)
    {
        //If the player enters the trigger
        if(collision2d.gameObject.tag == "Player")
        {
            //Set the emission mudule to disabled
            ParticleSystem.EmissionModule isEmmiting = psParticleReference.emission;
            isEmmiting.enabled = false;
        }
    }
}
