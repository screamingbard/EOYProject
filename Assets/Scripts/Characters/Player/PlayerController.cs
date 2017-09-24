using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float fSpeed = 15.0f;
    public float fJumpForce = 11.0f;
    public float fSJump = 2.0f;
    
    public float DistOverGround = 0.1f;
    
    //checks if on the ground
    public bool IsGrounded = false;

    Rigidbody2D rb2D = null;
    RaycastHit2D hit;
    // Use this for initialization
    void Awake () {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        fSJump = fSJump * rb2D.mass;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        hit = Physics2D.Raycast(transform.position, -Vector2.up, DistOverGround);;
        if(hit == false)
        {
            IsGrounded = false;
        }


        float HMove = Input.GetAxis("Horizontal");
        rb2D.velocity = new Vector2(HMove * fSpeed, rb2D.velocity.y);

        if (IsGrounded)
        {
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
    }
    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if(collision2D.gameObject.tag == "Wall")
            IsGrounded = true;
    }
}
