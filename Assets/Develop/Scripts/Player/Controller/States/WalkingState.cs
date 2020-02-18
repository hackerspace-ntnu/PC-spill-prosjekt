using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : PlayerState
{
    public static readonly WalkingState INSTANCE = new WalkingState();

    protected float idleSpeedThreshold = 0.1f;

    public override string Name => "WALKING";

    protected WalkingState() {}

    public override void Enter() {
        base.Enter();
        controller.HasAirJumped = false;
        controller.HasDashed = false;
        controller.Animator.SetBool("Run", true);
        
    }

    public override void Update()
    {
        base.Update();

        if (Math.Abs(rigidBody.velocity.x) < idleSpeedThreshold && controller.GetCurrentState() != IdleState.INSTANCE) {
            controller.ChangeState(IdleState.INSTANCE);
        } else if (!controller.Grounded || rigidBody.velocity.y * controller.FlipGravityScale < 0.0f) {
            controller.ChangeState(AirborneState.INSTANCE);
        } 

        //Debug.Log(rigidBody.velocity.x);
    }

    public override void FixedUpdate() {
        base.FixedUpdate();
    }

    public override void Exit() {
        base.Exit();
        controller.Animator.SetBool("Run", false);
    }

    public override void Jump() {
        controller.ChangeState(JumpingState.INSTANCE);
    }

    public override void Crouch() {
        if (controller.GlitchActive)
        {
            controller.ChangeState(GlitchCrouchingState.INSTANCE);
        }
        else
        {
            controller.ChangeState(CrouchingState.INSTANCE);
        }
    }

    public override void Dash()
    {
        if (Time.time - controller.DashTime > 0.4f) {
            if (controller.GlitchActive) {
                controller.ChangeState(GlitchDashingState.INSTANCE);
            } else {
                controller.ChangeState(DashingState.INSTANCE);
            }
        }
    }
}
