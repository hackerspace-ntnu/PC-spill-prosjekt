using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* TO DO
 * 
 * Fikse det jævla Collision Detector scriptet
 *     Dermed fikse walkoff-buggen
 *     groundObject = null
 *         :ANGERY:
 * 
 * Få dash til å fungere ordentlig mot bevegelsesretningen
 *     Skal kanskje være teleport, og er i så fall unødvendig
 * 
 * Flere abilities
 *     Egne scripts?
 * 
*/

public class MovementV2 : MonoBehaviour {

    private Rigidbody2D rigidBody;

    private bool CanWallclimb = true;
    private bool CanDash = true;

    public int airJumps = 2;

    public float moveSpeed = 12f;
    public float jumpSpeed = 20f;
    public float dashSpeed = 30f;
    public float dashDuration = 0.2f;
    public float ourGravity = 5f;
    public float gravityChange = 1.3f;

    public bool isGrounded = false;
    public bool hasJumped = false;
    public bool hasDashed = false;
    private bool jumping = false;
    private bool dashing = false;

    public float lastJumpTime;
    public float dashTime;
    public int jumpsSinceGround;

    public float scale;

    private Vector2 velocity;
    private float moveHorizontal;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.gravityScale = ourGravity;
        scale = (rigidBody.transform.localScale).x;
    }
	
	// Update is called once per frame
	void Update () {
        bool jumpKeyDown = Input.GetKeyDown(KeyCode.Space);
        bool shiftKeyDown = Input.GetKeyDown(KeyCode.LeftShift);

        if (!dashing && !jumping)
        {
            moveHorizontal = Input.GetAxis("Horizontal");
            //moveHorizontal = 1;
            velocity = new Vector2(moveHorizontal * moveSpeed, rigidBody.velocity.y);
        }

        if (CanWallclimb)
        {
            // NOT IMPLEMENTED
        }

        //Gjøre at isGrounded kan returnere false for walkoff buggen

        if (jumpKeyDown)
        {
            if (isGrounded)
            {
                jump(1.0f);
                jumpsSinceGround = 0;
            }

            else if (Time.time - lastJumpTime >= 0.2f && jumpsSinceGround < airJumps)
            {
                jump(1.2f);
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
        rigidBody.velocity = velocity;
        jumping = false;

        if (!isGrounded && rigidBody.velocity.y <= 0.2f)
        {
            rigidBody.gravityScale = ourGravity * gravityChange;
        }
    }

    public void OnGround(GameObject groundObject)
    {
        print("onGround");
        isGrounded = true;
        grounded();
        rigidBody.gravityScale = ourGravity;
    }

    private void grounded()
    {
        lastJumpTime = 0;
        jumpsSinceGround = 0;
        hasJumped = false;
        hasDashed = false;
    }

    private void jump(float scale)
    {
        velocity = new Vector2(0, jumpSpeed);
        isGrounded = false;
        hasJumped = true;
        lastJumpTime = Time.time;
        jumpsSinceGround++;
        jumping = true;
        rigidBody.gravityScale = ourGravity;
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
