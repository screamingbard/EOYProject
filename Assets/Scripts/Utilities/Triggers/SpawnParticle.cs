using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// ------------- ///
// Micheal Corben
/// ------------- ///
public class SpawnParticle : MonoBehaviour {

    //The player tag
    public string m_sPlayerTag;
    
    //Play one shot?
    public bool m_bPlayEachHit = false;

    //Reference to the gameobject controlling the particles
    public GameObject m_goParticles;

    private GameObject spawnedParticle;
    void Awake()
    {
        spawnedParticle = Instantiate(m_goParticles, transform);
        spawnedParticle.SetActive(false);
    }

    void Update()
    {
        if (m_bPlayEachHit)
        {
            if (spawnedParticle.activeInHierarchy)
            {
                if (!spawnedParticle.GetComponent<ParticleSystem>().isPlaying)
                {
                    spawnedParticle.SetActive(false);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D a_colCollider)
    {
        //If the player enters the trigger
        if (a_colCollider.gameObject.tag == m_sPlayerTag)
        {
            if (spawnedParticle != null)
            {
                spawnedParticle.SetActive(true);
                spawnedParticle.transform.position = transform.position;
            }
        }
    }

}
