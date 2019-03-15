using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* TO DO
 * ---
 * Disable moveHorizontal and dash when sliding down tilted surfaces !!!!!!!
 * ---
 * 
 * IDEAS
 * ---
 * Freeze X (to make aiming the hookshot easier with controls)
 * Make the hookshot snappy (like Shifu's E or Hornet's needle pull)
 * ---
 * 
 */

public class Movement : MonoBehaviour {

    public Rigidbody2D rb;

    public bool canDash = true;
    public bool canWalljump = true;
    public bool canAirJump = true;
    public bool canCrouch = true;
    public bool canGravityFlip = true;

    public bool isGrounded;
    public bool wallHit;
    public bool roofHit;
    public bool isCrouching;
    public bool takingDamage = false;

    //Public inntil bugtesting er over
    public bool hasJumped;
    public bool hasAirJumped;
    public bool hasDashed;
    public bool dashing;

    public int wallTrigger;
    public int ourGravity = 4;
    public int spriteDirection = 1;

    public float moveHorizontal;

    private bool jumping;
    private bool walljumping;

    public float moveSpeed = 7; //Skal verken være public eller float, kun intil TH fikser crouching
    private int dashSpeed = 13;
    private int maxVelocityY = 12;

    public float jumpSpeed = 13.5f; //Kun public mens jeg tester ting
    private float gravityChange = 1.3f;
    private float maxVelocityFix = 1f;

    private float jumpTime;
    private float dashTime;
    private float walljumpTime = -1;

    private float dashDuration = 0.2f;
    private float walljumpDuration = 0.2f;

    public Vector2 velocity;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = ourGravity;
        Application.targetFrameRate = 60;
    }
	
	void Update () {
        if (takingDamage)
        {
            Damaged(); //DO STUFF
            return;
        }

        bool jumpKeyDown = Input.GetKeyDown(KeyCode.Space);
        bool shiftKeyDown = Input.GetKeyDown(KeyCode.LeftShift);

        if (Math.Abs(moveHorizontal) <= Math.Abs(Input.GetAxis("Horizontal")))
        {
            moveHorizontal = Input.GetAxis("Horizontal"); //Fant en litt kul bug med dash. Beholde?
            if (Math.Abs(moveHorizontal) > 0.3f)
            {
                velocity = new Vector2(Math.Sign(moveHorizontal) * moveSpeed, rb.velocity.y);
            }
            else
            {
                velocity = new Vector2(moveHorizontal * moveSpeed, rb.velocity.y);
            }
        }
        else
        {
            velocity = new Vector2(0, rb.velocity.y);
            moveHorizontal = Input.GetAxis("Horizontal");
        }



        if (Math.Sign(ourGravity) == 1 && rb.velocity.y <= -maxVelocityY || Math.Sign(ourGravity) == -1 && rb.velocity.y >= maxVelocityY) {
            maxVelocityFix = 0.9f;
        }

        else {
            maxVelocityFix = 1f;
        }

        if (walljumping) {
            if (Time.time - walljumpTime <= walljumpDuration / 2) {
                Walljump(wallTrigger);
            }

            else if (Time.time - walljumpTime <= walljumpDuration) {
                Walljump(Math.Sign(moveHorizontal));
            }

            else {
                walljumping = false;
            }
            return;
        }

        if (wallHit) {
            if (canWalljump && jumpKeyDown) {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                Walljump(wallTrigger);
                walljumpTime = Time.time;
                jumpTime = Time.time;

                walljumping = true;
                moveHorizontal = spriteDirection;

                return;
            }

            else if (Math.Abs(rb.velocity.y) < 3 && wallTrigger == -Math.Sign(velocity.x)) {
                rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            }

            else {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                maxVelocityFix = 0.2f;
            }

            hasAirJumped = false;
            hasDashed = false;
        }

        if (jumpKeyDown || jumping) {
            if (isGrounded) {
                Jump(1.0f);
                hasJumped = true;
            }

            else if (!hasAirJumped && (Time.time - jumpTime >= 0.1f || rb.gravityScale == ourGravity * gravityChange)) {
                Jump(0.8f);
                hasAirJumped = true;
            }
        }

        if (shiftKeyDown && !hasDashed) { // && canDash
            hasDashed = true;
            dashTime = Time.time;
            dashing = true;
            Dash();
            return;
        }
        else if (dashing && Time.time - dashTime <= dashDuration) {
            Dash();
            return;
        }
        else {
            dashing = false;
        }

        if (roofHit) {
            velocity = new Vector2(velocity.x / 2, 0);
        }
    }

    void FixedUpdate() {
        rb.velocity = new Vector2(velocity.x * Math.Sign(ourGravity), velocity.y * maxVelocityFix);

        if (dashing) {
            rb.gravityScale = 0;
        }

        else if (isGrounded) {
            Grounded();
        }

        else if (walljumping || jumping) {
            rb.gravityScale = ourGravity;
        }

        else if ((!hasAirJumped && rb.velocity.y * Math.Sign(ourGravity) > 0 && !Input.GetKey(KeyCode.Space)) || rb.velocity.y * Math.Sign(ourGravity) < 0.1f) {
            rb.gravityScale = ourGravity * gravityChange;
        }

        jumping = false;
    }

    private void Grounded() {
        jumpTime = 0;
        hasDashed = false;
        hasJumped = false;
        hasAirJumped = false;
        rb.gravityScale = ourGravity;
    }

    private void Jump(float scale) {
        velocity = new Vector2(moveHorizontal * moveSpeed, jumpSpeed * scale * Math.Sign(ourGravity));
        jumpTime = Time.time;
        jumping = true;
    }

    private void Walljump(int x) {
        if (moveHorizontal != 0) {
            velocity = new Vector2(x * dashSpeed / 2, jumpSpeed * 0.64f * Math.Sign(ourGravity));
        }

        else {
            velocity = new Vector2(x * moveSpeed / 2, jumpSpeed * 0.75f * Math.Sign(ourGravity)); // Hopper drithøyt, wtf?
        }
    }

    private void Dash() {
        if (jumping) {
            velocity += new Vector2(spriteDirection * dashSpeed, 0);
            dashing = false;
        }

        else {
            velocity = new Vector2(spriteDirection * dashSpeed, 0);
        }

        moveHorizontal = spriteDirection;
    }

    private void Damaged() {
        if (rb.velocity.y * Math.Sign(ourGravity) > 0)
        {
            isGrounded = false;
        }
        hasAirJumped = false;
        hasDashed = false;
        //velocity = rb.velocity;
    }
}