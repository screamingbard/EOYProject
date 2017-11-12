using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {

    //
    public string m_sPlayerTag;

    //
    public AudioClip m_acAudioClip;

    //
    public bool m_bPlayMoreThanOnce;

    //
    public float m_fTimeBetweenSounds;

    //
    float m_fTimerBetweenSounds;

    //
    bool m_bPlayMoreThanOncePrivate = true;

    //
    public GameData m_gdGameData;
    void start()
    {
        //
        m_fTimerBetweenSounds = m_fTimeBetweenSounds;
    }

    void Update()
    {

        if (m_bPlayMoreThanOncePrivate)
        {
            if (m_fTimerBetweenSounds >= m_fTimeBetweenSounds)
            {
                m_fTimerBetweenSounds = 0;
            }
            else
            {
                m_fTimerBetweenSounds += Time.deltaTime;
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
                if (m_fTimerBetweenSounds >= m_fTimeBetweenSounds)
                {
                    m_fTimerBetweenSounds = 0;
                    AudioSource.PlayClipAtPoint(m_acAudioClip, transform.position, 0.5f/*m_gdGameData.m_setSettigs.m_fMasterVolume * m_gdGameData.m_setSettigs.m_fSFXVolume*/);
                    m_bPlayMoreThanOncePrivate = m_bPlayMoreThanOnce;
                }
            }
        }
    }

}
