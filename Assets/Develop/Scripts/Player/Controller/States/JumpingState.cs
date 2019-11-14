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
        base.Enter();
        
        if(controller.GetPreviousState() == AirborneState.INSTANCE) {
            AirJump();
        } else {
            GroundJump();
        }
    }

    public override void Update()
    {
        base.Update();

        if (rigidBody.velocity.y * flipGravityScale < 0.0f)
        {
            controller.ChangeState(AirborneState.INSTANCE);
        }

        if (controller.Grounded) {
            controller.ChangeState(IdleState.INSTANCE);
        }

    }

    public override void FixedUpdate() {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
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
}
