using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class GameData : MonoBehaviour
{
    //GameSettings
    [HideInInspector]
    public Settings m_setSettigs;

    //Player info
    [HideInInspector]
    public float m_fSpeedRunTimer;

    [HideInInspector]
    public float m_fFastestTime;
    
    [HideInInspector]
    public float m_fSecondFastestTime;

    [HideInInspector]
    public float m_fThirdFastestTime;

    [HideInInspector]
    public float m_fFourthFastestTime;

    [HideInInspector]
    public float m_fFifthFastestTime;

    [HideInInspector]
    public Transform m_tfPlayerPosiion;

    //
    [HideInInspector]
    public Transform m_tfLastCheckPoint;

    public struct Controls
    //Stores the controls
    {
        bool m_bSeperateAimAndMove;
        XboxButton m_inJump;
        XboxButton m_inShootGrapple;
        XboxAxis m_xaMovement;
        XboxAxis m_xaAiming;
        XboxAxis m_xaReeling;

        Controls(XboxButton a_inJump, XboxButton a_inShootGrapple, bool a_bSeperateAimAndMove, XboxAxis a_xaMovement, XboxAxis a_xaAiming, XboxAxis a_xaReeling)
        {
            m_inJump = a_inJump;
            m_inShootGrapple = a_inShootGrapple;
            m_bSeperateAimAndMove = a_bSeperateAimAndMove;
            m_xaMovement = a_xaMovement;
            m_xaAiming = a_xaAiming;
            m_xaReeling = a_xaReeling;
        }
    };

    public struct Settings
    //Stores only the controls
    {

        //Sound settings
        float m_fMasterVolume;
        float m_fMusicVolume;
        float m_fSFXVolume;
        float m_fAmbienceVolume;
        //Control settings
        Controls m_conControls;

        Settings(float a_fMasterVolume, float a_fMusicVolume, float a_fSFXVolume, float a_AmbienceVolume, Controls a_conControls)
        {
            m_fMasterVolume = a_fMasterVolume;
            m_fMusicVolume = a_fMusicVolume;
            m_fSFXVolume = a_fSFXVolume;
            m_fAmbienceVolume = a_AmbienceVolume;
            m_conControls = a_conControls;
        }
    };

    public GameData(Settings a_setSettigs, float a_fSpeedRunTimer, Transform a_tfPlayerPosition, Transform a_tfLastCheckPoint, 
        float a_fFastestTime, float a_fSecondFastestTime, float a_fThirdFastestTime, float a_fFourthFastestTime, float a_fFifthFastestTime)
    //Constructor for the GameData
    {
        m_setSettigs = a_setSettigs;
        m_fSpeedRunTimer = a_fSpeedRunTimer;
        m_tfPlayerPosiion = a_tfPlayerPosition;
        m_tfLastCheckPoint = a_tfLastCheckPoint;
        m_fFastestTime = a_fFastestTime;
        m_fSecondFastestTime = a_fSecondFastestTime;
        m_fThirdFastestTime = a_fThirdFastestTime;
        m_fFourthFastestTime = a_fFourthFastestTime;
        m_fFifthFastestTime = a_fFifthFastestTime;
    }
}