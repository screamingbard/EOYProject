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

    //[HideInInspector]
    public bool isDead = false;

    float jumpVelocity;
    float gravity;
    [HideInInspector]
    public Vector3 velocity;
    float XSmoothing;

    public float playerdirection;
    bool isturned = false;

    Controller2D controller;

    void Start()
    {
        controller = GetComponent<Controller2D>();
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJump, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJump;
    }

    void Update()
    {
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(XCI.GetAxisRaw(XboxAxis.LeftStickX), XCI.GetAxisRaw(XboxAxis.LeftStickY));//Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (XCI.GetButton(XboxButton.LeftBumper) && controller.collisions.below || XCI.GetButton(XboxButton.A) && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref XSmoothing, (controller.collisions.below) ? acceleratinTimeGrounded : accelerationTimeAirbourne);
        velocity.y += gravity * Time.deltaTime;

        if (velocity.x > 30)
            velocity.x = 30.0f;
        if (velocity.y > 30)
            velocity.y = 30.0f;

        if (velocity.x < -30)
            velocity.x = -30.0f;
        if (velocity.y < -30)
            velocity.y = -30.0f;

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
}
