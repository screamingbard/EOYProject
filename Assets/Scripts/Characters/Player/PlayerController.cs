using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float fSpeed = 15.0f;
    public float fJumpForce = 11.0f;
    public float fSJump = 2.0f;
    public float m_Force = 2.0f;
    public float fPullSPeed = 3.0f;

    public GameObject grappleObj;
    public Rigidbody2D rbProj2D;

    public float m_fFireRate = 1.0f;
    private int CurretnDir = 0;

    public Camera c1 = null;

    bool IsLeft = false;

    private float DistOverGround = 0.1f;
    //checks if on the ground
    public bool IsGrounded = false;

    Rigidbody2D rb2D = null;
    RaycastHit2D hit;
    // Use this for initialization
    void Awake () {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        rbProj2D = grappleObj.GetComponent<Rigidbody2D>();
        fSJump = fSJump * rb2D.mass;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        c1.transform.Rotate(0, 0, 0);
        hit = Physics2D.Raycast(transform.position, -Vector2.up, DistOverGround);
        if (hit == false)
        {
            IsGrounded = false;
        }

        if (IsGrounded)
        {
            rb2D.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * fSpeed, rb2D.velocity.y);

            c1.transform.position.Set(transform.position.x, transform.position.y - 5, -10);

            if (Input.GetAxisRaw("Horizontal") < 0 && !IsLeft)
            {
                transform.Rotate(0, 180, 0);
                IsLeft = true;
                CurretnDir = (int)Input.GetAxisRaw("Horizontal");
            }

            if (Input.GetAxisRaw("Horizontal") > 0 && IsLeft)
            {
                transform.Rotate(0, 180, 0);
                IsLeft = false;
                CurretnDir = (int)Input.GetAxisRaw("Horizontal");
            }

            if (Input.GetAxisRaw("Jump") == 1)
            {
                rb2D.AddForce(transform.up * (rb2D.mass * fJumpForce), ForceMode2D.Impulse);
                IsGrounded = false;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                rb2D.AddForce(transform.up * fSJump, ForceMode2D.Impulse);
                IsGrounded = false;
            }
        }

    }
    void LateUpdate()
    {
        Vector3 offset = new Vector3(0, 0, -10);
        c1.transform.position = transform.position + offset;
    }
    //Checks if the player is Colliding with a wall tag object
    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if(collision2D.gameObject.tag == "Wall")
            IsGrounded = true;

        if (collision2D.gameObject.tag != "Wall")
            IsGrounded = false;
    }

}
