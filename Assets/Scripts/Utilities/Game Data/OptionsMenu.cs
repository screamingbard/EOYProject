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
    public Options m_optionsOptions;

    public void OnEnable()
    {
        m_optionsOptions = new Options();
        DontDestroyOnLoad (m_optionsOptions);
    }

    public void OnToggleMusic()
    {
            m_optionsOptions.m_bMusicOnOff = m_tMusicMuteToggle;
    }

    public void OnToggleSFX()
    {
            m_optionsOptions.m_bSFXOnOff = m_tSFXMuteToggle;
    }
}
