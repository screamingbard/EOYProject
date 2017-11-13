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

    
	void Start () {

        DontDestroyOnLoad(m_options);
        if (m_options.m_bMusicOnOff)
        {
            m_asAmbientAudioSource.PlayOneShot(m_acMenuMusic, 0.5f);
        }
        else
        {
            m_asAmbientAudioSource.PlayOneShot(m_acMenuMusic, 0);
        }
	}
	

	void Update () {
        if (!m_asAmbientAudioSource.isPlaying)
        {
            if (m_options.m_bMusicOnOff)
            {
                m_asAmbientAudioSource.clip = m_lacAmbientSounds[Random.Range(0, m_lacAmbientSounds.Count)];
                m_asAmbientAudioSource.volume = 0.5f;
                m_asAmbientAudioSource.Play();
            }
            else
            {

                m_asAmbientAudioSource.clip = m_lacAmbientSounds[Random.Range(0, m_lacAmbientSounds.Count)];
                m_asAmbientAudioSource.volume = 0;
                m_asAmbientAudioSource.Play();
            }
        }
	}
}
