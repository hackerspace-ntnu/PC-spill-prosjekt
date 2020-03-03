using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchCrouchingState : CrouchingState
{
    public new static readonly GlitchCrouchingState INSTANCE = new GlitchCrouchingState();

    public override string Name => "GLITCH_CROUCHING";

    protected override string AnimatorParameterName => "GlitchCrouch";

    private GlitchCrouchingState() {}

    public override void Init(PlayerController controller)
    {
        base.Init(controller);

        controller.TargetVelocity = rigidbody.velocity;
    }

    public override void Update()
    {
        if (!Input.GetButton("Crouch") && controller.CanUncrouch)
        {
            controller.ChangeState(IdleState.INSTANCE);
        }

        if (rigidbody.velocity.y * controller.FlipGravityScale < 0.0f)
        {
            controller.ChangeState(AirborneState.INSTANCE);
        }
    }

    public override void FixedUpdate()
    {
        float newVelocityX = controller.TargetVelocity.x - rigidbody.velocity.x;

        rigidbody.AddForce(new Vector2(newVelocityX, 0), ForceMode2D.Impulse);
    }

    public override void ToggleGlitch()
    {
        controller.ChangeState(CrouchingState.INSTANCE);
    }
}
