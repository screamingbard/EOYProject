using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadGame : MonoBehaviour
{
    //A controlable file name for design control
    public string m_stPrefsSaveFileName;

    //The current transform for the player
    public Transform m_tfPlayerPosition;

    //The current game data
    GameData.Settings m_setCurrentSettings;

    //
    [HideInInspector]
    public Transform m_tfLastCheckPoint;

    //
    public bool m_bTimerIsVisible;

    //
    public GameObject m_goTimerTextUI;

    //Ingame timer for current run
    public float m_fSpeedRunTimer;

    //The highscores
    float m_fFastestTime;
    float m_fSecondFastestTime;
    float m_fThirdFastestTime;
    float m_fFourthFastestTime;
    float m_fFifthFastestTime;

    void Start()
    {
        m_tfLastCheckPoint = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        m_fSpeedRunTimer += Time.deltaTime;
        //Control over whether or not the timer is visible ingame
        if (m_bTimerIsVisible)
            m_goTimerTextUI.SetActive(true);
        else
            m_goTimerTextUI.SetActive(false);
    }
    public void SaveFile()
    //Save the data from the game
    {
        //Set the destination of the saved game data
        string m_stDestination = Application.persistentDataPath + "/" + m_stPrefsSaveFileName + ".dat";
        FileStream m_fsFile;

        //If the file already exists open the file stream
        if (File.Exists(m_stDestination)) m_fsFile = File.OpenWrite(m_stDestination);
        //If the file does not yet exist create a new file
        else m_fsFile = File.Create(m_stDestination);

        //Create the variable to store the current game data
        GameData.GameDataS m_sdData = 
            new GameData.GameDataS(m_setCurrentSettings, m_fSpeedRunTimer, m_tfLastCheckPoint.position.x, m_tfLastCheckPoint.position.y, 
            m_fFastestTime, m_fSecondFastestTime, m_fThirdFastestTime, m_fFourthFastestTime, m_fFifthFastestTime);
        BinaryFormatter m_bfBinaryFormatter = new BinaryFormatter();

        //Save the data in a binary format
        m_bfBinaryFormatter.Serialize(m_fsFile, m_sdData);

        //Close the file stream
        m_fsFile.Close();

    }

    public void LoadFile()
    //Load data into the game
    {
        //Get the destination to load the game data from
        string m_stDestination = Application.persistentDataPath + "/" + m_stPrefsSaveFileName + ".dat";
        FileStream m_fsFile;

        //If the file already exists open the file stream
        if (File.Exists(m_stDestination)) m_fsFile = File.OpenRead(m_stDestination);
        //Otherwise leave the function as to not break the game and log an error
        else
        {
            Debug.LogError("File not found");
            return;
        }
        BinaryFormatter m_bfBinaryFormatter = new BinaryFormatter();
        //Grap the game data from the file
        GameData.GameDataS m_sdData = (GameData.GameDataS)m_bfBinaryFormatter.Deserialize(m_fsFile);
        //Close the file stream
        m_fsFile.Close();
        
        //Set the loaded game data into the current data
        m_setCurrentSettings = m_sdData.m_setSettigs;
        m_fSpeedRunTimer = m_sdData.m_fSpeedRunTimer;
        m_tfLastCheckPoint.position = new Vector2(m_sdData.m_fLastCheckPointX, m_sdData.m_fLastCheckPointY);
        m_fFastestTime = m_sdData.m_fFastestTime;
        m_fSecondFastestTime = m_sdData.m_fSecondFastestTime;
        m_fThirdFastestTime = m_sdData.m_fThirdFastestTime;
        m_fFourthFastestTime = m_sdData.m_fFourthFastestTime;
        m_fFifthFastestTime = m_sdData.m_fFifthFastestTime;
    }

    public void HighScores()
    {
        if (m_fFastestTime < m_fSpeedRunTimer)
        {
            m_fFifthFastestTime = m_fFourthFastestTime;
            m_fFourthFastestTime = m_fThirdFastestTime;
            m_fThirdFastestTime = m_fSecondFastestTime;
            m_fSecondFastestTime = m_fFastestTime;
            m_fFastestTime = m_fSpeedRunTimer;
        }
        else if (m_fSecondFastestTime < m_fSpeedRunTimer)
        {
            m_fFifthFastestTime = m_fFourthFastestTime;
            m_fFourthFastestTime = m_fThirdFastestTime;
            m_fThirdFastestTime = m_fSecondFastestTime;
            m_fSecondFastestTime = m_fSpeedRunTimer;
        }
        else if (m_fThirdFastestTime < m_fSpeedRunTimer)
        {
            m_fFifthFastestTime = m_fFourthFastestTime;
            m_fFourthFastestTime = m_fThirdFastestTime;
            m_fThirdFastestTime = m_fSpeedRunTimer;
        }
        else if (m_fFourthFastestTime < m_fSpeedRunTimer)
        {
            m_fFifthFastestTime = m_fFourthFastestTime;
            m_fFourthFastestTime = m_fSpeedRunTimer;
        }
        else if (m_fFifthFastestTime < m_fSpeedRunTimer)
        {
            m_fFifthFastestTime = m_fSpeedRunTimer;
        }
    }
}
