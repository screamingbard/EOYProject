using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

    //Toggle the music
    public Toggle m_tMusicMuteToggle;

    //Toggle the SFX
    public Toggle m_tSFXMuteToggle;
    
    //Public options variable
    public Options m_options;

    public void OnEnable()
    {
        //Set the options to the saved player prefs
        m_options.LoadSettings();

        //If the music option is set to one
        if (m_options.m_iMusicOn == 1)
        {
            //Set the music to on
            m_tMusicMuteToggle.isOn = true;
        }
        else
        {
            //Otherwise set the music to off
            m_tMusicMuteToggle.isOn = false;
        }
        //If the SFX option is set to one
        if (m_options.m_iSFXOn == 1)
        {
            //Set the SFX to on
            m_tSFXMuteToggle.isOn = true;
        }
        else
        {
            //Otherwise set the SFX to off
            m_tSFXMuteToggle.isOn = false;
        }
    }

    public void OnToggleMusic()
    {
        //Check the toggles for the music being on or off and then assign the correct value
        if (m_tMusicMuteToggle.isOn)
        {
            m_options.m_iMusicOn = 1;
        }
        else
        {
            m_options.m_iMusicOn = 0;
        }
        m_options.SaveSettings();
    }

    public void OnToggleSFX()
    {
        //Check the toggles for the SFX being on or off and then assign the correct value
        if (m_tSFXMuteToggle.isOn)
        {
            m_options.m_iSFXOn = 1;
        }
        else
        {
            m_options.m_iSFXOn = 0;
        }
        m_options.SaveSettings();
    }
}
