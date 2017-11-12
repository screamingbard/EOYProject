using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    //
    public AudioSource m_asAmbientAudioSource;

    //
    public AudioSource m_asMusicAudioSource;

    //
    public GameData m_gdGameData;

    //
    public AudioClip m_acMenuMusic;

    //
    public List<AudioClip> m_lacBGM;

    //
    public List<AudioClip> m_lacAmbientSounds;

    
	void Start () {
        m_asAmbientAudioSource.PlayOneShot(m_acMenuMusic, 0.5f/*m_gdGameData.m_setSettigs.m_fMusicVolume * m_gdGameData.m_setSettigs.m_fMasterVolume*/);
	}
	

	void Update () {
        if (!m_asAmbientAudioSource.isPlaying)
        {
            m_asAmbientAudioSource.clip = m_lacAmbientSounds[Random.Range(0, m_lacAmbientSounds.Count)];
            m_asAmbientAudioSource.volume = 0.5f /*m_gdGameData.m_setSettings.m_fMusicVolume * m_gdGameData.m_setSettings.m_fMasterVolume*/;
            m_asAmbientAudioSource.Play();
        }
	}
}
