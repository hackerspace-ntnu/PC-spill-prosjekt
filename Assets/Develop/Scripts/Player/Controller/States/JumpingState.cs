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
        HandleHorizontalInput();

        if(Input.GetButtonDown("Jump")) {
            if(!hasAirJumped && Time.time >= jumpTime + MINIMUM_TIME_BEFORE_AIR_JUMP) {
                AirJump();
            }
        }
        else if (wallTrigger != 0)
        {
            controller.ChangeState(WallClingingState.INSTANCE);
        }
        else if (rigidBody.velocity.y * flipGravityScale < 0.0f)
        {
            controller.ChangeState(AirborneState.INSTANCE);
        }
    }

    public override void FixedUpdate() {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        rigidBody.gravityScale = baseGravityScale;
    }

    internal void GroundJump()
    {
        Grounded = false;

        targetVelocity.y = groundJumpSpeed * flipGravityScale;
        jumpTime = Time.time;
    }

    internal void AirJump()
    {
        hasAirJumped = true;
        targetVelocity.y = (airJumpSpeed - rigidBody.velocity.y) * flipGravityScale;
        jumpTime = Time.time;
    }

    internal void WallJump()
    {
        if (Math.Abs(horizontalInput) >= 0.3)
            targetVelocity = new Vector2(wallTrigger * dashSpeed * 1.5f, airJumpSpeed - rigidBody.velocity.y) * flipGravityScale * 10f;
        else
            targetVelocity = new Vector2(wallTrigger * movementSpeed * 1.5f, groundJumpSpeed - rigidBody.velocity.y) * flipGravityScale * 10f;
    }
}
