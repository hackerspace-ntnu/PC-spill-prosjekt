using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneState : PlayerState
{
    public static readonly AirborneState INSTANCE = new AirborneState();

    public override string Name => "AIRBORNE";

    private AirborneState() {}

    public override void Enter()
    {
        controller.Animator.SetBool("Airborne", true);
    }

    public override void Update()
    {
        base.Update();

        if (controller.Grounded)
            controller.ChangeState(IdleState.INSTANCE);
        else if (controller.WallTrigger != 0)
        {
            if (controller.GlitchActive)
                controller.ChangeState(GlitchWallClingingState.INSTANCE);
            else
                controller.ChangeState(WallClingingState.INSTANCE);
        }
    }

    public override void FixedUpdate()
    {
        if (controller.FlipGravityScale == 1 && rigidbody.velocity.y <= -maxVelocityY
            || controller.FlipGravityScale == -1 && rigidbody.velocity.y >= maxVelocityY)
        {
            maxVelocityFix = 0.02f;
        }
        else
            maxVelocityFix = 0f;

        float newVelocityX;
        // decreases horizontal acceleration in air while input in opposite direction of velocity
        if (Math.Sign(controller.TargetVelocity.x) != Math.Sign(rigidbody.velocity.x))
        {
            newVelocityX = controller.TargetVelocity.x * 0.2f - rigidbody.velocity.x * 0.1f;
        }
        else
        {
            newVelocityX = controller.TargetVelocity.x - rigidbody.velocity.x;
        }

        float newVelocityY = - rigidbody.velocity.y * maxVelocityFix;

        rigidbody.AddForce(new Vector2(newVelocityX, newVelocityY), ForceMode2D.Impulse);
        //controller.TargetVelocity = new Vector2(newVelocityX, 0);
    }

    public override void Exit()
    {
        controller.Animator.SetBool("Airborne", false);
        controller.DashTime = 0f;
    }

    public override void Jump()
    {
        if (!controller.HasAirJumped && controller.GlitchActive)
            controller.ChangeState(JumpingState.INSTANCE);
        else
            controller.JumpButtonPressTime = Time.time;
    }

    public override void Dash()
    {
        if (controller.GlitchActive)
            controller.ChangeState(GlitchDashingState.INSTANCE);
        else
            controller.ChangeState(DashingState.INSTANCE);
    }
}
