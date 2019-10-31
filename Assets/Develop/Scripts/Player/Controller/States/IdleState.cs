using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : WalkingState
{
    public new static readonly IdleState INSTANCE = new IdleState();

    public override string Name => "IDLE";

    private IdleState() {}

    public override void Update()
    {
        base.Update();

        if (rigidBody.velocity.magnitude >= idleSpeedThreshold)
            controller.ChangeState(WalkingState.INSTANCE);
    }
}
