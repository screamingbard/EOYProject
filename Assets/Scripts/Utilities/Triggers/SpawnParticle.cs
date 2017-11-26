using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// ------------- ///
// Micheal Corben
/// ------------- ///
public class SpawnParticle : MonoBehaviour {

    //The player tag
    public string m_sPlayerTag;

    //The particles
    public ParticleSystem m_psParticleSystem;

    //Will the particles play more than once
    public bool m_bPlayMoreThanOnce;
    
    //The time between spawns of the particles
    public float m_fTimeBetweenSpawns;

    //The counter for the time between spawns
    float m_fTimerBetweenSpawns;

    //Controls whether or not the particles will play
    bool m_bPlayMoreThanOncePrivate = true;

    void start()
    {
        //Set the timer to the time
        m_fTimerBetweenSpawns = m_fTimeBetweenSpawns;
    }

    void Update()
    {
        //If the particles haven't played or they will play more than once
        if (m_bPlayMoreThanOncePrivate)
        {
            //If the timer is less than the time count up
            if (m_fTimerBetweenSpawns < m_fTimeBetweenSpawns)
            {
                m_fTimerBetweenSpawns += Time.deltaTime;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D a_colCollider)
    {
        //If the player enters the trigger
        if (a_colCollider.gameObject.tag == m_sPlayerTag)
        {
            //If true will play the animation
            if (m_bPlayMoreThanOncePrivate)
            {
                //If the Timer has counted up to the time
                if (m_fTimerBetweenSpawns >= m_fTimeBetweenSpawns)
                {
                    //Play them particles
                    m_fTimerBetweenSpawns = 0;
                    m_psParticleSystem.Play();
                    m_bPlayMoreThanOncePrivate = m_bPlayMoreThanOnce;
                }
            }
        }
    }

}
