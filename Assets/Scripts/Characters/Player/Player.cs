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

    float colcd = 0;

    float grapAngle = 1;

    float targetVelocityX = 0;

    bool goingback = false;

    int CurrentDir = 1;

    //Checks if the player has started grappling
    float CountCheck = 0;

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
        bool bGrappling = shootOBJ.cBall && shootOBJ.cBall.GetComponent<Grapple>().GrapConnected == true;

        //if (controller.collisions.left && bGrappling && colcd == 0|| controller.collisions.right && bGrappling && colcd == 0)
        //{
        //    float storevel = velocity.x;
        //    velocity.x = -storevel;
        //    colcd += Time.deltaTime;
        //}

        if (colcd >= 1)
        {
            colcd = 0;
        }

        if (shootOBJ.cBall != null)
            grapAngle = Quaternion.Angle(transform.rotation, shootOBJ.cBall.transform.rotation);

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
        //if (velocity.y < -20)
            //velocity.y = -20;

        if (controller.collisions.below && !bGrappling)
        {
            moveSpeed = defaultSpeed;
            targetVelocityX = 0.0f;
            targetVelocityX = input.x * moveSpeed;
        }
        else if(bGrappling)
        {
            //if (CountCheck > 1)
            //{
            gameObject.GetComponentInChildren<FollowArrow>().JoystickStore.x = input.x;
            targetVelocityX += (input.x) * moveSpeed * Time.deltaTime * inAirModifier;
            //}
            //else
            //{
            //    targetVelocityX = 0;
            //}
            //CountCheck += Time.deltaTime;

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
        if (controller.collisions.below == false || controller.collisions.above == false)
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

        if (bGrappling && !gameObject.GetComponent<PlayerInput>().isMoving)
        {
            if (Mathf.Sign(targetVelocityX) < 0 && transform.position.x != shootOBJ.GetComponent<ShootOBJ>().cBall.transform.position.x)
            {
                targetVelocityX += (moveSpeed + grapAngle) * Time.deltaTime * 10.0f;
                goingback = true;
            }
            else
            {
                targetVelocityX -= (moveSpeed + grapAngle) * Time.deltaTime * 10.0f;
                goingback = false;
            }
        }

        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref XSmoothing, (controller.collisions.below) ? acceleratinTimeGrounded : accelerationTimeAirbourne);

        //Debug.Log(velocity.x);
        if (!bGrappling || gameObject.GetComponentInChildren<ShootOBJ>().IsShooting)
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

        if (gameObject.GetComponent<PlayerInput>().justReleased)
        {
            if (velocity.x > fPlayerMaxSpeed)
            {
                velocity.x = fPlayerMaxSpeed;//calcSpeedOutOfJump();
            }
            if (velocity.x < -fPlayerMaxSpeed)
            {
                velocity.x = -fPlayerMaxSpeed; //-calcSpeedOutOfJump();
            }
        }

        controller.Move(velocity * Time.deltaTime);

        //playerdirection = Mathf.Sign(velocity.x);

        if (gameObject.GetComponentInChildren<FollowArrow>().JoystickStore.x > 0 && !isturned)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            isturned = true;
            //CurrentDir = -1;
        }

        if (gameObject.GetComponentInChildren<FollowArrow>().JoystickStore.x < 0 && isturned)
        {
            transform.localScale = new Vector3(1, 1, 1);
            isturned = false;
            //CurrentDir = 1;
        }

        //transform.right = new Vector3(CurrentDir, 0, 0);

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
        speed = -(velocity.x / velocity.y);


        return speed;
    }
}

