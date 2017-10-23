using System.Collections;
using System.Collections.Generic;
using XboxCtrlrInput;
using UnityEngine;

public class ShootOBJ : MonoBehaviour {

    //Grappling shooting for raycast
    public LineRenderer lrLineRenderer;
    public GameObject goPlayer;
    private Vector2 mPos;

    private Vector3 mDir;
    [HideInInspector]
    public bool IsGrappling = false;

    public float fMaxDist = 10.0f;

    //Grappling Swinging
    DistanceJoint2D dj2dJoint;
    RaycastHit2D rc2dRaycast;
    //Set to private later
    [Tooltip("Maximum distance for the player to slide back on the grapple, defaults to 10.0f")]
    public float fHoldDistance = 10.0f;
    [Tooltip("The layer that the player attatches to, in this case the layer of the grapple")]
    public LayerMask lmLayerMask;

    [HideInInspector]
    public Vector2 ballDir;

    public GameObject grappleObj;
    private GameObject cBall = null;

    public float grapImpulse = 1.0f;
    public float GrapCooldown = 0.5f;
    float cd = 0;
    public bool cooldowncheck = false;

    public Rigidbody2D rbPlayer;
    [HideInInspector]
    public Vector2 StorePos;

    int bstart = 0;
    int grapCount = 0;

    public Transform rotObject;
    Quaternion storeAngle;

    bool initialHit = true;

    //float timer = 0;
    //bool IsTiming = false;

    // Use this for initialization
    void Awake() {

        dj2dJoint = goPlayer.GetComponent<DistanceJoint2D>();

        lrLineRenderer.enabled = false;
        
        StorePos = Input.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {

        StorePos = (Input.mousePosition - transform.position);

        transform.LookAt(rotObject);



        //Places down the pivot point for the grapple
        if (cooldowncheck == false) {
            if (XCI.GetButtonDown(XboxButton.RightBumper))
            {

                Shoot();

                grappleObj.GetComponent<Grapple>().holdGrapple = true;

                IsGrappling = true;

                if (cBall.GetComponent<Grapple>().GrapConnected && initialHit)
                {
                    goPlayer.GetComponent<Player>().velocity += mDir * grapImpulse;
                    fHoldDistance -= 1;
                    initialHit = false;
                }
                cooldowncheck = true;
            }
        }
        //else
        if(cooldowncheck)
        {
            cd += Time.deltaTime;
            if(cd >= GrapCooldown)
            {
                cooldowncheck = false;
                cd = 0;
            }
        }

        if (IsGrappling)
        {
            if (lrLineRenderer.enabled == true)
                DrawLine();
        }

        //***Makes sure that if the player is in the air all scripts are disabled!!
        if (cBall != null)
        {
            if (IsGrappling && cBall.GetComponent<Grapple>().GrapConnected == true)
            {
                goPlayer.GetComponent<Controller2D>().enabled = false;
                goPlayer.GetComponent<Player>().enabled = false;
                goPlayer.GetComponent<PlayerController>().enabled = true;
                rbPlayer.bodyType = RigidbodyType2D.Dynamic;
                if (bstart < 1)
                {
                    rbPlayer.velocity = goPlayer.GetComponent<Player>().velocity;
                    bstart++;
                }
                grapCount++;

                rbPlayer.freezeRotation = true;
            }
            else
            {
                if (goPlayer.GetComponent<PlayerController>().IsGrounded)
                {
                    ScriptNormSet();
                    grapCount = 0;
                }
            }
        }
        else
        {
            if (goPlayer.GetComponent<PlayerController>().IsGrounded)
            {
                ScriptNormSet();
                grapCount = 0;
            }
        }
        //destroys the grapple if the mouse button is released
        if (XCI.GetButtonUp(XboxButton.RightBumper))
        {
            StopShoot();

            IsGrappling = false;

            goPlayer.GetComponent<Player>().velocity = rbPlayer.velocity;

            initialHit = true;

            cooldowncheck = true;
        }

    }

    void LateUpdate()
    {
        if (dj2dJoint.distance > fMaxDist)
        {
            StopShoot();
        }
    }
    //Shoots out the grapple
    public void Shoot()
    {

        mDir = mPos - (Vector2)goPlayer.transform.position;

        mDir.Normalize();

        //Quaternion projLoc = Quaternion.Euler(0, 0, Mathf.Atan2(mDir.y, mDir.x) * Mathf.Rad2Deg);
    
        //Creates a new grapple object if there is none
        if (cBall == null)
        {
            cBall = Instantiate(grappleObj, transform.position, Quaternion.identity);
            cBall.GetComponent<Grapple>().tempobj = gameObject;
            cBall.GetComponent<Grapple>().followobj = rotObject.gameObject;
        }
            Shoot1();
        
    }

    void ScriptNormSet()
    {
        goPlayer.GetComponent<Controller2D>().enabled = true;
        goPlayer.GetComponent<Player>().enabled = true;
        goPlayer.GetComponent<PlayerController>().enabled = false;
        //rbPlayer.bodyType = RigidbodyType2D.Static;
        rbPlayer.velocity = rbPlayer.velocity / 2;
        bstart = 0;
    }
    public void Shoot1()
    {
        rc2dRaycast = Physics2D.Raycast(transform.position, mDir, fHoldDistance, lmLayerMask);
        if (rc2dRaycast.collider != null && rc2dRaycast.collider.gameObject.GetComponent<Rigidbody2D>() != null)
        {
                //Enables the joint for rotation
                dj2dJoint.enabled = true;

                //Connects Grapple with the rigidbody
                dj2dJoint.connectedBody = rc2dRaycast.collider.gameObject.GetComponent<Rigidbody2D>();

                //Connects the Anchor
                dj2dJoint.connectedAnchor = rc2dRaycast.point - new Vector2(rc2dRaycast.collider.transform.position.x, rc2dRaycast.collider.transform.position.y);

            dj2dJoint.distance = grappleObj.GetComponent<Grapple>().CurrentDist;
            //Enables line drawing
            lrLineRenderer.enabled = true;
        }
    }

    public void DrawLine()
    {
        if (lrLineRenderer == null)
            return;
        if (cBall == null)
            return;

        //Sets the position of the player and the pivot point for the line to draw
        lrLineRenderer.SetPosition(0, goPlayer.transform.position);
        lrLineRenderer.SetPosition(1, cBall.transform.position);
    }

    //Stops the shooting of the Grapple
    public void StopShoot()
    {
        if (cBall != null)
            Destroy(cBall);

        dj2dJoint.enabled = false;
        lrLineRenderer.enabled = false;
    }
}
