using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticle : MonoBehaviour {

    //
    public string m_sPlayerTag;

    //
    public ParticleSystem m_psParticleSystem;

    //
    public bool m_bPlayMoreThanOnce;
    
    //
    public float m_fTimeBetweenSpawns;

    //
    float m_fTimerBetweenSpawns;

    //
    bool m_bPlayMoreThanOncePrivate = true;

    void start()
    {
        //
        m_fTimerBetweenSpawns = m_fTimeBetweenSpawns;
    }

    void Update()
    {

        if (m_bPlayMoreThanOncePrivate)
        {
            if (m_fTimerBetweenSpawns >= m_fTimeBetweenSpawns)
            {
                m_fTimerBetweenSpawns = 0;
            }
            else
            {
                m_fTimerBetweenSpawns += Time.deltaTime;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D a_colCollider)
    {
        if (a_colCollider.gameObject.tag == m_sPlayerTag)
        {
            //If true will play the animation
            if (m_bPlayMoreThanOncePrivate)
            {
                if (m_fTimerBetweenSpawns >= m_fTimeBetweenSpawns)
                {
                    m_fTimerBetweenSpawns = 0;
                    m_psParticleSystem.Play();
                    m_bPlayMoreThanOncePrivate = m_bPlayMoreThanOnce;
                }
            }
        }
    }

}
