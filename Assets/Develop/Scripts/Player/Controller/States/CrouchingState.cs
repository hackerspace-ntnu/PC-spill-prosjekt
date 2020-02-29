using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchingState : PlayerState
{
    public static readonly CrouchingState INSTANCE = new CrouchingState();

    public override string Name => "CROUCHING";

    private const float CROUCH_HEIGHT_RATIO = 0.8f;
    private const float CROUCH_SPEED = 0.5f;

    protected virtual string AnimatorParameterName => "Crouch";

    // Game objects for crouching and uncrouching
    protected BoxCollider2D collider;

    protected Vector2 baseColliderSize;
    protected Vector2 baseColliderOffset;
    protected Vector2 crouchColliderSize;
    protected Vector2 crouchColliderOffset;

    protected int animatorParameterId;

    protected CrouchingState() {}

    public override void Init(PlayerController controller)
    {
        base.Init(controller);

        collider = controller.GetComponent<BoxCollider2D>();

        baseColliderSize = collider.size;
        baseColliderOffset = collider.offset;

        crouchColliderSize = baseColliderSize;
        crouchColliderSize.y *= CROUCH_HEIGHT_RATIO;

        crouchColliderOffset = baseColliderOffset;
        float heightDifference = baseColliderSize.y - crouchColliderSize.y;
        crouchColliderOffset.y = -heightDifference / 2f;

        animatorParameterId = Animator.StringToHash(AnimatorParameterName);
    }


    public override void Enter()
    {
        collider.size = crouchColliderSize;
        collider.offset = crouchColliderOffset;

        controller.Animator.SetBool(animatorParameterId, true);
    }

    public override void Update()
    {
        base.Update();

        if (!Input.GetButton("Crouch") && controller.CanUncrouch)
            controller.ChangeState(IdleState.INSTANCE);

        if (rigidbody.velocity.y * controller.FlipGravityScale < 0.0f)
            controller.ChangeState(AirborneState.INSTANCE);
    }

    public override void FixedUpdate()
    {
        // Set movement speed to crouch speed.
        controller.TargetVelocity = new Vector2(controller.TargetVelocity.x * CROUCH_SPEED, controller.TargetVelocity.y);

        base.FixedUpdate();
    }

    public override void Exit() {
        collider.size = baseColliderSize;
        collider.offset = baseColliderOffset;

        controller.Animator.SetBool(animatorParameterId, false);
    }

    public override void Jump()
    {
        if (controller.CanUncrouch)
            controller.ChangeState(JumpingState.INSTANCE);
        else
            controller.JumpButtonPressTime = Time.time;
    }

    public override void ToggleGlitch()
    {
        controller.ChangeState(GlitchCrouchingState.INSTANCE);
    }
}
