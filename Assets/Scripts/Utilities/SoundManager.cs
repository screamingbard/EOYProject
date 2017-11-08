using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    //
    AudioSource m_asAudioSource;

    //
    public GameData m_gdGameData;

    //
    public AudioClip m_acMenuMusic;

    //
    public List<AudioClip> m_lacBGM;

    //
    public List<AudioClip> m_lacAmbientSounds;


	// Use this for initialization
	void Start () {
        m_asAudioSource.PlayOneShot(m_acMenuMusic, 0.5f/*m_gdGameData.m_setSettigs.m_fMusicVolume * m_gdGameData.m_setSettigs.m_fMasterVolume*/);
	}
	
	// Update is called once per frame
	void Update () {
        if (!m_asAudioSource.isPlaying)
        {
            m_asAudioSource.clip = m_lacAmbientSounds[Random.Range(0, m_lacAmbientSounds.Count)];
            m_asAudioSource.Play();
        }
	}
}
