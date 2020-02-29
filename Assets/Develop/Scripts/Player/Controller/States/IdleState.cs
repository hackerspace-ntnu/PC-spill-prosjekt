using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : WalkingState
{
    public new static readonly IdleState INSTANCE = new IdleState();

    public override string Name => "IDLE";

    private IdleState() {}

    public override void Enter() {
        controller.HasAirJumped = false;
        controller.HasDashed = false;
        controller.Animator.SetBool("Idle", true);
    }

    public override void Update()
    {
        base.Update();

        if (Math.Abs(rigidbody.velocity.x) >= idleSpeedThreshold)
            controller.ChangeState(WalkingState.INSTANCE);
    }

    public override void FixedUpdate() {
        base.FixedUpdate();
    }

    public override void Exit() {
        base.Exit();
        controller.Animator.SetBool("Idle", false);
    }
}
