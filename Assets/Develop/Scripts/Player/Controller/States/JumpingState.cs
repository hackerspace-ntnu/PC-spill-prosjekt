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
        rigidBody.gravityScale = JUMPING_GRAVITY_SCALE;
        PlayerState prevInstance = controller.GetPreviousState();
        hasAirJumped = false;
        //Since all other logic is tested in these states, this if/else is all we need
        if (prevInstance == AirborneState.INSTANCE) {
            AirJump();
        } else if (prevInstance == WallClingingState.INSTANCE) {
            WallJump();
            jumpTime = WallClingingState.INSTANCE.getJumpTime();
        } else {
            GroundJump();
            jumpTime = AirborneState.INSTANCE.getJumpTime();
        }
    }

    public override void Update()
    {
        else if (wallTrigger != 0)
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
            targetVelocity = new Vector2(wallTrigger * dashSpeed * 1.5f, airJumpSpeed - rigidBody.velocity.y) * flipGravityScale * 10f;
        else
            targetVelocity = new Vector2(wallTrigger * movementSpeed * 1.5f, groundJumpSpeed - rigidBody.velocity.y) * flipGravityScale * 10f;
    }
}
