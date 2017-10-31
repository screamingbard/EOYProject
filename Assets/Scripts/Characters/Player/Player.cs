using System.Collections;
using System.Collections.Generic;
using XboxCtrlrInput;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(DistanceJoint2D))]

public class Player : MonoBehaviour {

    public float jumpHeight = 4.0f;
    public float timeToJump = 0.4f;
    float accelerationTimeAirbourne = 0.2f;
    float acceleratinTimeGrounded = 0.1f;
    public float moveSpeed = 6;
    float defaultSpeed;

    //[HideInInspector]
    public bool isDead = false;

    float jumpVelocity;
    float gravity;
    [HideInInspector]
    public Vector3 velocity;
    float XSmoothing;

    public float drag;
    public float playerdirection;
    bool isturned;

    [HideInInspector]
    public Vector2 input;
    [HideInInspector]
    public bool CanJump = false;

    public float fPlayerMaxSpeed = 30.0f;

    float targetVelocityX = 0;

    Controller2D controller;

    ShootOBJ shootOBJ;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJump, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJump;

        CanJump = false;

        defaultSpeed = moveSpeed;

        shootOBJ = GetComponentInChildren<ShootOBJ>();
    }

    void Update()
    {
        //Debug.Log(velocity.x + " ," + velocity.y);

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
            gameObject.GetComponent<PlayerInput>().canInput = true;
        }

        if (CanJump == true && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }

        
        velocity.y += gravity * Time.deltaTime;
        if (velocity.y < -50)
            velocity.y = -50;

        bool bGrappling = shootOBJ.cBall && shootOBJ.cBall.GetComponent<Grapple>().GrapConnected == true;

        if (controller.collisions.below && !bGrappling)
        {
            targetVelocityX = input.x * moveSpeed;

            //moveSpeed = defaultSpeed;
        }
        else if(bGrappling)
        {
            targetVelocityX += input.x * moveSpeed * Time.deltaTime * 10.0f;
        }
        else
        {
            targetVelocityX = input.x * moveSpeed;
        }


        if(bGrappling && !gameObject.GetComponent<PlayerInput>().isMoving)
        {
            if(Mathf.Sign(targetVelocityX) < 0)
            {
                targetVelocityX += moveSpeed * Time.deltaTime * 5.0f;
            }
            else
            {
                targetVelocityX -= moveSpeed * Time.deltaTime * 5.0f;
            }
        }
        //tst
        //if (bGrappling && gameObject.GetComponent<PlayerInput>().isMoving && controller.collisions.below)
        //{

        //}

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref XSmoothing, (controller.collisions.below) ? acceleratinTimeGrounded : accelerationTimeAirbourne);

        Debug.Log(velocity.x);
        if (!bGrappling)
        {
            if (velocity.x > 100)
                velocity.x = 100;
            if (velocity.y > 100)
                velocity.y = 100;

            if (velocity.x < -100)
                velocity.x = -100;
            if (velocity.y < -100)
                velocity.y = -100;
        }

        if (gameObject.GetComponent<PlayerInput>().justReleased)
        {
            if (velocity.x > fPlayerMaxSpeed)
            {
                velocity.x = calcSpeedOutOfJump();
            }
            if (velocity.x < -fPlayerMaxSpeed)
            {
                velocity.x = -calcSpeedOutOfJump();
            }
        }

        controller.Move(velocity * Time.deltaTime);

        //playerdirection = Mathf.Sign(velocity.x);

        //if (XCI.GetAxisRaw(XboxAxis.LeftStickX) > 0 && !isturned)
        //{
        //    transform.Rotate(0, 180, 0);
        //    isturned = true;
        //}

        //if (XCI.GetAxisRaw(XboxAxis.LeftStickX) < 0 && !isturned)
        //{
        //    transform.Rotate(0, 180, 0);
        //    isturned = false;
        //}

        controller.HorizontalDeathCollision(ref velocity);
        controller.VerticalDeathCollision(ref velocity);
        //DEBUGGING
        isDead = controller.collisions.IsDying;
    }
    float calcmaxspeed()
    {
        float storeNum = Mathf.Pow(fPlayerMaxSpeed, 2) / 2;
        float res = Mathf.Sqrt(storeNum);
        return res;
    }

    float calcSpeedOutOfJump()
    {
        float speed = 0;
        speed = -(velocity.x / velocity.y) * 12;


        return speed;
    }
}

