using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float moveSpeed = 12f;
    public float jumpSpeed = 20f;
    public float dashSpeed = 30f;
    public float dashDuration = 0.2f;
    public float wallJumpDuration = 0.2f;
    public float ourGravity = 5f;
    public float gravityChange = 1.3f;

    public bool isGrounded = false;
    public bool wallHit = false;
    public bool hasJumped = false;
    public bool hasDashed = false;
    private bool jumping = false;
    private bool dashing = false;
    private bool wallJumping = false;

    public float lastJumpTime;
    public float dashTime;
    public float wallJumpTime = -1;
    public int jumpsSinceGround;

    public float scale;

    private Vector2 velocity;
    private float moveHorizontal;

    public string wallCollider = null;

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

        if (!dashing && !wallJumping)
        {
            moveHorizontal = Input.GetAxis("Horizontal");
            //moveHorizontal = 1;
            velocity = new Vector2(moveHorizontal * moveSpeed, rb.velocity.y);
        }

        // Walljump resetter Dash og du kan alltid bruke minst ett airjump etter dash (så lenge det er unlocket)
        if (wallHit)
        {
            if (CanWalljump)
            {
                if (jumpKeyDown || wallJumping)
                {
                    if (wallCollider == "Wall Collider Left")
                    {
                        wallJump(1);
                    }
                    else
                    {
                        wallJump(-1);
                    }
                    wallJumpTime = Time.time;

                    if (jumpsSinceGround == airJumps && airJumps != 0)
                    {
                        jumpsSinceGround--;
                    }

                    hasDashed = false;
                }
            }

            // LEGG TIL WALLSLIDE

        }

        if (wallJumping)
        {
            if (Time.time - wallJumpTime <= wallJumpDuration)
            {
                if (wallCollider == "Wall Collider Left")
                {
                    wallJump(1);
                }
                else
                {
                    wallJump(-1);
                }
            }
            else
            {
                wallJumping = false;
            }
        }


        if (jumpKeyDown && !wallJumping || jumping)
        {
            if (isGrounded)
            {
                jump(1.0f);
                jumpsSinceGround = 0;
            }

            else if (Time.time - lastJumpTime >= 0.3f || rb.gravityScale == ourGravity * gravityChange)
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
        }
    }

    void FixedUpdate()
    {
        rb.velocity = velocity;
        jumping = false;

        if (isGrounded && rb.velocity.y == 0)
        {
            grounded();
        }
        else if (jumping || wallJumping)
        {
            rb.gravityScale = ourGravity;
        }

        else if ((jumpsSinceGround <= airJumps - 1 && rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space)) || rb.velocity.y < 0.2f)
        {
            rb.gravityScale = ourGravity * gravityChange;
        }
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
        velocity = new Vector2(x * moveSpeed / 2, jumpSpeed);
        wallJumping = true;
        lastJumpTime = Time.time;
    }

    private void dash()
    {
        if (jumping)
        {
            velocity += new Vector2(moveHorizontal * dashSpeed, 0);
            dashing = false;
        }

        else
        {
            velocity = new Vector2(moveHorizontal * dashSpeed, 0);
        }
    }
}
