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
        DontDestroyOnLoad (m_options);
    }

    public void OnToggleMusic()
    {
        if (m_tMusicMuteToggle)
        {
            m_options.m_iMusicOn = 1;
        }
        else
        {
            m_options.m_iMusicOn = 0;
        }
    }

    public void OnToggleSFX()
    {
        if (m_tSFXMuteToggle)
        {
            m_options.m_iSFXOn = 1;
        }
        else
        {
            m_options.m_iSFXOn = 0;
        }
    }
}
