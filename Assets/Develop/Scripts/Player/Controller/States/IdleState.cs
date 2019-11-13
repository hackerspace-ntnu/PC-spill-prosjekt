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
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (Math.Abs(rigidBody.velocity.x) >= idleSpeedThreshold)
            controller.ChangeState(WalkingState.INSTANCE);
    }

    public override void FixedUpdate() {
        base.FixedUpdate();
    }

    public override void Exit() {
        base.Exit();
    }
}
