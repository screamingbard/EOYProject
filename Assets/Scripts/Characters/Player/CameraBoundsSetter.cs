using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoundsSetter : MonoBehaviour {

    //Players tag
    public string m_stPlayerTag;

    //Minimum and maximum heights for the cameras
    public float m_fMinimumCameraHeight;
    public float m_fMaximumCameraHeight;

    //The list of cameras with the required script
    GameObject m_goCameras;

    void Start()
    {
        m_goCameras = GameObject.FindGameObjectWithTag("Cameras");
    }

    void OnTriggerStay2D(Collider2D a_colCollider)
    {
        if (a_colCollider.gameObject.tag == m_stPlayerTag)
        {
            m_goCameras.GetComponent<CameraFollow>().m_fMinCamHeight = m_fMinimumCameraHeight;
            m_goCameras.GetComponent<CameraFollow>().m_fMaxCamHeight = m_fMaximumCameraHeight;
        }
    }
}
