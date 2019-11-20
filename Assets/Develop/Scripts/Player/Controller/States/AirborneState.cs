using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneState : PlayerState
{
    public static readonly AirborneState INSTANCE = new AirborneState();

    public override string Name => "AIRBORNE";

    public override void Enter()
    {
        Grounded = false;

        if(controller.GetPreviousState() == JumpingState.INSTANCE) {
            hasAirJumped = controller.GetPreviousState().getHasAirJumped();
            jumpTime = controller.GetPreviousState().getJumpTime();
        } else {
            hasAirJumped = false;
        }
    }

    public override void Update()
    {
        HandleHorizontalInput();

        if (Grounded) {
            controller.ChangeState(IdleState.INSTANCE);
        }
        else if (wallTrigger != 0)
        {
            controller.ChangeState(WallClingingState.INSTANCE);
        }
        else if (Input.GetButtonDown("Jump")) {
            if (!hasAirJumped && Time.time >= jumpTime + MINIMUM_TIME_BEFORE_AIR_JUMP) {
                hasAirJumped = true;
                controller.ChangeState(JumpingState.INSTANCE);
            }
        }
    }

    public override void FixedUpdate() {
        base.FixedUpdate();
    }

    public override void Exit()
    {

    }
}
