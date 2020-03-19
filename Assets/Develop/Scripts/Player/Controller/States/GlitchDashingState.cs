using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchDashingState : DashingState
{
    public new static readonly GlitchDashingState INSTANCE = new GlitchDashingState();

    public override string Name => "GLITCH_DASHING";

    private GlitchDashingState() {}

    public override void Enter()
    {
        controller.HasDashed = true;
        controller.DashTime = Time.time;

        controller.TargetVelocity = new Vector2((int)controller.FacingDirection * dashSpeed * controller.FlipGravityScale, 0);
        rigidbody.gravityScale = 0;
        //disable damageBoxCollider
    }

    public override void Exit()
    {
        rigidbody.gravityScale = baseGravityScale * controller.FlipGravityScale;
        //enable damageBoxCollider
    }
}
