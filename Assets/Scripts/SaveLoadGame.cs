using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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


    float m_fSpeedRunTimer;

    void Update()
    {
        m_fSpeedRunTimer += Time.deltaTime;
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
        GameData m_sdData = 
            new GameData(m_setCurrentSettings, m_fSpeedRunTimer, m_tfPlayerPosition, m_tfLastCheckPoint);
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
        GameData m_sdData = (GameData)m_bfBinaryFormatter.Deserialize(m_fsFile);
        //Close the file stream
        m_fsFile.Close();

        //Set the loaded game data into the current data
        m_setCurrentSettings = m_sdData.m_setSettigs;
        m_fSpeedRunTimer = m_sdData.m_fSpeedRunTimer;
        m_tfPlayerPosition = m_sdData.m_tfPlayerPosiion;
        m_tfLastCheckPoint = m_sdData.m_tfLastCheckPoint;
    }
}
