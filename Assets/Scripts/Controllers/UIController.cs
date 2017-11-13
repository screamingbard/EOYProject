using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using XboxCtrlrInput;

public class UIController : MonoBehaviour {

    //The variable controlling which scene is loaded in the scene load method
    public int m_iSceneIndex = 1;

    //The Resume Button
    public Button m_btnResumeButton;

    //The settings button
    public Button m_btnSettingsButton;

    //The quit button
    public Button m_btnQuitButton;

    //The cancel quit button
    public Button m_btnCancelQuitButton;

    //The quit game button
    public Button m_btnQuitGameButton;
    
    //The quit to menu button
    public Button m_btnRestartButton;

    //An event system
    public EventSystem m_esEventSysRef;

    //The first selected gameobject
    public GameObject m_goFirstSelected;

    //The first selected gameobject within the settings menu
    public GameObject m_goFirstSelectedSettings;

    //The first selected gameobject within the quit menu
    public GameObject m_goFirstSelectedQuit;

    //The main menu game object
    public GameObject m_goPauseMenu;

    //The settings menu game object
    public GameObject m_goSettingsMenu;

    //The settings menu game object
    public GameObject m_goQuitMenu;

    void Awake()
    {
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }
        if (m_goPauseMenu.activeInHierarchy)
        {
            m_goPauseMenu.SetActive(false);
        }
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
        if (m_esEventSysRef.currentSelectedGameObject == null)
        {
            m_esEventSysRef.SetSelectedGameObject(m_goFirstSelected);
        }
        if (XCI.GetButtonDown(XboxButton.Start) || Input.GetKeyDown(KeyCode.Escape))
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
        if (XCI.GetButtonDown(XboxButton.B))
        {
            Unpause();
            BackOutOfQuit();
            BackOutOfSettings();
        }
    }
    //Go into the settings menu
    public void GoToSettings()
    {
        m_goSettingsMenu.SetActive(true);
        m_esEventSysRef.SetSelectedGameObject(m_goFirstSelectedSettings);
        m_goPauseMenu.SetActive(false);
    }

    //Go back to the pause menu from the settings
    public void BackOutOfSettings()
    {
        m_goSettingsMenu.SetActive(false);
        m_esEventSysRef.SetSelectedGameObject(null);
        m_goPauseMenu.SetActive(true);
    }

    //Go into the quit menu
    public void GoToQuit()
    {
        m_goQuitMenu.SetActive(true);
        m_esEventSysRef.SetSelectedGameObject(m_goFirstSelectedQuit);
        m_goPauseMenu.SetActive(false);
    }

    //Go back to the pause menu from the quit
    public void BackOutOfQuit()
    {
        m_goQuitMenu.SetActive(false);
        m_esEventSysRef.SetSelectedGameObject(null);
        m_goPauseMenu.SetActive(true);
    }

    void Pause()
    {
        Time.timeScale = 0;
        m_goPauseMenu.SetActive(true);
    }

    void Unpause()
    {
        Time.timeScale = 1;
        m_goPauseMenu.SetActive(false);
    }
}
