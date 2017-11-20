using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableParticleSystem : MonoBehaviour {

    public ParticleSystem psParticleReference;

    void Awake()
    {
        psParticleReference = GetComponent<ParticleSystem>();
        ParticleSystem.EmissionModule isEmmiting = psParticleReference.emission;
        isEmmiting.enabled = true;
    }

    void OntriggerEnter2D(Collision2D collision2d)
    {
        if(collision2d.gameObject.tag == "Player")
        {
            ParticleSystem.EmissionModule isEmmiting = psParticleReference.emission;
            isEmmiting.enabled = false;
        }
    }
}
