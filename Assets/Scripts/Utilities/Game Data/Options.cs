using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Options : MonoBehaviour {

    //Turn music on and off
    public int m_iMusicOn;

    //Turn SFX on and off
    public int m_iSFXOn;
    
    
    void Awake()
    {
        //When the game starts set the sound settings to on
        m_iMusicOn = 1;
        m_iSFXOn = 1;
    }

    public void SaveSettings()
    {
        //When called save the settings into to the player prefs
        PlayerPrefs.SetInt("Music", m_iMusicOn);

        PlayerPrefs.SetInt("SFX", m_iSFXOn);

    }
    public void LoadSettings()
    {
        //When called load the settings from the player prefs
        m_iMusicOn = PlayerPrefs.GetInt("Music");

        m_iSFXOn = PlayerPrefs.GetInt("SFX");
    }
}
