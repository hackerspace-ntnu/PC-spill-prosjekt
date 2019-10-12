using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    // Models
    PlayerModel model; // All variables related to player. Doesn't do any logic, just hold variables. Used ALOT.



    // Views
    Rigidbody2D body; // The rigid body of our player.

    private StateMachine stateMachine;

    public StateMachine StateMachine { get => stateMachine; private set => stateMachine = value; }


    // Start is called before the first frame update
    void Start()
    {
        body = GameObject.Find("View").GetComponent<Rigidbody2D>();
        StateMachine = GetComponent<StateMachine>();
        StateMachine.Owner = this;
        model = GameObject.Find("Models").GetComponent<PlayerModel>();
        model.NewVelocity = body.velocity;
        model.NewGravityScale = model.BaseGravityScale * model.FlipGravityScale;
    }




    // Update is called once per frame
    void Update()
    {
        model.MaxVelocityY = 12f;
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
        model.MaxVelocityFix = 1f;
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
            model.MaxVelocityFix = 1f;// fix/swap for max velocity when jumping. 0.8F is force when jumping
        }

        if (!model.IsGrounded && Math.Sign(model.NewVelocity.x) != Math.Sign(body.velocity.x))
        {
            model.HorizontalVelocity *= 0.5f; // slowdown horizontal speed if character is not on the ground
        }

        body.AddForce(new Vector2(model.HorizontalVelocity - body.velocity.x, model.VerticalVelocity - body.velocity.y * (1 - model.MaxVelocityFix)), ForceMode2D.Impulse);
        model.VerticalVelocity = 0;

        //rigidBody.velocity = new Vector2(newVelocity.x, newVelocity.y * maxVelocityFix); // Ta inn maxVelocityFix, mye renere
        model.IsVelocityDirty = false;
        if (body.gravityScale != model.NewGravityScale)
            body.gravityScale = model.NewGravityScale;

    }

    /*
        private void HandleChangeState()
        {
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
            model.MaxVelocityFix = 1f;

            if (model.MoveState == MovementStat.WALL_JUMPING)
            {
                return;
            }
            else if (model.MoveState == MovementStat.DASHING)
            {
                if (model.WallTrigger != 0)
                {
                    if (!model.IsGrounded)
                        model.MoveState = MovementStat.WALL_CLINGING;
                    else
                        model.MoveState = MovementStat.STANDARD;
                }
            }
            else if (Input.GetKeyDown(model.JumpKey) && (model.WallTrigger == 0 || model.IsGrounded)) // If just pressed jumpKey and there's no collision detected by the wallTriggers:
            {
                if (model.IsGrounded)
                    model.MoveState = MovementStat.JUMPING;
                else if (!model.HasAirJumped && Time.time >= model.JumpTime + model.MinimumTimeBeforeAirJump)
                    model.MoveState = MovementStat.AIR_JUMPING;
            }
            else if (Input.GetKeyDown(model.DashKey) && !model.HasDashed)
            {
                model.LastDashTime = Time.time;
                model.MoveState = MovementStat.DASHING;
            }
            // if F: grapple
            else if (model.WallTrigger != 0 && !model.IsGrounded)
            {
                model.MoveState = MovementStat.WALL_CLINGING;
            }
            else
                model.MoveState = MovementStat.STANDARD;
        }
        private void StandardState()
        {

            model.IsVelocityDirty = true;
            if (model.IsGrounded)
            {
                model.HasDashed = false;
                model.HasAirJumped = false;
            }
        }
        /*private void JumpingState()
        {
            switch (model.MoveState)
            {
                case MovementStat.JUMPING:
                    jumpCtrl.GroundJump(body.gravityScale);
                    if (jumpInput && IsVelocityUpwards())
                        model.NewGravityScale = model.JumpingGravityScaleMultiplier * model.BaseGravityScale * model.FlipGravityScale;
                    else
                    {
                        model.NewGravityScale = model.BaseGravityScale * model.FlipGravityScale;
                        model.MoveState = MovementStat.STANDARD;
                    }
                    break;

                case MovementStat.AIR_JUMPING:
                    jumpCtrl.AirJump(body.velocity.y, body.gravityScale);
                    model.HasAirJumped = true;
                    model.MoveState = MovementStat.STANDARD;
                    break;
            }
        }

        private void WallClingingState()
        {
            Debug.Log("jumpinput: "+jumpInput);
            model.WallJumpDirection = model.WallTrigger;

            if (body.velocity.y * Math.Sign(model.NewGravityScale) <= 0)
                model.NewGravityScale = model.BaseGravityScale * model.WallslideGravityScaleMultiplier * model.FlipGravityScale;

            if (jumpInput)
            {
                body.constraints = RigidbodyConstraints2D.FreezeRotation;
                model.NewGravityScale = model.BaseGravityScale * model.JumpingGravityScaleMultiplier * model.FlipGravityScale;
                model.WallJumpTime = Time.time;
                model.JumpTime = Time.time;
                wallClingCtrl.WallJumpingState(horizontalInput, body.velocity.y);

                model.HasDashed = false;
                model.HasAirJumped = false;

                model.MoveState = MovementStat.WALL_JUMPING;
            }
            else if (Math.Abs(body.velocity.y) <= 6 && model.WallTrigger == -Math.Sign(horizontalInput * model.FlipGravityScale)) //wallcling if input towards wall
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
        }*/


}

