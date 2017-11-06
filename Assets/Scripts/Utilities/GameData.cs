using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class GameData : MonoBehaviour
{
    //Players last checkpoint
    public Transform m_tfLastCheckPoint;

    [System.Serializable]
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

    [System.Serializable]
    public struct Settings
    //Stores only the controls
    {

        //Sound settings
        [Range(0,1)]
        public float m_fMasterVolume;
        [Range(0, 1)]
        public float m_fMusicVolume;
        [Range(0, 1)]
        public float m_fSFXVolume;
        [Range(0, 1)]
        public float m_fAmbienceVolume;
        //Control settings
        public Controls m_conControls;

        public Settings(float a_fMasterVolume, float a_fMusicVolume, float a_fSFXVolume, float a_AmbienceVolume, Controls a_conControls)
        {
            m_fMasterVolume = a_fMasterVolume;
            m_fMusicVolume = a_fMusicVolume;
            m_fSFXVolume = a_fSFXVolume;
            m_fAmbienceVolume = a_AmbienceVolume;
            m_conControls = a_conControls;
        }
    };

[System.Serializable]
    public struct GameDataS
    {

        //GameSettings
        public Settings m_setSettigs;

        //Current time taken to get through the level
        public float m_fSpeedRunTimer;

        //The current fastest time to complete a level
        public float m_fFastestTime;

        //The current second fastest time to complete a level
        public float m_fSecondFastestTime;

        //The current third fastest time to complete a level
        public float m_fThirdFastestTime;

        //The current fourth fastest time to complete a level
        public float m_fFourthFastestTime;

        //The current fifth fastest time to complete a level
        public float m_fFifthFastestTime;

        //Players last checkpoint x
        public float m_fLastCheckPointX;

        //Players last checkpoint y
        public float m_fLastCheckPointY;

        public GameDataS(Settings a_setSettigs, float a_fSpeedRunTimer, float a_fLastCheckPointX, float a_fLastCheckPointY
            , float a_fFastestTime,float a_fSecondFastestTime, float a_fThirdFastestTime, float a_fFourthFastestTime, float a_fFifthFastestTime)
        //Constructor for the GameData
        {
            m_setSettigs = a_setSettigs;
            m_fSpeedRunTimer = a_fSpeedRunTimer;
            m_fLastCheckPointX = a_fLastCheckPointX;
            m_fLastCheckPointY = a_fLastCheckPointY;
            m_fFastestTime = a_fFastestTime;
            m_fSecondFastestTime = a_fSecondFastestTime;
            m_fThirdFastestTime = a_fThirdFastestTime;
            m_fFourthFastestTime = a_fFourthFastestTime;
            m_fFifthFastestTime = a_fFifthFastestTime;
        }
    }
}