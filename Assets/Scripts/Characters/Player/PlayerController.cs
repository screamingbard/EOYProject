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
    private GameObject cBall = null;

    public float m_fFireRate = 1.0f;
    private Vector3 target;
    private Vector2 mPos;
    private int CurretnDir = 0;
    private int count = 0;

    private Vector3 mDir;

    public Camera c1 = null;

    bool IsLeft = false;

    float m_fNextFire = 0.0f;

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
	void FixedUpdate ()
    {
        c1.transform.Rotate(0, 0, 0);
        hit = Physics2D.Raycast(transform.position, -Vector2.up, DistOverGround);
        if(hit == false)
        {
            IsGrounded = false;
        }

        if (IsGrounded)
        {
            rb2D.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * fSpeed, rb2D.velocity.y);

            c1.transform.position.Set(transform.position.x, -5 , -10);

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
        if (Input.GetMouseButtonDown(0))
        {
            mPos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

            Shoot();
            count++;
        }

        if (Input.GetMouseButton(1))
        {
            transform.position -= (transform.position - cBall.transform.position) * fPullSPeed * Time.deltaTime;
        }

        if (cBall != null)
        {
            Debug.DrawLine(transform.position, cBall.transform.position, Color.red);
            //the line
            //Debug.Log((transform.position - cBall.transform.position).magnitude);
        }
    }

    void LateUpdate()
    {
        Vector3 offset = new Vector3(0, -transform.position.y - 5, -10);
        c1.transform.position = transform.position + offset;
    }
    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if(collision2D.gameObject.tag == "Wall")
            IsGrounded = true;
    }

    void Shoot()
    {
        mDir = mPos - (Vector2)grappleObj.transform.position;
        
        mDir.Normalize();

        Quaternion projLoc = Quaternion.Euler(0, 0, Mathf.Atan2(mDir.y, mDir.x) * Mathf.Rad2Deg);
        //Creates a new grapple object if there is none
        if (count <= 0)
            cBall = Instantiate(grappleObj, mPos, projLoc);
        //Moves the already existing point
        if(count > 0)
            cBall.transform.position = mPos;

        Vector2 ballDir = new Vector2(projLoc.x, projLoc.y);
        ballDir.Normalize();

        rbProj2D.AddForce(ballDir * m_Force, ForceMode2D.Impulse);

        float dist = (mPos - (Vector2)cBall.transform.position).magnitude;

        Physics2D.Raycast(transform.position, mDir, dist);
        //The Raycast
        //Debug.Log(dist);
    }
}
