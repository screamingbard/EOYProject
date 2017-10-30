using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadSettings : MonoBehaviour
{

    //A controlable file name for design control
    public string m_stPrefsSaveFileName;

    //The current game data
    GameData.Settings m_setCurrentSettings;

    public void SaveFile()
    //Save the settings as set by the player whenever they are applied by the player
    {
        //Set the destination of the saved settings
        string m_stDestination = Application.persistentDataPath + "/" + m_stPrefsSaveFileName + ".dat";
        FileStream m_fsFile;

        //If the file already exists open the file stream
        if (File.Exists(m_stDestination)) m_fsFile = File.OpenWrite(m_stDestination);
        //If the file does not yet exist create a new file
        else m_fsFile = File.Create(m_stDestination);

        //Create the variable to store the current settings
        GameData.Settings m_sdData = m_setCurrentSettings;
        BinaryFormatter m_bfBinaryFormatter = new BinaryFormatter();

        //Save the settings in a binary format
        m_bfBinaryFormatter.Serialize(m_fsFile, m_sdData);

        //Close the file stream
        m_fsFile.Close();
    }

    public void LoadFile()
    //Load settings into the game as they where previously set
    {
        //Get the destination to load the settings from
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
        //Grap the settings from the file
        GameData.Settings m_sdData = (GameData.Settings)m_bfBinaryFormatter.Deserialize(m_fsFile);
        //Close the file stream
        m_fsFile.Close();

        //Set the loaded settings into the current data
        m_setCurrentSettings = m_sdData;
    }

}
