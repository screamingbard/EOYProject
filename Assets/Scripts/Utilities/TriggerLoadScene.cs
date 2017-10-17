using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TriggerLoadScene : MonoBehaviour {

    //The variable controlling which scene is loaded in the scene load method
    public int m_iSceneIndex;

    void OnTriggerEnter2D()
    //When the player passes through the trigger will load a specified scene
    {
        SceneManager.LoadScene(m_iSceneIndex);
    }
}
