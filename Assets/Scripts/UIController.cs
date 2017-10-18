using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour {

    //The variable controlling which scene is loaded in the scene load method
    public int m_iSceneIndex = 0;

    //
    public Button restartButton;

    //
    public EventSystem EventSysRef;

    void Awake()
    {
        EventSysRef.SetSelectedGameObject(restartButton.gameObject);
    }
    public void Quit()
    //On call will close the game
    {
        Application.Quit();
    }

    public void LoadLevel()
    //On call will load a specified scene
    {
        SceneManager.LoadScene(m_iSceneIndex);
    }
}
