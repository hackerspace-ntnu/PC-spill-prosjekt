using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchCrouchingState : CrouchingState
{
    public new static readonly GlitchCrouchingState INSTANCE = new GlitchCrouchingState();

    public override string Name => "GLITCH_CROUCHING";

    private const float SPRITE_POS_OFFSET = 0.22f;
    private const float CROUCH_HEIGHT_RATIO = 0.6f;
    protected override string AnimatorParameterName => "GlitchCrouch";

    protected Vector2 glitchCrouchColliderSize;
    protected Vector2 glitchCrouchColliderOffset;

    private bool tryToUnglitch = false;

    private GlitchCrouchingState() {}

    public override void Enter()
    {
        base.Enter();

        collider.size = glitchCrouchColliderSize;
        collider.offset = glitchCrouchColliderOffset;

        tryToUnglitch = false;

        Vector3 animPos = controller.SkeletonMecanim.gameObject.transform.position;
        animPos.y -= SPRITE_POS_OFFSET;
        controller.SkeletonMecanim.gameObject.transform.position = animPos;
    }

    public override void Init(PlayerController controller)
    {
        base.Init(controller);

        glitchCrouchColliderSize = baseColliderSize;
        glitchCrouchColliderSize.y *= CROUCH_HEIGHT_RATIO;

        glitchCrouchColliderSize = baseColliderSize;
        glitchCrouchColliderSize.y *= CROUCH_HEIGHT_RATIO;

        glitchCrouchColliderOffset = baseColliderOffset;
        float heightDifference = baseColliderSize.y - glitchCrouchColliderSize.y;
        glitchCrouchColliderOffset.y = -heightDifference / 2f;
    }

    public override void Update()
    {
        base.Update();

        if (tryToUnglitch && controller.CanUnglitch)
        {
            controller.ChangeState(CrouchingState.INSTANCE);
        }

        if (Math.Abs(rigidbody.velocity.x) >= IDLE_SPEED_THRESHOLD)
        {
            controller.Animator.SetBool("Walk", true);
            controller.Animator.SetBool("Idle", false);
        }
        else
        {
            controller.Animator.SetBool("Idle", true);
            controller.Animator.SetBool("Walk", false);
        }
    }

    public override void ToggleGlitch()
    {
        tryToUnglitch = !tryToUnglitch;
    }

    public override void Exit()
    {
        base.Exit();

        controller.Animator.SetBool("Idle", false);
        controller.Animator.SetBool("Walk", false);

        Vector3 animPos = controller.SkeletonMecanim.gameObject.transform.position;
        animPos.y += SPRITE_POS_OFFSET;
        controller.SkeletonMecanim.gameObject.transform.position = animPos;
    }
}
