using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchCrouchingState : CrouchingState
{
    public new static readonly GlitchCrouchingState INSTANCE = new GlitchCrouchingState();

    public override string Name => "GLITCH_CROUCHING";

    private const float crouchSpeed = 1f;

    protected override string AnimatorParameterName => "GlitchCrouch";

    public override void Init(PlayerController controller) {
        base.Init(controller);

        controller.TargetVelocity = rigidbody.velocity;
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
