using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

struct Abilities
{
    public bool CanShrink;
    public bool CanWallclimb;
    public bool CanTriplejump;
    public bool CanDoublejump;
    public bool CanDash;
    //public intk

    public Abilities(bool canShrink, bool canWallclimb, bool canTriplejump,
                     bool canDoublejump, bool canDash)
    {
        CanShrink = canShrink;
        CanWallclimb = canWallclimb;
        CanTriplejump = canTriplejump;
        CanDoublejump = canDoublejump;
        CanDash = canDash;
    }
}

public enum State
{
    PRE_STAGE = -1, STAGE_1 = 0, STAGE_2 = 1, STAGE_3 = 2, STAGE_4 = 3, STAGE_5 = 4, STAGE_END
};

public class Movement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 20;
    public float dashSpeed = 30;
    public float dashGravityTime = 0.2f;
    public float ourGrav = 5f;
    public float changeGrav = 1.5f;

    private Rigidbody2D rigidBody;
    public bool isGrounded = false;
    public bool hasJumped = false;
    public bool hasDashed = false;
    private bool jumping = false;
    private bool dashing = false;

    public float lastJumpTime;
    public float dashTime;
    public float shrinkTime;
    public int jumpsSinceGround;

    public float shrunkScale;
    public float originalScale;
    public bool shrunk = false;
    private float currentScale;

    private Vector2 velocity;
    private Vector2 force;


    public Animator animController;
    private float moveHorizontal;

    public State state;

    Abilities[] stageAbilities = new Abilities[]{
        new Abilities(false, false, false, false, false),
        new Abilities(true,  true,  true,  false, true),
        new Abilities(false, true,  true,  false, true),
        new Abilities(false, true,  false, true,  true),
        new Abilities(false, true,  false, false, true)
    };

    // Called on next stage, resets all states
    void nextStage()
    {
        velocity = new Vector2(0.0f, 0.0f);
        force = new Vector2(0.0f, 0.0f);

        currentScale = originalScale;
        shrunkScale = 0.5f * currentScale;


        if (state < State.STAGE_END)
        {
            state = (State)(((int)state) + 1); // TODO: confirm implicit conversion
        }

        resetJumps();
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animController = GetComponent<Animator>();
        originalScale = (rigidBody.transform.localScale).x;
        rigidBody.gravityScale = ourGrav;

        // Initialize the stage
        state = State.PRE_STAGE;
        nextStage();
        nextStage();
    }
    //nextStage TO GANGER

    void Update()
    {

        if (rigidBody.transform.localPosition.y <= -11)
        {
            SceneManager.LoadScene("I guess");
        }

        bool jumpKeyDown = Input.GetKeyDown(KeyCode.Space);
        bool shiftKeyDown = Input.GetKeyDown(KeyCode.LeftShift);
        bool sKeyDown = Input.GetKeyDown(KeyCode.S);

        if (!dashing)
        {
            moveHorizontal = Input.GetAxis("Horizontal");
            //moveHorizontal = 1;
            velocity = new Vector2(moveHorizontal * speed, rigidBody.velocity.y);
        }

       /*
        if (velocity.y > 0.1f)
        {
            animController.SetTrigger("jump");
        } else if (velocity.y < -0.1f)
        {
           animController.SetTrigger("dash");
        } else
        {
            if (Mathf.Abs(moveHorizontal) > 0.1f)
                animController.SetTrigger("run");
            else
                animController.SetTrigger("idle");
        }
        */

        if (isGrounded)
        {
            hasDashed = false;
        }

        Abilities abilities = stageAbilities[(int)state];
        if (abilities.CanShrink && sKeyDown && Time.time - shrinkTime >= 0.5f)
        {
            shrink();
        }

        if (abilities.CanWallclimb)
        {
            // NOT GONNA IMPLEMENT
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            nextStage();
        }


        //Gjøre at isGrounded kan returnere false for walkoff buggen

        if (jumpKeyDown)
        {
            if (isGrounded)
            {
                // In air: double-, triple jump or air dash
                jump(1.0f);
            }

            else if (Time.time - lastJumpTime >= 0.2f * (currentScale / originalScale))
            {
                if (abilities.CanTriplejump && jumpsSinceGround < 3)
                {
                    jump(1.2f);
                }
                else if (abilities.CanDoublejump && jumpsSinceGround < 2)
                {
                    jump(1.2f);
                }

            }

        }

        if (!hasDashed && shiftKeyDown && abilities.CanDash)
        {
            hasDashed = true;
            dashTime = Time.time;
            //rigidBody.gravityScale = 0;
            dashing = true;
            dash();
        }
        else if (dashing && Time.time - dashTime <= dashGravityTime)
        {
            dash();
        }      
        else
        {
            //Fant og fjernet en disable gravity bug, feature?
            dashing = false;
            //rigidBody.gravityScale = ourGrav;
        }
    }

    void FixedUpdate()
    {
        rigidBody.velocity = velocity;
        //rigidBody.AddForce(-Vector2.up * 10f * (currentScale / originalScale));
        rigidBody.AddForce(force);
        force = new Vector2(0f, 0f);
        jumping = false;


        if (hasJumped && rigidBody.velocity.y <= 0.2f)
        {
            rigidBody.gravityScale = ourGrav * changeGrav;
        }
    }

    public void OnGround(GameObject groundObject)
    {
        print("onGround");
        isGrounded = true;
        resetJumps();
        rigidBody.gravityScale = ourGrav;
    }

    public void hitWall(GameObject wallHit)
    {
        SceneManager.LoadScene("I guess");
    }


    private void resetJumps()
    {
        lastJumpTime = 0;
        jumpsSinceGround = 0;
        hasJumped = false;
    }


    private void shrink()
    {
        shrinkTime = Time.time;
        if (!shrunk)
        {
            transform.localScale = new Vector3(shrunkScale, shrunkScale, originalScale);
            shrunk = true;
            currentScale = shrunkScale;
        }
        else
        {
            transform.localScale = new Vector3(originalScale, originalScale, originalScale);
            shrunk = false;
            currentScale = originalScale;
        }
    }

    //Mer jumpForce når i luften (for double/triple jump)
    //Hopp buggen som aldri forsvinner
    private void jump(float scale)
    {
        //rigidBody.AddForce(new Vector2(0, 1) * jumpForce);
        velocity = new Vector2(0, jumpSpeed) * (currentScale / originalScale) * scale;
        isGrounded = false;
        hasJumped = true;
        lastJumpTime = Time.time;
        jumpsSinceGround++;
        jumping = true;
        rigidBody.gravityScale = ourGrav;
    }

    private void dash()
    {
        if (jumping)
        {
            velocity += new Vector2(moveHorizontal * dashSpeed * 2, 0);
            dashing = false;
        }

        else
        {
            velocity = new Vector2(moveHorizontal * dashSpeed, 0);
        }
    }
}
