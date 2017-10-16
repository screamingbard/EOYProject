using System.Collections;
using System.Collections.Generic;
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
    float moveSpeed = 6;

    float jumpVelocity;
    float gravity;
    [HideInInspector]
    public Vector3 velocity;
    float XSmoothing;

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

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref XSmoothing, (controller.collisions.below) ? acceleratinTimeGrounded : accelerationTimeAirbourne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
