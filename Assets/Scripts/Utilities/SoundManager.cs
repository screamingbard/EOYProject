using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    //
    public AudioSource m_asAmbientAudioSource;

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
        m_asAmbientAudioSource.PlayOneShot(m_acMenuMusic, 0.5f/*m_gdGameData.m_setSettigs.m_fMusicVolume * m_gdGameData.m_setSettigs.m_fMasterVolume*/);
	}
	
	// Update is called once per frame
	void Update () {
        if (!m_asAmbientAudioSource.isPlaying)
        {
            m_asAmbientAudioSource.clip = m_lacAmbientSounds[Random.Range(0, m_lacAmbientSounds.Count)];
            m_asAmbientAudioSource.Play();
        }
	}
}
