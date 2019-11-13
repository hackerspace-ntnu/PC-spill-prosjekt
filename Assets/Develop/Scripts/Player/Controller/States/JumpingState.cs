using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : PlayerState
{
    public static readonly JumpingState INSTANCE = new JumpingState();

    public override string Name => "JUMPING";

    private float newGravityScale;

    private float singleJumpScale = 1.0f;
    private float doubleJumpScale = 0.8f;

    public override void Enter()
    {
        Grounded = false;

        if(controller.GetPreviousState() == AirborneState.INSTANCE) {
            hasAirJumped = controller.GetPreviousState().getHasAirJumped();
            jumpTime = controller.GetPreviousState().getJumpTime();
        } else {
            hasAirJumped = false;
        }
        
        if(hasAirJumped) {
            Jump(doubleJumpScale);
        } else {
            Jump(singleJumpScale);
        }
    }

    public override void Update()
    {
        HandleHorizontalInput();

        if(Input.GetButtonDown("Jump")) {
            if(!hasAirJumped && Time.time >= jumpTime + MINIMUM_TIME_BEFORE_AIR_JUMP) {
                hasAirJumped = true;
                Jump(doubleJumpScale);
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

    private void Jump(float scale) {
        targetVelocity.y = (jumpSpeed * scale + Math.Abs(rigidBody.velocity.y)) * flipGravityScale;
        jumpTime = Time.time;
    }
}
