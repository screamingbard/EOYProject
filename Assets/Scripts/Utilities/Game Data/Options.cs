using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour {

    //Turn music on and off
    public bool m_bMusicOnOff;

    //Turn SFX on and off
    public bool m_bSFXOnOff;
    
    void Awake()
    {
        m_bMusicOnOff = true;
        m_bSFXOnOff = true;
    }
}
