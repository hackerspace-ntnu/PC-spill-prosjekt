using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : PlayerState
{
    public static readonly JumpingState INSTANCE = new JumpingState();

    private float wallJumpTime;

    public override string Name => "JUMPING";

    public override void Enter()
    {
        controller.Animator.SetBool("Jump", true);
        rigidBody.gravityScale = JUMPING_GRAVITY_SCALE * controller.FlipGravityScale;
        PlayerState prevInstance = controller.GetPreviousState();

        //Since all other logic is tested in these states, this logic is all we need
        if (prevInstance == AirborneState.INSTANCE) {
            AirJump();
        } else if (prevInstance == WallClingingState.INSTANCE || prevInstance == GlitchWallClingingState.INSTANCE) {
            WallJump();
        } else {
            GroundJump();
        }
    }

    public override void Update()
    {
        if (controller.WallTrigger != 0 && Time.time - controller.JumpTime > 0.2f)
        {
            if (controller.GlitchActive)
            {
                controller.ChangeState(GlitchWallClingingState.INSTANCE);
            }
            else
            {
                controller.ChangeState(WallClingingState.INSTANCE);
            }
        }
        else if (rigidBody.velocity.y * controller.FlipGravityScale < 0.0f && controller.TargetVelocity.y == 0)
        {
            controller.ChangeState(AirborneState.INSTANCE);
        }

        if (controller.Grounded) {
            controller.ChangeState(IdleState.INSTANCE);
        }

        if (Time.time - wallJumpTime > 0.05f)
            base.Update();
    }

    public override void FixedUpdate() {
        float newVelocityX;

        // decreases horizontal acceleration in air while input in opposite direction of velocity
        if (Math.Sign(controller.TargetVelocity.x) != Math.Sign(rigidBody.velocity.x))
        {
            newVelocityX = controller.TargetVelocity.x * 0.2f - rigidBody.velocity.x * 0.1f;
        }
        else
        {
            newVelocityX = controller.TargetVelocity.x - rigidBody.velocity.x;
        }


        float newVelocityY = 0f;
        if (controller.TargetVelocity.y != 0)
        {
            newVelocityY = controller.TargetVelocity.y - rigidBody.velocity.y;
        }

        rigidBody.AddForce(new Vector2(newVelocityX, newVelocityY), ForceMode2D.Impulse);
        controller.TargetVelocity = new Vector2(newVelocityX, 0);
    }

    public override void Exit()
    {
        controller.Animator.SetBool("Jump", false);
        rigidBody.gravityScale = baseGravityScale * controller.FlipGravityScale;
    }

    public override void Jump()
    {
        if (!controller.HasAirJumped && Time.time - controller.JumpTime > 0.2f)
            AirJump();
        else
            controller.JumpButtonPressTime = Time.time;
    }

    internal void GroundJump()
    {
        controller.Grounded = false;

        controller.TargetVelocity = new Vector2(controller.TargetVelocity.x, groundJumpSpeed * controller.FlipGravityScale);
        controller.JumpTime = Time.time;
        Debug.Log("GroundJumping");
    }

    internal void AirJump()
    {
        controller.HasAirJumped = true;
        controller.TargetVelocity = new Vector2(controller.TargetVelocity.x, airJumpSpeed * controller.FlipGravityScale);
        controller.JumpTime = Time.time;
        Debug.Log("AirJumping");
    }

    internal void WallJump()
    {
        // The input to differentiate between the kinds of wallJump is too tight
        if (Math.Abs(horizontalInput) >= 0.8f)
            controller.TargetVelocity = new Vector2(controller.WallTrigger * dashSpeed * 2f, airJumpSpeed) * controller.FlipGravityScale * 1.2f;
        else
            controller.TargetVelocity = new Vector2(controller.WallTrigger * movementSpeed * 1.5f, groundJumpSpeed) * controller.FlipGravityScale * 1.1f;
        controller.HasDashed = false;
        controller.HasAirJumped = false;
        wallJumpTime = Time.time;
        controller.JumpTime = Time.time;
        Debug.Log("WallJumping");
    }

    public override void Dash()
    {
        if (controller.GlitchActive)
        {
            controller.ChangeState(GlitchDashingState.INSTANCE);
        }
        else
        {
            controller.ChangeState(DashingState.INSTANCE);
        }
    }
}
