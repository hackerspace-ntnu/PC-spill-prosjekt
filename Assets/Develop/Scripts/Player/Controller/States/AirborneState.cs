using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneState : PlayerState
{
    public static readonly AirborneState INSTANCE = new AirborneState();

    public override string Name => "AIRBORNE";

    public override void Enter()
    {
        base.Enter();

       controller.Grounded = false;
    }

    public override void Update()
    {
        base.Update();

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
        controller.ChangeState(JumpingState.INSTANCE);
    }
}
