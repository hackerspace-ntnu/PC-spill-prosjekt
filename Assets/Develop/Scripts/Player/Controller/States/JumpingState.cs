using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : PlayerState
{
    public static readonly JumpingState INSTANCE = new JumpingState();

    public override string Name => "JUMPING";

    public override void Enter()
    {
        controller.Animator.SetBool("Jump", true);
        rigidBody.gravityScale = JUMPING_GRAVITY_SCALE;
        PlayerState prevInstance = controller.GetPreviousState();
        //Since all other logic is tested in these states, this if/else is all we need
        if (prevInstance == AirborneState.INSTANCE && !controller.HasAirJumped) {
            AirJump();
            Debug.Log("AirJumping");
        } else if (prevInstance == WallClingingState.INSTANCE) {
            WallJump();
            Debug.Log("WallJumping");
        } else {
            GroundJump();
            Debug.Log("GroundJumping");
        }
    }

    public override void Update()
    {
        if (controller.WallTrigger != 0)
        {
            controller.ChangeState(WallClingingState.INSTANCE);
        }
        else if (rigidBody.velocity.y * flipGravityScale < 0.0f)
        {
            controller.ChangeState(AirborneState.INSTANCE);
        }

        if (controller.Grounded) {
            controller.ChangeState(IdleState.INSTANCE);
        }

        base.Update();
    }

    public override void FixedUpdate() {
        if (Math.Sign(rigidBody.gravityScale) == 1 && rigidBody.velocity.y <= -maxVelocityY ||
            Math.Sign(rigidBody.gravityScale) == -1 && rigidBody.velocity.y >= maxVelocityY)
        {
            maxVelocityFix = 0.2f;
        }
        else
        {
            maxVelocityFix = 0f;
        }

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

        float newVelocityY = controller.TargetVelocity.y - rigidBody.velocity.y * maxVelocityFix;

        rigidBody.AddForce(new Vector2(newVelocityX, newVelocityY), ForceMode2D.Impulse);
        controller.TargetVelocity = Vector2.zero;
    }

    public override void Exit()
    {
        controller.Animator.SetBool("Jump", false);
        rigidBody.gravityScale = baseGravityScale;
    }

    public override void Jump()
    {
        AirJump();
    }

    internal void GroundJump()
    {
        controller.Grounded = false;

        controller.TargetVelocity = new Vector2(controller.TargetVelocity.x, groundJumpSpeed * flipGravityScale);
        controller.JumpTime = Time.time;
    }

    internal void AirJump()
    {
        controller.HasAirJumped = true;
        controller.TargetVelocity = new Vector2(controller.TargetVelocity.x, (airJumpSpeed - rigidBody.velocity.y) * flipGravityScale);
        controller.JumpTime = Time.time;
    }

    internal void WallJump()
    {
        // The input to differentiate between the kinds of wallJump is too tight
        if (Math.Abs(horizontalInput) >= 0.8f)
            controller.TargetVelocity = new Vector2(controller.WallTrigger * dashSpeed * 1.5f * 2, airJumpSpeed - rigidBody.velocity.y) * flipGravityScale * 1.2f;
        else
            controller.TargetVelocity = new Vector2(controller.WallTrigger * movementSpeed * 1.5f, groundJumpSpeed - rigidBody.velocity.y) * flipGravityScale * 1.1f;
        controller.HasDashed = false;
    }

    public override void Dash()
    {
        if (!controller.HasDashed)
        {
            controller.ChangeState(DashingState.INSTANCE);
        }
    }
}
