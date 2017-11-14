using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    //
    public AudioSource m_asAmbientAudioSource;

    //
    public AudioSource m_asMusicAudioSource;

    //
    public Options m_options;

    //
    public AudioClip m_acMenuMusic;

    //
    public List<AudioClip> m_lacBGM;

    //
    public List<AudioClip> m_lacAmbientSounds;

    //
    public bool m_bAmbientSoundsOn;

    
	void Start () {

        DontDestroyOnLoad(m_options);
        if (m_options.m_iMusicOn == 1)
        {
            m_asAmbientAudioSource.PlayOneShot(m_acMenuMusic, 0.5f);
        }
        else
        {
            m_asAmbientAudioSource.PlayOneShot(m_acMenuMusic, 0);
        }
	}
	

	void Update () {
        //If the ambients sounds bool is true play ambient sounds is a random loop
        if (m_bAmbientSoundsOn)
        {
            if (!m_asAmbientAudioSource.isPlaying)
            {
                m_asAmbientAudioSource.clip = m_lacAmbientSounds[Random.Range(0, m_lacAmbientSounds.Count)];
                m_asAmbientAudioSource.Play();
            }
            if (m_options.m_iMusicOn == 1)
            {
                m_asAmbientAudioSource.volume = 0.5f;
            }
            else
            {
                m_asAmbientAudioSource.volume = 0;
            }
        }
        //Play music in a rondom loop
        if (!m_asMusicAudioSource.isPlaying)
        {
            m_asMusicAudioSource.clip = m_lacBGM[Random.Range(0, m_lacBGM.Count)];
            m_asMusicAudioSource.Play();
        }
        if (m_options.m_iMusicOn == 1)
        {
            m_asMusicAudioSource.volume = 0.5f;
        }
        else
        {
            m_asMusicAudioSource.volume = 0;
        }
    }
}
