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
        } else if (prevInstance == WallClingingState.INSTANCE) {
            WallJump();
        } else {
            GroundJump();
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
        // decreases horizontal acceleration in air while input in opposite direction of velocity
        if (Math.Sign(controller.TargetVelocity.x) != Math.Sign(rigidBody.velocity.x)) {
            controller.TargetVelocity = new Vector2(controller.TargetVelocity.x * 0.5f, controller.TargetVelocity.y);
        }

        base.FixedUpdate();
    }

    public override void Exit()
    {
        controller.Animator.SetBool("Jump", false);
        rigidBody.gravityScale = baseGravityScale;
    }
    public override void Jump() {
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
        if (Math.Abs(horizontalInput) >= 0.3)
            controller.TargetVelocity = new Vector2(controller.WallTrigger * dashSpeed * 1.5f, airJumpSpeed - rigidBody.velocity.y) * flipGravityScale;
        else
            controller.TargetVelocity = new Vector2(controller.WallTrigger * movementSpeed * 1.5f, groundJumpSpeed - rigidBody.velocity.y) * flipGravityScale;
    }
}
