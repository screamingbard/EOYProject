using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float fSpeed = 15.0f;
    private float fJumpForce = 11.0f;
    public float fSJump = 2.0f;
    public float m_Force = 2.0f;
    public float fPullSPeed = 3.0f;

    public GameObject grappleObj;
    public Rigidbody2D rbProj2D;

    public float m_fFireRate = 1.0f;
    private int CurretnDir = 0;

    bool IsLeft = false;

    private float DistOverGround = 0.1f;
    //checks if on the ground
    public bool IsGrounded = false;
    //Remove before release/Add Cheatcode
    [Tooltip("Only enable if you are debugging, creates a much larger jump")]
    public bool bCheatJump = false;

    public float MaxSpeed = 10.0f;

    //Grappling shooting for raycast
    public LineRenderer lrLineRenderer;
    private GameObject cBall = null;
    private Vector2 mPos;
    private int count = 0;
    private Vector3 mDir;
    bool IsGrappling = false;

    float fcount = 0;

    //Grappling Swinging
    DistanceJoint2D dj2dJoint;
    RaycastHit2D rc2dRaycast;
    
    [Tooltip("Maximum distance for the player to slide back on the grapple, defaults to 10.0f")]
    public float fHoldDistance = 10.0f;
    [Tooltip("The layer that the player attatches to, in this case the layer of the grapple")]
    public LayerMask lmLayerMask;

    Rigidbody2D rb2D = null;
    RaycastHit2D hit;
    // Use this for initialization
    void Awake () {
        //Gets Players RigidBody
        rb2D = gameObject.GetComponent<Rigidbody2D>();

        //gets component of the grapple object
        rbProj2D = grappleObj.GetComponent<Rigidbody2D>();

        //Jumping height
        fSJump = fSJump * rb2D.mass;

        dj2dJoint = GetComponent<DistanceJoint2D>();
        dj2dJoint.enabled = false;
        lrLineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if the player is a certain distance over the ground
        hit = Physics2D.Raycast(transform.position, -Vector2.up, DistOverGround);
        if (hit == false)
        {
            IsGrounded = false;
        }

        //Checks if the player is grounded
        if (IsGrounded)
        {
            //Jumping
            if (bCheatJump)
            {
                if (Input.GetAxisRaw("Jump") == 1)
                {
                    rb2D.AddForce(transform.up * (rb2D.mass * fJumpForce), ForceMode2D.Impulse);
                    IsGrounded = false;
                    if (Input.GetMouseButtonUp(0))
                    {
                        StopShoot();
                        IsGrappling = false;
                    }
                }
            }
            //Small jump
            if (Input.GetKeyDown(KeyCode.W) || Input.GetAxisRaw("Jump") == 1)
            {
                rb2D.AddForce(transform.up * (rb2D.mass * fSJump), ForceMode2D.Impulse);
                IsGrounded = false;
            }
            if (Input.GetMouseButtonUp(0))
            {
                StopShoot();
                IsGrappling = false;
            }
        }

        //Moves the player
        //Allow for velocity to change, for jumping
        //Uses Physics
        rb2D.velocity += Vector2.right * Input.GetAxis("Horizontal") * fSpeed * Time.deltaTime;
        Vector2 v2StoreVelocity = rb2D.velocity;
        //Sets a maximum and minimum speed to make sure the player does not speed up infinatly
        if (v2StoreVelocity.x > MaxSpeed)
        {
            v2StoreVelocity.x = MaxSpeed;
            rb2D.velocity = v2StoreVelocity;
        }
        if(v2StoreVelocity.x < -MaxSpeed)
        {
            v2StoreVelocity.x = -MaxSpeed;
            rb2D.velocity = v2StoreVelocity;
        }
        
        if(Input.GetAxis("Horizontal") == 0 && IsGrounded)
        {
            rb2D.velocity -= Vector2.right * fSpeed * Time.deltaTime * fSpeed;
            Vector2 storeVel = rb2D.velocity;
            if (storeVel.x <= 0.05 || storeVel.x > -0.05)
            {
                storeVel.x = 0;
                rb2D.velocity = storeVel;
            }
        }

        //Rotates the player to the Right when the player is left
        if (Input.GetAxisRaw("Horizontal") < 0 && !IsLeft)
        {
            transform.Rotate(0, 180, 0);
            IsLeft = true;
            CurretnDir = (int)Input.GetAxisRaw("Horizontal");
        }

        //Rotates the player to thr left when the player is right
        if (Input.GetAxisRaw("Horizontal") > 0 && IsLeft)
        {
            transform.Rotate(0, 180, 0);
            IsLeft = false;
            CurretnDir = (int)Input.GetAxisRaw("Horizontal");
        }

        //Places down the pivot point for the grapple
        if (Input.GetMouseButtonDown(0))
        {
            mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Shoot();

            IsGrappling = true;
        }
        //Renders the line if the mouse is held down
        if (IsGrappling)
        {
            lrLineRenderer.SetPosition(0, transform.position);
        }

        if (Input.GetMouseButton(0))
        {
            //NO JUMPING IN THE AIR!!!
            fcount = 0;
            if (!IsGrounded)
            {
                IsGrounded = false;
            }

            if (IsGrounded)
            {
                fcount++;
            }

            if (fcount > 0)
            {
                IsGrounded = false;
            }   
        }
        
        //destroys the grapple if the mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            StopShoot();
            IsGrappling = false;
        }

    }
    void LateUpdate()
    {
        //Makes sure that the grapple has been removed
        if (IsGrappling == false)
        {
            lrLineRenderer.enabled = false;
            StopShoot();
        }
        else if (IsGrappling == false && IsGrounded)
        {
            lrLineRenderer.enabled = false;
            StopShoot();
        }
    }
    //Checks if the player is Colliding with a wall tag object
    void OnCollisionEnter2D(Collision2D collision2D)
    {
        //Only allows the player to be grounded when colliding with another gameobject
        if (collision2D.gameObject.tag == "Wall")
            IsGrounded = true;
        else
            IsGrounded = false;
    }
    //Shoots out the grapple
    void Shoot()
    {

        mDir = mPos - (Vector2)grappleObj.transform.position;

        mDir.Normalize();

        Quaternion projLoc = Quaternion.Euler(0, 0, Mathf.Atan2(mDir.y, mDir.x) * Mathf.Rad2Deg);

        //Creates a new grapple object if there is none
        if (cBall == null)
            cBall = Instantiate(grappleObj, mPos, projLoc);

        Vector2 ballDir = new Vector2(projLoc.x, projLoc.y);
        ballDir.Normalize();

        rbProj2D.AddForce(ballDir * m_Force, ForceMode2D.Impulse);

        float dist = (mPos - (Vector2)cBall.transform.position).magnitude;

        Physics2D.Raycast(transform.position, mDir, dist);

        Shoot1();
    }
    void Shoot1()
    {
        rc2dRaycast = Physics2D.Raycast(transform.position, (Vector3)mPos - transform.position, fHoldDistance, lmLayerMask);
        if(rc2dRaycast.collider != null && rc2dRaycast.collider.gameObject.GetComponent<Rigidbody2D>() != null)
        {
            //Enables the joint for rotation
            dj2dJoint.enabled = true;
            
            //Connects Grapple with the rigidbody
            dj2dJoint.connectedBody = rc2dRaycast.collider.gameObject.GetComponent<Rigidbody2D>();
            
            //Connects the Anchor
            dj2dJoint.connectedAnchor = rc2dRaycast.point - new Vector2(rc2dRaycast.collider.transform.position.x, rc2dRaycast.collider.transform.position.y);
            
            //Gets the distance between the two points, lenght of line
            dj2dJoint.distance = Vector2.Distance(transform.position, cBall.transform.position);

            //Enables line drawing
            lrLineRenderer.enabled = true;

            //Sets the position of the player and the pivot point for the line to drqaw
            lrLineRenderer.SetPosition(0, transform.position);
            lrLineRenderer.SetPosition(1, cBall.transform.position);
        }
    }
    //Stops the shooting of the Grapple
    void StopShoot()
    {
        if (grappleObj != null)
            Destroy(cBall);

        dj2dJoint.enabled = false;
        lrLineRenderer.enabled = false;
    }
}
