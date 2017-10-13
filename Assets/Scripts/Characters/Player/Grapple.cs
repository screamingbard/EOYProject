using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour {

    public GameObject m_goPlayer;
    public float projectileSpeed = 5.0f;
    private float Speed;
    private float stop = 0.0f;
    public Vector2 CollideLocation;

    Rigidbody2D rb;
    Renderer mat;

    Color m_cColor;
    void Awake ()
    {
        rb = GetComponent<Rigidbody2D>();
        mat = GetComponent<Renderer>();

        Speed = projectileSpeed;

        //Debugging
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
    void OncollisionEnter2D(Collision2D collision2d)
    {
        
    }
}
