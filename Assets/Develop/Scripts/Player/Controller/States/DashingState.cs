using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingState : PlayerState
{
    public static readonly DashingState INSTANCE = new DashingState();

    public override string Name => "DASHING";

    protected float dashDuration = 0.2f;

    protected DashingState() {}

    public override void Enter()
    {
        controller.HasDashed = true;
        controller.DashTime = Time.time;

        controller.TargetVelocity = new Vector2((int)controller.Dir * dashSpeed * controller.FlipGravityScale, 0);
        rigidbody.gravityScale = 0;
        controller.Animator.SetBool("Dash", true);
    }

    public override void Update()
    {
        CheckGrappling();

        if (controller.WallTrigger != 0)
        {
            if (controller.GlitchActive)
            {
                controller.ChangeState(GlitchWallClingingState.INSTANCE);
            }
            else
            {
                controller.ChangeState(WallClingingState.INSTANCE);
            }
        }
        else if (Time.time - controller.DashTime >= dashDuration)
        {
            if (controller.Grounded)
                controller.ChangeState(IdleState.INSTANCE);
            else
                controller.ChangeState(AirborneState.INSTANCE);
        }
    }

    public override void FixedUpdate()
    {
        float newVelocityX = controller.TargetVelocity.x - rigidbody.velocity.x;
        float newVelocityY = controller.TargetVelocity.y - rigidbody.velocity.y;

        rigidbody.AddForce(new Vector2(newVelocityX, newVelocityY), ForceMode2D.Impulse);
    }

    public override void Exit()
    {
        rigidbody.gravityScale = baseGravityScale * controller.FlipGravityScale;
        controller.Animator.SetBool("Dash", false);
    }

    public override void Crouch()
    {
        if (controller.Grounded)
        {
            if (controller.GlitchActive)
            {
                controller.ChangeState(GlitchCrouchingState.INSTANCE);
            }
            else
            {
                controller.ChangeState(CrouchingState.INSTANCE);
            }
        }
    }
}
