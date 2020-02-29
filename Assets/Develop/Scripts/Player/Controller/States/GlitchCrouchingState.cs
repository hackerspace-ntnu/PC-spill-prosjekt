using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchCrouchingState : CrouchingState
{
    public new static readonly GlitchCrouchingState INSTANCE = new GlitchCrouchingState();

    public override string Name => "GLITCH_CROUCHING";

    private const float crouchSpeed = 1f;
    private const float animPosOffset = 0.4f;

    protected override string AnimatorParameterName => "GlitchCrouch";

    private GlitchCrouchingState() {}

    public override void Enter() {
        collider.size = crouchColliderSize;
        collider.offset = crouchColliderOffset;

        crouchCeilingDetector.SetActive(true);

        Vector3 animPos = controller.SkeletonMecanim.gameObject.transform.position;
        animPos.y -= animPosOffset;
        controller.SkeletonMecanim.gameObject.transform.position = animPos;
    }

    public override void Init(PlayerController controller)
    {
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
        controller.Animator.SetBool("GlitchCrouch", true);
        controller.Animator.SetBool("GlitchCrouch", true);
    }

    public override void Exit() {
        collider.size = baseColliderSize;
        collider.offset = baseColliderOffset;

        crouchCeilingDetector.SetActive(false);

        Vector3 animPos = controller.SkeletonMecanim.gameObject.transform.position;
        animPos.y += animPosOffset;
        controller.SkeletonMecanim.gameObject.transform.position = animPos;
    }
}
