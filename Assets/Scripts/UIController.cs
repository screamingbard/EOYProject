using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {

    public int m_iSceneIndex;

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(m_iSceneIndex);
    }
}
