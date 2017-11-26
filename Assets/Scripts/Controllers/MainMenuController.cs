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
public class MainMenuController : MonoBehaviour {

    //The variable controlling which scene is loaded in the scene load method
    public int m_iSceneIndex = 0;

    //The Resume Button
    public Button m_btnPlayButton;

    //The credits button
    public Button m_btnCreditsButton;

    //The settings button
    public Button m_btnSettingsButton;

    //The quit button
    public Button m_btnQuitButton;

    //An event system
    public EventSystem m_esEventSysRef;

    //The first selected gameobject
    public GameObject m_goFirstSelected;

    //The first selected gameobject within the settings menu
    public GameObject m_goFirstSelectedSettings;

    //The first selected gameobject within the credites menu
    public GameObject m_goFirstSelectedCredits;

    //The main menu game object
    public GameObject m_goMainMenu;

    //The settings menu game object
    public GameObject m_goSettingsMenu;

    //The settings menu game object
    public GameObject m_goCreditsMenu;

    //The canvas
    public GameObject m_goCanvas;

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
        //Make sure the only active menu is the main menu
        if (m_goCreditsMenu.activeInHierarchy)
        {
            m_goCreditsMenu.SetActive(false);
        }
        if (m_goSettingsMenu.activeInHierarchy)
        {
            m_goSettingsMenu.SetActive(false);
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
            //If the event system doesn't have a current selected, set the current to the first selected
            m_esEventSysRef.SetSelectedGameObject(m_goFirstSelected);
        }

        if (XCI.GetButtonDown(XboxButton.B))
        {
            //Back out to the main menu if the 'B' button is pressed
            BackOutOfSettings();
            BackOutOfCredits();
        }
    }

    //Go into the settings menu
    public void GoToSettings()
    {
        //Set the settings menu to active
        m_goSettingsMenu.SetActive(true);
        //Set the selected button
        m_esEventSysRef.SetSelectedGameObject(m_goFirstSelectedSettings);
        //Set the main menu to deactive
        m_goMainMenu.SetActive(false);
    }

    //Go back to the main menu from the settings
    public void BackOutOfSettings()
    {
        //Set the settings menu to deactive
        m_goSettingsMenu.SetActive(false);
        //Set the selected button
        m_esEventSysRef.SetSelectedGameObject(null);
        //Set the main menu to active
        m_goMainMenu.SetActive(true);
    }

    //Go back to the main menu from the credits
    public void GoToCredits()
    {
        //Set the credits menu to active
        m_goCreditsMenu.SetActive(true);
        //Set the selected button
        m_esEventSysRef.SetSelectedGameObject(m_goFirstSelectedCredits);
        //Set the main menu to deactive
        m_goMainMenu.SetActive(false);
    }

    //Go back to the main menu from the settings
    public void BackOutOfCredits()
    {
        //Set the credits menu to deactive
        m_goCreditsMenu.SetActive(false);
        //Set the selected button
        m_esEventSysRef.SetSelectedGameObject(null);
        //Set the main menu to active
        m_goMainMenu.SetActive(true);
    }
}
