using System.Collections;
using System.Collections.Generic;
using XboxCtrlrInput;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
//[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerController))]
//[RequireComponent(typeof(DistanceJoint2D))]

public class PlayerOld : MonoBehaviour {

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
    //[HideInInspector]
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
    [Tooltip("This will increase the speed that the player accelerates at as they swing")]
    public float inAirModifier = 20.0f;
    [Tooltip("This effects the distance the player can swing")]
    public float MaxInAirSpeed = 100.0f;


    float percentagedecrease = 1.0f;

    float colcd = 0;

    float grapAngle = 1;

    float targetVelocityX = 0;

    bool goingback = false;


    //Checks if the player has started grappling

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
        if (Time.timeScale == 0)
            return;

        bool bGrappling = shootOBJ.cBall && shootOBJ.cBall.GetComponent<Grapple>().GrapConnected == true;

        if (colcd >= 1)
        {
            colcd = 0;
        }

        if (shootOBJ.cBall != null)
            grapAngle = Quaternion.Angle(transform.rotation, shootOBJ.cBall.transform.rotation);

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
           // gameObject.GetComponent<PlayerInput>().canInput = true;
        }

        if (CanJump == true && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }

        
        velocity.y += gravity * Time.deltaTime;
        if (velocity.y < -20)
            velocity.y = -20;

        if (controller.collisions.below && !bGrappling)
        {
           // moveSpeed = defaultSpeed;
            targetVelocityX = 0.0f;
            targetVelocityX = input.x * moveSpeed;
        }
        else if(bGrappling)
        {
            gameObject.GetComponentInChildren<FollowArrow>().JoystickStore.x = input.x;
            //***RETURN TO THIS!!***
            targetVelocityX += input.x * moveSpeed * Time.deltaTime * inAirModifier;

            velocity.y = 0;

        }
        else if (bGrappling && controller.collisions.below)
        {
            targetVelocityX = 0.0f;
            targetVelocityX = input.x * moveSpeed;
        }
        else
        {
            targetVelocityX = 0.0f;
            targetVelocityX = input.x * moveSpeed;
        }

        //--------------------
        //In air Max Speed
        //--------------------
        if (!controller.collisions.below || !controller.collisions.above)
        {
            if (velocity.x > MaxInAirSpeed)
                velocity.x = MaxInAirSpeed;
            if (velocity.y > MaxInAirSpeed)
                velocity.y = MaxInAirSpeed;

            if (velocity.x < -MaxInAirSpeed)
                velocity.x = -MaxInAirSpeed;
            if (velocity.y < -MaxInAirSpeed)
                velocity.y = -MaxInAirSpeed;
        }



        //--------------------
        //On Land Max Speed
        //--------------------
        if (controller.collisions.below == true || controller.collisions.above == true)
        {
            if (velocity.x > fPlayerMaxSpeed)
                velocity.x = fPlayerMaxSpeed;
            if (velocity.y > fPlayerMaxSpeed)
                velocity.y = fPlayerMaxSpeed;

            if (velocity.x < -fPlayerMaxSpeed)
                velocity.x = -fPlayerMaxSpeed;
            if (velocity.y < -fPlayerMaxSpeed)
                velocity.y = -fPlayerMaxSpeed;
        }

        //-----------------------------------------------
        //if the player is grappling and is on the ground
        //-----------------------------------------------
        if(bGrappling && controller.collisions.below)
        {
            if (velocity.x > fPlayerMaxSpeed)
                velocity.x = fPlayerMaxSpeed;
            if (velocity.y > fPlayerMaxSpeed)
                velocity.y = fPlayerMaxSpeed;

            if (velocity.x < -fPlayerMaxSpeed)
                velocity.x = -fPlayerMaxSpeed;
            if (velocity.y < -fPlayerMaxSpeed)
                velocity.y = -fPlayerMaxSpeed;
        }

        //***LOOK OVER THIS!!!***
        //-----------------------------------
        //This makes the character swing when 
        //the character is idling.
        //------------------------------------
        if (bGrappling )//&& !gameObject.GetComponent<PlayerInput>())//.isMoving)
        {
            if (Mathf.Sign(targetVelocityX) < 0 && transform.position.x != shootOBJ.GetComponent<ShootOBJ>().cBall.transform.position.x)
            {
                targetVelocityX += (moveSpeed + grapAngle) * Time.deltaTime * 10.0f * percentagedecrease;
                goingback = true;
            }
            else if (velocity.x == 0)
            {
                velocity.y = 0.0f;
            }
            else
            {
                targetVelocityX -= (moveSpeed + grapAngle) * Time.deltaTime * 10.0f * percentagedecrease;
                goingback = false;
            }
        }
        else if (bGrappling && gameObject.GetComponent<PlayerInput>())//.isMoving)
        {
            targetVelocityX = Mathf.Sign(velocity.x) * moveSpeed * Time.deltaTime;
        }

        if (!gameObject.GetComponent<PlayerInput>())//.isMoving && gameObject.GetComponent<Controller2D>().collisions.IsDying)
        {
            velocity.x = 0;
        }

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref XSmoothing, (controller.collisions.below) ? acceleratinTimeGrounded : accelerationTimeAirbourne);

        if (gameObject.GetComponentInChildren<ShootOBJ>().cBall != null)
        {
            if (gameObject.GetComponentInChildren<ShootOBJ>().cBall.GetComponent<Grapple>().GrapConnected)
            {
                if (velocity.x > MaxInAirSpeed)
                    velocity.x = MaxInAirSpeed;
                if (velocity.y > MaxInAirSpeed)
                    velocity.y = MaxInAirSpeed;

                if (velocity.x < -MaxInAirSpeed)
                    velocity.x = -MaxInAirSpeed;
                if (velocity.y < -MaxInAirSpeed)
                    velocity.y = -MaxInAirSpeed;
            }
        }

        if (gameObject.GetComponent<PlayerInput>())//.justReleased)
        {
            if (velocity.x > fPlayerMaxSpeed)
            {
                velocity.x = fPlayerMaxSpeed;
            }
            if (velocity.x < -fPlayerMaxSpeed)
            {
                velocity.x = -fPlayerMaxSpeed;
            }
        }

        if (velocity.x > 0 && !isturned)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            isturned = true;
        }

        if (velocity.x < 0 && isturned)
        {
            transform.localScale = new Vector3(1, 1, 1);
            isturned = false;
        }

        //controller.Move(velocity * Time.deltaTime);

        //isDead = controller.collisions.IsDying;
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
        speed = -(velocity.x / velocity.y);


        return speed;
    }
}

