using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using XboxCtrlrInput;

public class UIController : MonoBehaviour {

    //The variable controlling which scene is loaded in the scene load method
    public int m_iSceneIndex = 0;

    //
    public Button m_buRstartButton;

    //
    public EventSystem m_esEventSysRef;

    void Awake()
    {
        m_esEventSysRef.SetSelectedGameObject(m_buRstartButton.gameObject);
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

    void Update()
    {
        if (m_esEventSysRef.alreadySelecting == false)
        {
            m_esEventSysRef.SetSelectedGameObject(m_buRstartButton.gameObject);
        }

        if (XCI.GetButton(XboxButton.Start))
        {
            if (Time.timeScale == 1)
            {
                Pause();
            }
            else
            {
                Unpause();
            }
        }
    }

    void Pause()
    {
        Time.timeScale = 0;
    }

    void Unpause()
    {
        Time.timeScale = 1;
    }
}
