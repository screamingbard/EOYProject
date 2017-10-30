using System.Collections;
using System.Collections.Generic;
using XboxCtrlrInput;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public GameObject grappleObj;
    public Rigidbody2D rbProj2D;

    [HideInInspector]
    public float m_fFireRate = 1.0f;

    bool IsLeft = false;

    private float DistOverGround = 0.1f;
    //checks if on the ground
    [HideInInspector]
    public bool IsGrounded = false;

    //Grappling shooting for raycast
    public LineRenderer lrLineRenderer;
    [HideInInspector]
    public bool IsGrappling = false;
    
    //for twisting player
    int CurretnDir = 0;

    //Grappling Swinging
    DistanceJoint2D dj2dJoint;
    RaycastHit2D rc2dRaycast;

    public Rigidbody2D rb2D = null;
    RaycastHit2D hit;
    // Use this for initialization
    void Awake () {
        //Gets Players RigidBody
        rb2D = gameObject.GetComponent<Rigidbody2D>();

        //gets component of the grapple object
        rbProj2D = grappleObj.GetComponent<Rigidbody2D>();

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

        //ROTATEPLAYER CHECK
        //        //Rotates the player to the Right when the player is left
        //        //***CHANGE CODE***
        //        if (XCI.GetAxis(XboxAxis.LeftStickX) < 0 && !IsLeft)
        //        {
        //            //transform.Rotate(0, 180, 0);
        //            IsLeft = true;
        //            CurretnDir = (int)XCI.GetAxisRaw(XboxAxis.LeftStickX);
        //        }

        //        //Rotates the player to thr left when the player is right
        //        //***CHANGE CODE***
        //        if (XCI.GetAxis(XboxAxis.LeftStickX) > 0 && IsLeft)
        //        {
        //            //transform.Rotate(0, 180, 0);
        //            IsLeft = false;
        //            CurretnDir = (int)XCI.GetAxisRaw(XboxAxis.LeftStickX);
        //        }


    }
    //Checks if the player is Colliding with a wall tag object
    void OnCollisionEnter2D(Collision2D collision2D)
    {
        //Only allows the player to be grounded when colliding with another gameobject
        if (collision2D.gameObject.tag == "Wall" || collision2D.gameObject.tag == "Wall" && IsGrappling || collision2D.gameObject.layer == 9 || collision2D.gameObject.layer == 9 && IsGrappling)
            IsGrounded = true;
        else
            IsGrounded = false;

        if((collision2D.gameObject.layer == 9 && gameObject.GetComponentInChildren<ShootOBJ>().cBall) && gameObject.GetComponentInChildren<ShootOBJ>().cBall.GetComponent<Grapple>().GrapConnected)
        {
            gameObject.GetComponent<Player>().velocity.x = -gameObject.GetComponent<Player>().velocity.x/2;
        }
    }
}
