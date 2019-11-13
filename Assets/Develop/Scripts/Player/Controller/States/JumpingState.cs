using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : PlayerState
{
    public static readonly JumpingState INSTANCE = new JumpingState();

    public override string Name => "JUMPING";

    private float newGravityScale;

    public override void Enter()
    {
        if(controller.GetPreviousState() == AirborneState.INSTANCE) {
            hasAirJumped = controller.GetPreviousState().getHasAirJumped();
            jumpTime = controller.GetPreviousState().getJumpTime();
        } else {
            hasAirJumped = false;
        }
        
        if(controller.GetPreviousState() == AirborneState.INSTANCE) {
            AirJump();
        } else {
            GroundJump();
        }
    }

    public override void Update()
    {
        HandleHorizontalInput();

        if(Input.GetButtonDown("Jump")) {
            if(!hasAirJumped && Time.time >= jumpTime + MINIMUM_TIME_BEFORE_AIR_JUMP) {
                hasAirJumped = true;
                AirJump();
            }
        }

        if (rigidBody.velocity.y * flipGravityScale < 0.0f)
        {
            controller.ChangeState(AirborneState.INSTANCE);
        }
    }

    public override void FixedUpdate() {
        base.FixedUpdate();
    }

    public override void Exit()
    {

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
}
