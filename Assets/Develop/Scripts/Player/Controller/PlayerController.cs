using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    // Models
    PlayerModel model; // All variables related to player. Doesnt do any logic, just hold variables. Used ALOT. 

    // Subcontrollers - contains logic that affect the various model states ( jump controller for jump logic etc.)
    LifeController life; // Controller of life and death
    JumpController jump; // Kangaroo controller.
    HorizontalDirectionController move; // general sideway movement, on the ground but also in the air!
    DashController dash; // You wont be sonic, but pretty close.
    GraphlingHookController graphHook; // For fishing and doing spiderman things. 
    ActionController action; // Lets you have action... someday.
    WallClingController wallCling; // Lets you be even more like spiderman.


    // Views 
    Rigidbody2D body; // The rigidbody of our player. 

    // Inputs
    private float horizontalInput; // a value between -1 to +1, in float value (0.0043) for example. -1 is to the left. + 1 to the right.
    private float verticalInput; // a value between -1 to +1, in float value (0.0043) for example. -1 is down. + 1 is up.
    private bool dashInput; // is key pressed?
    private bool jumpInput; // is key pressed?
    private bool graphHookInput; // is key pressed?


    // Start is called before the first frame update
    void Start()
    {
        body = GameObject.Find("View").GetComponent<Rigidbody2D>();
        model = GameObject.Find("Models").GetComponent<PlayerModel>();
        GetPlayerSubcontrollers();
        model.NewVelocity = body.velocity;
        model.NewGravityScale = model.BaseGravityScale * model.FlipGravityScale;
        AssignInterfaces();
        InitializePlayerState();
        InitializeStandardInputValues();
    }

    private void InitializePlayerState()
    {
        model.walkState = WalkState.IDLE;
        model.moveState = MovementStat.STANDARD; // this state is only one we use currently - but rest can easely be built upon. 
        model.turnDirState = TurnDirectionState.RIGHT;
        model.wallClingState = WallClingState.DEFAULT;
        model.actionState = ActionState.DEFAULT;
        model.inAirState = InAirState.ON_GROUND;
        model.lifeState = LifeState.ALIVE;
        model.graphHookState = GraphlingHookState.DEFAULT;
    }

    private void GetPlayerSubcontrollers()
    {
        life = GetComponent<LifeController>();
        move = GetComponent<HorizontalDirectionController>();
        jump = GetComponent<JumpController>();
        dash = GetComponent<DashController>();
        wallCling = GetComponent<WallClingController>();
    }

    private void InitializeStandardInputValues()
    {
        horizontalInput = 0;
        verticalInput = 0;
        dashInput = false;
        jumpInput = false; ;
        graphHookInput = false;
    }

    private void AssignInterfaces()
    {
        move.MoveRules = (IMove)model;
        jump.JumpRules = (IJump)model;
        wallCling.IWallClingRules = (IWallCling)model;
        life.LifeRules = (ILife)model;
        dash.DashRules = (IDash)model;
        //graphHook.GraphHookRules = (IGraphHook)model;
        //action.ActionRules = (IAction)model;
    }
    private void RecordInputs()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        dashInput = Input.GetKey(model.DashKey);
        jumpInput = Input.GetKey(model.JumpKey);
        graphHookInput = Input.GetKey(model.GraphHookKey);
    }


    // Update is called once per frame
    void Update()
    {
        RecordInputs();
        model.MaxVelocityY = 12f;
        HandleChangeState();
        switch (model.moveState)
        {
            case MovementStat.STANDARD:
                StandardState();
                move.MoveCharacter(horizontalInput);
                break;
            case MovementStat.JUMPING:
                StandardState();
                move.MoveCharacter(horizontalInput);
                jump.Jump(1.0f, body);
                JumpingState();
                break;
            case MovementStat.AIR_JUMPING:
                break;
            case MovementStat.DASHING:
                model.moveState = dash.Dash(body.velocity.y);
                break;
            case MovementStat.CROUCHING:
                break;
            case MovementStat.WALL_CLINGING:
                WallClingingState();
                break;
            case MovementStat.WALL_JUMPING:
                wallCling.WallJumpingState(move.LastInput);
                break;
            case MovementStat.GRAPPLING:
                break;
            case MovementStat.DAMAGED:
                break;

        }
    }

    private void FixedUpdate()
    {
        body.AddForce(new Vector2(0, -body.velocity.y * body.mass * 2));  // gravity.

        if (Math.Sign(body.gravityScale) == 1 && body.velocity.y <= -model.MaxVelocityY || Math.Sign(body.gravityScale) == -1 && body.velocity.y >= model.MaxVelocityY)
        {
            model.MaxVelocityFix = 0.8f; // fix/swap for max velocity when air jumping. 0.8F is force when air jumping
        }

        else
        {
            model.MaxVelocityFix = 1f;// fix/swap for max velocity when  jumping. 0.8F is force when jumping
        }

        if (!model.IsGrounded && Math.Sign(model.NewVelocity.x) != Math.Sign(body.velocity.x))
        {
            model.HorizontalVelocity *= 0.5f; // slowdown horizontal speed if character is not on the ground
        }

        if (model.NewVelocity.x == 0 && model.moveState != MovementStat.DASHING && model.moveState != MovementStat.JUMPING) //Check speed issues, also latency
        {
            print("hello!");
            body.AddForce(new Vector2(-body.velocity.x * body.mass, 0), ForceMode2D.Impulse);
        }
        else /*if (Math.Abs(rigidBody.velocity.x) <= movementSpeed || Math.Sign(newVelocity.x) != Math.Sign(rigidBody.velocity.x))*/
        {

            body.AddForce(new Vector2(model.NewVelocity.x - body.velocity.x, -body.velocity.y * (1 - model.MaxVelocityFix)), ForceMode2D.Impulse);
        }

        if (Math.Abs(model.NewVelocity.y) > 0) //Might have to multiply by flipGravityScale instead
        {
            body.AddForce(new Vector2(0, model.NewVelocity.y), ForceMode2D.Impulse);
            model.VerticalVelocity = 0;
        }
        //rigidBody.velocity = new Vector2(newVelocity.x, newVelocity.y * maxVelocityFix); // Ta inn maxVelocityFix, mye renere
        model.IsVelocityDirty = false;
        if (body.gravityScale != model.NewGravityScale)
            body.gravityScale = model.NewGravityScale;
        //Legg til air drag
        //Gjør at når du prøver å endre retning i luften, så blir kraften mot deg ganget med en konstant,
        //slik at man ikke kan endre retning umiddelbart
    }

    private void HandleChangeState()
    {
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
        model.MaxVelocityFix = 1f;

        if (model.moveState == MovementStat.WALL_JUMPING)
        {
            return;
        }
        else if (model.moveState == MovementStat.DASHING)
        {
            if (model.WallTrigger != 0)
            {
                if (!model.IsGrounded)
                    model.moveState = MovementStat.WALL_CLINGING;
                else
                    model.moveState = MovementStat.STANDARD;
            }
        }
        else if (Input.GetKeyDown(model.JumpKey) && (model.WallTrigger == 0 || model.IsGrounded)) // If just pressed jumpKey and there's no collision detected by the wallTriggers:
        {
            if (model.IsGrounded)
                model.moveState = MovementStat.JUMPING;
            else if (!model.HasAirJumped && Time.time >= model.JumpTime + model.MinimumTimeBeforeAirJump)
                model.moveState = MovementStat.AIR_JUMPING;
        }
        else if (Input.GetKeyDown(model.DashKey) && !model.HasDashed)
        {
            model.LastActionTime = Time.time;
            model.moveState = MovementStat.DASHING;
        }
        // if F: grapple
        else if (model.WallTrigger != 0 && !model.IsGrounded)
        {
            model.moveState = MovementStat.WALL_CLINGING;
        }
        else
            model.moveState = MovementStat.STANDARD;
    }
    private void StandardState()
    {

        model.IsVelocityDirty = true;
        if (model.IsGrounded)
        {
            model.HasDashed = false;
        }
    }
    private void JumpingState()
    {
        switch (model.moveState)
        {
            case MovementStat.JUMPING:
                jump.Jump(1.0f,body);
                if (jumpInput && IsVelocityUpwards())
                    model.NewGravityScale = model.JumpingGravityScaleMultiplier * model.BaseGravityScale * model.FlipGravityScale;
                else
                {
                    model.NewGravityScale = model.BaseGravityScale * model.FlipGravityScale;
                    model.moveState = MovementStat.STANDARD;
                }
                break;

            case MovementStat.AIR_JUMPING:
                jump.Jump(0.8f, body);
                model.HasAirJumped = true;
                model.moveState = MovementStat.STANDARD;
                break;
        }
    }

    private void WallClingingState()
    {
        move.MoveCharacter(horizontalInput);
        model.WallJumpDirection = model.WallTrigger;

        if (body.velocity.y * Math.Sign(model.NewGravityScale) <= 0)
            model.NewGravityScale = model.BaseGravityScale * model.WallslideGravityScaleMultiplier * model.FlipGravityScale;

        if (jumpInput)
        {
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
            model.NewGravityScale = model.BaseGravityScale * model.JumpingGravityScaleMultiplier * model.FlipGravityScale;
            model.WallJumpTime = Time.time;
            model.JumpTime = Time.time;
            model.moveState = wallCling.WallJumpingState(move.LastInput);

            model.HasDashed = false;
            model.HasAirJumped = false;

            model.moveState = MovementStat.WALL_JUMPING;

            return;
        }
        else if (Math.Abs(body.velocity.y) <= 6 && model.WallTrigger == -Math.Sign(model.NewVelocity.x * model.FlipGravityScale)) //wallcling if input towards wall
        {
            body.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
            model.MaxVelocityY = 2f;
        }
    }
    private bool IsVelocityUpwards()
    {
        return body.velocity.y * Math.Sign(body.gravityScale) > 0;
    }


}

