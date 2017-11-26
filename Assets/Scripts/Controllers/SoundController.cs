using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// ------------- ///
// Micheal Corben
/// ------------- ///
public class SoundController : MonoBehaviour {

    //The audio scource for the ambient sounds
    public AudioSource m_asAmbientAudioSource;

    //The audio scource for the music
    public AudioSource m_asMusicAudioSource;

    //Reference to the options to get if the sounds are on or off
    public Options m_options;

    //The main menu specific music
    public AudioClip m_acMenuMusic;

    //List of background musics
    public List<AudioClip> m_lacBGM;

    //List of ambient sounds
    public List<AudioClip> m_lacAmbientSounds;

    //Check for if ambient sounds are playing
    public bool m_bAmbientSoundsOn;

    
	void Start () {
        //Make sure that the options aren't destroyed on a scene change
        DontDestroyOnLoad(m_options);
        if (m_options.m_iMusicOn == 1)
        {
            //Play menu music
            m_asAmbientAudioSource.PlayOneShot(m_acMenuMusic, 1.0f);
        }
        else
        {
            //If the music is turned off in the settings set the volume of the menu music to zero
            m_asAmbientAudioSource.PlayOneShot(m_acMenuMusic, 0);
        }
	}
	

	void Update () {
        //If the ambients sounds bool is true play ambient sounds in a random loop
        if (m_bAmbientSoundsOn)
        {
            //If there is no ambient sound playing
            if (!m_asAmbientAudioSource.isPlaying)
            {
                //Randomly get an amibient sound from the list and play it
                m_asAmbientAudioSource.clip = m_lacAmbientSounds[Random.Range(0, m_lacAmbientSounds.Count)];
                m_asAmbientAudioSource.Play();
            }
            if (m_options.m_iMusicOn == 1)
            {
                //If the music is set to on in the menu make sure the volume is 1
                m_asAmbientAudioSource.volume = 1.0f;
            }
            else
            {
                //Otherwise make sure the volume is 0
                m_asAmbientAudioSource.volume = 0.0f;
            }
        }
        //Play music in a rondom loop
        //If the is no music paying
        if (!m_asMusicAudioSource.isPlaying)
        {
            //Play some music
            m_asMusicAudioSource.clip = m_lacBGM[Random.Range(0, m_lacBGM.Count)];
            m_asMusicAudioSource.Play();
        }
        if (m_options.m_iMusicOn == 1)
        {
            //If the music is set to on in the menu make sure the volume is 1
            m_asMusicAudioSource.volume = 1.0f;
        }
        else
        {
            //Otherwise make sure the volume is 0
            m_asMusicAudioSource.volume = 0.0f;
        }
    }
}
