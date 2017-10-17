using System.Collections;
using System.Collections.Generic;
using XboxCtrlrInput;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float fSpeed = 15.0f;
    private float fJumpForce = 11.0f;
    public float fSJump = 2.0f;
    public float m_Force = 2.0f;
    public float fPullSPeed = 3.0f;

    public GameObject grappleObj;
    public Rigidbody2D rbProj2D;
    [Tooltip("This object is the empty gameobject that is a child to the player")]
    public GameObject Aiming;

    [HideInInspector]
    public float m_fFireRate = 1.0f;

    bool IsLeft = false;

    private float DistOverGround = 0.1f;
    //checks if on the ground
    [HideInInspector]
    public bool IsGrounded = false;
    //Remove before release/Add Cheatcode
    [Tooltip("Only enable if you are debugging, creates a much larger jump")]
    public bool bCheatJump = false;

    [Tooltip("Currently only works in air")]
    public float MaxSpeed = 10.0f;

    //Grappling shooting for raycast
    public LineRenderer lrLineRenderer;
    private GameObject cBall = null;
    [HideInInspector]
    public bool IsGrappling = false;

    float fcount = 0;
    int CurretnDir = 0;

    //Grappling Swinging
    DistanceJoint2D dj2dJoint;
    RaycastHit2D rc2dRaycast;
    //Set to private later
    //[Tooltip("Maximum distance for the player to slide back on the grapple, defaults to 10.0f")]
    [HideInInspector]
    public float fHoldDistance = 10.0f;

    [Tooltip("The layer that the player attatches to, in this case the layer of the grapple")]
    public LayerMask lmLayerMask;
    [HideInInspector]
    public Vector2 ballDir;

    public Rigidbody2D rb2D = null;
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
                //***CHANGE CODE***
                if (XCI.GetAxisRaw(XboxAxis.LeftTrigger) == 1)
                {
                    rb2D.AddForce(transform.up * (rb2D.mass * fJumpForce), ForceMode2D.Impulse);
                    IsGrounded = false;
                    if (Input.GetMouseButtonUp(0))
                    {
                        Aiming.GetComponent<ShootOBJ>().StopShoot();
                        IsGrappling = false;
                    }
                }
            }
            //Small jump
            //***CHANGE CODE***
            if (XCI.GetAxisRaw(XboxAxis.LeftTrigger) == 1)
            {
                rb2D.AddForce(transform.up * (rb2D.mass * fSJump), ForceMode2D.Impulse);
                IsGrounded = false;
            }
            if (Input.GetMouseButtonUp(0))
            {
                Aiming.GetComponent<ShootOBJ>().StopShoot();
                IsGrappling = false;
            }
        }

        //Moves the player
        //Allow for velocity to change, for jumping
        //Uses Physics
        //***CHANGE CODE
        rb2D.velocity += Vector2.right * XCI.GetAxis(XboxAxis.LeftStickX) * fSpeed * Time.deltaTime;
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
        
        //Slowing down
        if(XCI.GetAxis(XboxAxis.LeftStickX) == 0 && IsGrounded)
        {
            rb2D.velocity -= Vector2.right * fSpeed * Time.deltaTime * fSpeed;
            Vector2 storeVel = rb2D.velocity;
            if (storeVel.x <= 0.0000001 || storeVel.x > -0.00000001)
            {
                storeVel.x = 0;
                rb2D.velocity = storeVel;
            }
        }

        if (cBall != null)
        {
            if (IsGrounded)
            {
                //Rotates the player to the Right when the player is left
                //***CHANGE CODE***
                if (XCI.GetAxis(XboxAxis.LeftStickX) < 0 && !IsLeft)
                {
                    //transform.Rotate(0, 180, 0);
                    IsLeft = true;
                    CurretnDir = (int)XCI.GetAxisRaw(XboxAxis.LeftStickX);
                }

                //Rotates the player to thr left when the player is right
                //***CHANGE CODE***
                if (XCI.GetAxis(XboxAxis.LeftStickX) > 0 && IsLeft)
                {
                    //transform.Rotate(0, 180, 0);
                    IsLeft = false;
                    CurretnDir = (int)XCI.GetAxisRaw(XboxAxis.LeftStickX);
                }
            }

        }
        //***CHANGE CODE***
        if (XCI.GetButton(XboxButton.RightBumper))
        {
            Aiming.GetComponent<ShootOBJ>().DrawLine();

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
        
    }
    //Checks if the player is Colliding with a wall tag object
    void OnCollisionEnter2D(Collision2D collision2D)
    {
        //Only allows the player to be grounded when colliding with another gameobject
        if (collision2D.gameObject.tag == "Wall" || collision2D.gameObject.tag == "Wall" && IsGrappling || collision2D.gameObject.layer == 9 || collision2D.gameObject.layer == 9 && IsGrappling)
            IsGrounded = true;
        else
            IsGrounded = false;
    }
}
