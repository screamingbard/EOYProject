using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

    //Toggle the music
    public Toggle m_tMusicMuteToggle;

    //Toggle the SFX
    public Toggle m_tSFXMuteToggle;
    
    //
    public Options m_options;

    public void OnEnable()
    {
        m_options.LoadSettings();

        if (m_options.m_iMusicOn == 1)
        {
            m_tMusicMuteToggle.isOn = true;
        }
        else
        {
            m_tMusicMuteToggle.isOn = false;
        }
        if (m_options.m_iSFXOn == 1)
        {
            m_tSFXMuteToggle.isOn = true;
        }
        else
        {
            m_tSFXMuteToggle.isOn = false;
        }
    }

    public void OnToggleMusic()
    {
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
