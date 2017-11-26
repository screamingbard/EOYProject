using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// ------------- ///
// Micheal Corben
/// ------------- ///
public class PlaySound : MonoBehaviour {

    //The player tag
    public string m_sPlayerTag;

    //The sound to be played
    public AudioClip m_acAudioClip;

    //Will the sound be played more than once
    public bool m_bPlayMoreThanOnce;

    //The time between each time the sound plays
    public float m_fTimeBetweenSounds;

    //The counter that controls when the sound plays
    float m_fTimerBetweenSounds;

    //Works to start the sound and is set to the public variable after it plays once
    bool m_bPlayMoreThanOncePrivate = true;

    //
    public Options m_options;
    void start()
    {
        //Set the counter to the count
        m_fTimerBetweenSounds = m_fTimeBetweenSounds;
    }

    void Update()
    {
        //If this is the first pass through or the sound is going to play more than once
        if (m_bPlayMoreThanOncePrivate)
        {
            //If the timer is less than the time count up
            if (m_fTimerBetweenSounds < m_fTimeBetweenSounds)
            {
                m_fTimerBetweenSounds += Time.deltaTime;
            }
        }
    }
    AudioSource PlayClipAt(AudioClip a_acClip, Vector2 a_v2Position)
    //Custon play clip at point method that will play a sound for as long as the file is to allow for alert sounds when the player moves through a vision cone
    {
        //Create a temperary game object which will hold the sound and play it
        GameObject m_goTempGameObject = new GameObject("TempAudio");
        //Set the position of the game object
        m_goTempGameObject.transform.position = a_v2Position;
        //Add an audio source to the temperary game object
        AudioSource m_audioSource = m_goTempGameObject.AddComponent<AudioSource>();
        //Set the audio clip to be played
        m_audioSource.clip = a_acClip;
        //Play the stored audio clip
        m_audioSource.Play();
        //Destroy the temp game object after the tim of the audio clip
        Destroy(m_goTempGameObject, a_acClip.length);
        //return an audio scource
        return m_audioSource;
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
                    //If the Timer has counted up and the player has entered the trigger
                    m_fTimerBetweenSounds = 0;
                    //Play the sound
                    if (m_options.m_iSFXOn == 1)
                    {
                        //If the sound effects are turned on play them wih volume one
                        PlayClipAt(m_acAudioClip, transform.position).volume = 1;
                    }
                    else
                    {
                        //Otherwise if they are turned off play if but at volume zero
                        PlayClipAt(m_acAudioClip, transform.position).volume = 0;
                    }
                    m_bPlayMoreThanOncePrivate = m_bPlayMoreThanOnce;
                }
            }
        }
    }

}
