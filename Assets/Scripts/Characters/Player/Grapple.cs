using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour {

    public GameObject m_goPlayer;
    //Rigidbody2D rb;
    Renderer mat;
    int colCode;

    int count = 0;

    Color m_cColor;
    void Awake ()
    {
        //rb = GetComponent<Rigidbody2D>();
        mat = GetComponent<Renderer>();
        //int randNum = Random.Range(1, 5);
        //switch (randNum)
        //{
        //    case 1:
        //        m_cColor.r = 0;
        //        m_cColor.g = 0;
        //        m_cColor.b = 255;
        //        m_cColor.a = 255;
        //        mat.material.color = m_cColor;
        //        break;
        //    case 2:
        //        m_cColor.r = 0;
        //        m_cColor.g = 255;
        //        m_cColor.b = 0;
        //        m_cColor.a = 255;
        //        mat.material.color = m_cColor;
        //        break;
        //    case 3:
        //        m_cColor.r = 255;
        //        m_cColor.g = 0;
        //        m_cColor.b = 0;
        //        m_cColor.a = 255;
        //        mat.material.color = m_cColor;
        //        break;
        //    case 4:
        //        m_cColor.r = 255;
        //        m_cColor.g = 255;
        //        m_cColor.b = 0;
        //        m_cColor.a = 255;
        //        mat.material.color = m_cColor;
        //        break;
        //}
        //Anti Epelepsy
        m_cColor.r = 0;
        m_cColor.g = 0;
        m_cColor.b = 255;
        m_cColor.a = 255;
        mat.material.color = m_cColor;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
    }
}
