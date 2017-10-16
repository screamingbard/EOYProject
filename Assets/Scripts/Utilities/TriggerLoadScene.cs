using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TriggerLoadScene : MonoBehaviour {

    public int m_iSceneIndex;

    void OnTriggerEnter2D()
    {
        SceneManager.LoadScene(m_iSceneIndex);
    }
}
