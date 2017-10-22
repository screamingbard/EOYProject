using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Grapple : MonoBehaviour
{
    public float projectileSpeed = 10.0f;

    [HideInInspector]
    public Vector2 CollideLocation;

    private Vector3 Dir = new Vector3(0, 0, 0);

    [HideInInspector]
    public GameObject tempobj;

    [HideInInspector]
    public bool GrapConnected = false;

    [HideInInspector]
    public bool holdGrapple = false;

    [HideInInspector]
    public float CurrentDist = 0.0f;

    public string CollisionTag;

    [HideInInspector]
    public GameObject followobj;

    Renderer mat;

    Rigidbody2D rb;

    Vector2 StoreMouse;

    Color m_cColor;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //StoreMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void Start()
    {
        //Dir = StoreMouse - (Vector2)tempobj.transform.position;
        Dir = followobj.transform.position - tempobj.transform.position;
        Dir.Normalize();
        rb.freezeRotation = true;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Dir * projectileSpeed * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision2d)
    {
        if (collision2d.gameObject.tag == CollisionTag)
        {
            projectileSpeed = 0;
            GrapConnected = true;
            CurrentDist = (transform.position - tempobj.transform.position).magnitude;
            holdGrapple = false;
        }

    }
}
