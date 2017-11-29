using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using XboxCtrlrInput;

/// ------------- ///
// Micheal Corben
/// ------------- ///
public class UIController : MonoBehaviour {

    //The variable controlling which scene is loaded in the scene load method
    public int m_iSceneIndex = 1;

    //The Resume Button
    public Button m_btnPlayButton;

    //The settings button
    public Button m_btnSettingsButton;

    //The quit button
    public Button m_btnQuitButton;

    //The cancel quit button
    public Button m_btnCancelQuitButton;

    //The confirmation the player wishes to quit
    public Button m_btnConfirmQuitButton;
    
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

    //Pause screen background
    public GameObject m_goPauseScreenBackground;

    //The canvas
    public GameObject m_goCanvas;

    //To stop the player jumping when you unpause
    bool m_bInputDelayingness;

    public GraphicRaycaster m_GraphicRaycaster;

    void Awake()
    {
        //Make sure the canvas exists in the scene
        if (m_goCanvas != null && m_goCanvas.activeInHierarchy == false)
        {
            m_goCanvas.SetActive(true);
        }
        //On awake make sure the game isn't paused
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }
        //Also if any menus are active deactivate them
        if (m_goQuitMenu.activeInHierarchy)
        {
            BackOutOfQuit();
        }
        if (m_goSettingsMenu.activeInHierarchy)
        {
            BackOutOfSettings();
        }
        if (m_goPauseMenu.activeInHierarchy)
        {
            m_goPauseMenu.SetActive(false);
        }
        if (m_goPauseScreenBackground.activeInHierarchy)
        {
            m_goPauseScreenBackground.SetActive(false);
        }
    }
    public void Quit()
    //On call will close the game
    {
        Application.Quit();
    }
    public void ReloadLevel()
    //On call will load a specified scene
    {
        Awake();
        SceneManager.LoadScene(m_iSceneIndex);
    }

    void Update()
    {
        //If the event system doesn't have anything selected
        if (m_esEventSysRef.currentSelectedGameObject == null)
        {
            //Select the assigned first selected
            m_esEventSysRef.SetSelectedGameObject(m_goFirstSelected);
        }
        //Pause the game if the game is unpaused or unpause if other wise
        if (XCI.GetButtonDown(XboxButton.Start) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Pause();
            }
            else
            {
                //Deactivate the quit menu
                if (m_goQuitMenu.activeInHierarchy)
                {
                    BackOutOfQuit();
                }
                //Deactivate the settings menu
                if (m_goSettingsMenu.activeInHierarchy)
                {
                    BackOutOfSettings();
                }
                Unpause();
            }
        }
        //Back out of the current menu to the previouse or unpause if the current active menu is the pause screen
        if (XCI.GetButtonDown(XboxButton.B))
        {
            if (m_goQuitMenu.activeInHierarchy)
            {
                BackOutOfQuit();
            }
            else if (m_goSettingsMenu.activeInHierarchy)
            {
                BackOutOfSettings();
            }
            else if (m_goPauseMenu.activeInHierarchy)
            {
                Unpause();
            }
        }
    }
    public void GoToSettings()
    //Go into the settings menu
    {
        m_goSettingsMenu.SetActive(true);
        m_esEventSysRef.SetSelectedGameObject(m_goFirstSelectedSettings);
        m_goPauseMenu.SetActive(false);
    }

    public void BackOutOfSettings()
    //Go back to the pause menu from the settings
    {
        m_goSettingsMenu.SetActive(false);
        m_esEventSysRef.SetSelectedGameObject(m_btnSettingsButton.gameObject);
        m_btnSettingsButton.OnSelect(null);
        m_goPauseMenu.SetActive(true);
    }

    public void GoToQuit()
    //Go into the quit menu
    {
        m_goQuitMenu.SetActive(true);
        m_esEventSysRef.SetSelectedGameObject(m_goFirstSelectedQuit);
        m_goPauseMenu.SetActive(false);
    }

    public void BackOutOfQuit()
    //Go back to the pause menu from the quit
    {
        m_goQuitMenu.SetActive(false);
        m_esEventSysRef.SetSelectedGameObject(m_btnQuitButton.gameObject);
        m_btnQuitButton.OnSelect(null);
        m_goPauseMenu.SetActive(true);
    }
    
    public void Pause()
    {
        if (XCI.GetNumPluggedCtrlrs() != 0)
        {
            m_GraphicRaycaster.enabled = false;
            Cursor.visible = false;
        }
        else
        {
            m_GraphicRaycaster.enabled = true;
            Cursor.visible = true;
        }
        //Set the selected button to the play button
        m_esEventSysRef.SetSelectedGameObject(m_btnPlayButton.gameObject);
        m_btnPlayButton.OnSelect(null);
        //Pause the game by setting the time scale to zero
        Time.timeScale = 0;
        //Set the pause menu to active
        m_goPauseMenu.SetActive(true);
        //Also activate the pause menu background
        m_goPauseScreenBackground.SetActive(true);
    }

    public void Unpause()
    {
        //Unpause the game by setting the time scale back to one
        Time.timeScale = 1;
        //deactivate the pause menu and background screen
        m_goPauseMenu.SetActive(false);
        m_goPauseScreenBackground.SetActive(false);
    }
}
