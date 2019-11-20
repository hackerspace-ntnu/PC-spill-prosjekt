using System;
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

        if (controller.Grounded) {
            controller.ChangeState(IdleState.INSTANCE);
        }
        else if (wallTrigger != 0)
        base.Update();

        }
        else if (Input.GetButtonDown("Jump")) {
    }

    public override void FixedUpdate() {
        // decreases horizontal acceleration in air while input in opposite direction of velocity
        if (Math.Sign(controller.TargetVelocity.x) != Math.Sign(rigidBody.velocity.x)) {
            controller.TargetVelocity = new Vector2(controller.TargetVelocity.x * 0.5f, controller.TargetVelocity.y);
        }

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
