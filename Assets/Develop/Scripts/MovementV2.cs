using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* TO DO
 * 
 * Få dash til å fungere ordentlig mot bevegelsesretningen
 *     Skal kanskje være teleport, og er i så fall unødvendig
 *     Hvis teleport: Ledge cancel mechanic?
 * 
 * Flere abilities
 *     Egne scripts?
 * 
 * Walljump koden kan gjøres mer elegant
 * 
*/

public class MovementV2 : MonoBehaviour
{
    private Rigidbody2D rb;

    // Skal ikke være readonly i spillet, ettersom spilleren får tak i abilities underveis
    private readonly bool CanWalljump = true;
    private readonly bool CanDash = true;

    public int airJumps = 2;

    public float moveSpeed = 8f;
    public float jumpSpeed = 15f;
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float wallJumpDuration = 0.2f;
    public float ourGravity = 4f;
    public float gravityChange = 1.3f;
    public float maxVelocityY = 12f;
    private float maxVelocityFix = 1f;

    public bool isGrounded = false;
    public bool wallHit = false;
    public bool hasJumped = false;
    public bool hasDashed = false;
    private bool jumping = false;
    private bool dashing = false;
    private bool wallJumping = false;
    private bool wallStick = false;

    public float lastJumpTime;
    public float dashTime;
    public float wallJumpTime = -1;
    public int jumpsSinceGround;

    public float scale;

    private Vector2 velocity;
    private float moveHorizontal;
    private float lastMove = 1;

    public int wallCollider;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = ourGravity;
        scale = (rb.transform.localScale).x;
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        bool jumpKeyDown = Input.GetKeyDown(KeyCode.Space);
        bool shiftKeyDown = Input.GetKeyDown(KeyCode.LeftShift);

        if (rb.velocity.y + maxVelocityY * Math.Sign(ourGravity) < 0)
        {
            maxVelocityFix = 0.9f;
        }
        else
        {
            maxVelocityFix = 1f;
        }

        if (!dashing)
        {
            moveHorizontal = Input.GetAxis("Horizontal");
            //moveHorizontal = 1;
            velocity = new Vector2(moveHorizontal * moveSpeed, rb.velocity.y);

            if (moveHorizontal != 0)
            {
                lastMove = Math.Sign(moveHorizontal);

                if (Math.Abs(moveHorizontal) > 0.3f)
                {
                    moveHorizontal = Math.Sign(moveHorizontal);
                }
            }
        }

        // Walljump resetter Dash og du kan alltid bruke minst ett airjump etter dash (så lenge det er unlocket)
        if (wallHit)
        {
            if (!wallJumping)
            {
                if (CanWalljump && jumpKeyDown)
                {
                    wallJump(wallCollider);
                    wallJumpTime = Time.time;
                    lastJumpTime = Time.time;

                    if (jumpsSinceGround == airJumps && airJumps != 0)
                    {
                        jumpsSinceGround--;
                    }

                    hasDashed = false;

                    return;
                }

                else if (rb.velocity.y <= 0 && wallCollider == -Math.Sign(moveHorizontal))
                {
                    rb.sharedMaterial.friction = 0.4f;
                }

                else
                {
                    rb.sharedMaterial.friction = 1f;
                    maxVelocityFix = 0.2f;
                }

            }

            

        }

        // DETTE ER STYGT
        else if (maxVelocityFix < 0.8)
        {
            maxVelocityFix = 1f;
        }

        if (wallJumping)
        {
            if (Time.time - wallJumpTime <= wallJumpDuration / 2)
            {
                wallJump(wallCollider);
            }

            else if (Time.time - wallJumpTime <= wallJumpDuration)
            {
                wallJump(Math.Sign(moveHorizontal));
            }
            else
            {
                wallJumping = false;
            }

            return;
        }


        if (jumpKeyDown && !wallJumping || jumping)
        {
            if (isGrounded)
            {
                jump(1.0f);
                jumpsSinceGround = 0;
            }

            else if (Time.time - lastJumpTime >= 0.2f || rb.gravityScale == ourGravity * gravityChange)
            {
                if (jumpsSinceGround == airJumps - 1)
                {
                    jump(0.8f);
                }
                else if (jumpsSinceGround < airJumps)
                {
                    jump(1.0f);
                }
            }
        }

        // DASH ELLER TELEPORT? 
        if (!hasDashed && shiftKeyDown && CanDash)
        {
            hasDashed = true;
            dashTime = Time.time;
            dashing = true;
            dash();
        }
        else if (dashing && Time.time - dashTime <= dashDuration)
        {
            dash();
        }
        else
        {
            dashing = false;
            rb.gravityScale = ourGravity;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2 (velocity.x, velocity.y * maxVelocityFix);
        
        if (isGrounded && rb.velocity.y == 0)
        {
            grounded();
        }
        else if (wallJumping)
        {
            rb.gravityScale = ourGravity;
        }

        //jumpsSinceGround <= airJumps - 1  && ...
        else if ((jumpsSinceGround <= airJumps - 1 && rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space)) || rb.velocity.y < 0.1f)
        {
            rb.gravityScale = ourGravity * gravityChange;
        }

        jumping = false;

        //print(rb.velocity.y);
    }

    private void grounded()
    {
        lastJumpTime = 0;
        jumpsSinceGround = 0;
        hasJumped = false;
        hasDashed = false;
        rb.gravityScale = ourGravity;
    }

    private void jump(float scale)
    {
        velocity = new Vector2(moveHorizontal * moveSpeed, jumpSpeed * scale);
        hasJumped = true;
        lastJumpTime = Time.time;
        jumpsSinceGround++;
        jumping = true;
        rb.gravityScale = ourGravity;
    }

    private void wallJump(int x)
    {
        if (moveHorizontal != 0)
        {
            velocity = new Vector2(x * dashSpeed / 2, jumpSpeed * 0.64f);
        }

        else
        {
            velocity = new Vector2(x * moveSpeed / 2, jumpSpeed * 0.64f);
        }
        wallJumping = true;
    }

    private void dash()
    {
        rb.gravityScale = 0;
        if (jumping)
        {
            velocity += new Vector2(lastMove * dashSpeed, 0);
            dashing = false;
        }

        else
        {
            velocity = new Vector2(lastMove * dashSpeed, 0);
        }
    }
    
}
