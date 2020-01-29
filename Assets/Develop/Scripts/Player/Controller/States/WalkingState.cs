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
        controller.Animator.SetBool("Run", true);
        
    }

    public override void Update()
    {
        base.Update();

        if (Math.Abs(rigidBody.velocity.x) < idleSpeedThreshold && controller.GetCurrentState() != IdleState.INSTANCE) {
            controller.ChangeState(IdleState.INSTANCE);
        } else if (rigidBody.velocity.y * flipGravityScale < 0.0f) {
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
        controller.ChangeState(CrouchingState.INSTANCE);
    }
}
