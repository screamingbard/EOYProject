using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TriggerLoadScene : MonoBehaviour {

    //Tag that designates the player
    public string m_stPlayerTag;

    //
    public GameObject m_gSaveLoadController;

    //The variable controlling which scene is loaded in the scene load method
    public int m_iSceneIndex;

    void OnTriggerEnter2D(Collider2D a_col2dCollider)
    //When the player passes through the trigger will load a specified scene
    {
        if (a_col2dCollider.gameObject.tag == m_stPlayerTag)
        {
            m_gSaveLoadController.GetComponent<SaveLoadGame>().HighScores();
            m_gSaveLoadController.GetComponent<SaveLoadGame>().SaveFile();
            SceneManager.LoadScene(m_iSceneIndex);

            Debug.Log(Application.persistentDataPath);
        }
    }
}
