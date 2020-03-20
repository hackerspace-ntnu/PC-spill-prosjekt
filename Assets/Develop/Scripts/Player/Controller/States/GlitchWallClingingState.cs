using System;
using System.Collections;
using System.Collections.Generic;
using GlobalEnums;
using UnityEngine;

public class GlitchWallClingingState : WallClingingState
{
    public new static readonly GlitchWallClingingState INSTANCE = new GlitchWallClingingState();

    public override string Name => "GLITCH_WALL_CLINGING";

    private const float SPRITE_POS_OFFSET = 0.1f;

    private Vector3 defaultSpinePos;

    private bool isStuck = false;

    private GlitchWallClingingState() {}

    public override void Init(PlayerController controller)
    {
        base.Init(controller);

        defaultSpinePos = controller.SkeletonMecanim.gameObject.transform.localPosition;
    }

    public override void Enter()
    {
        controller.HasDashed = false;
        controller.HasAirJumped = false;

        if (controller.WallTrigger == WallTrigger.LEFT)
            controller.SkeletonMecanim.skeleton.ScaleX = 1;
        else
            controller.SkeletonMecanim.skeleton.ScaleX = -1;

        if (Time.time - controller.JumpButtonPressTime < 0.125f)
        {
            controller.ChangeState(JumpingState.INSTANCE);
            return;
        }

        isStuck = false;
    }

    public override void Update()
    {
        HandleHorizontalInput();
        controller.Animator.SetFloat("Hinput", Mathf.Abs(horizontalInput));

        CheckGrappling();

        if (controller.WallTrigger == WallTrigger.LEFT)
            controller.SkeletonMecanim.skeleton.ScaleX = 1;
        else
            controller.SkeletonMecanim.skeleton.ScaleX = -1;

        if (Math.Sign(horizontalInput) == -(int)controller.WallTrigger)
        {
            rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            TogglePlayerStuck();
        }
        else
            rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (controller.Grounded)
            controller.ChangeState(IdleState.INSTANCE);
        else if (controller.WallTrigger == WallTrigger.NONE)
            if (rigidbody.velocity.y * controller.FlipGravityScale > 0)
            {
                controller.ChangeState(JumpingState.INSTANCE);
            }
            else
            {
                controller.ChangeState(AirborneState.INSTANCE);
            }
    }

    public override void Exit()
    {
        controller.Animator.SetBool("WallCling", false);
        controller.Animator.SetBool("Idle", false);
        controller.SkeletonMecanim.gameObject.transform.localPosition = defaultSpinePos;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public override void Jump()
    {
        controller.ChangeState(JumpingState.INSTANCE);
    }

    public override void Dash()
    {
        if (!controller.HasDashed)
        {
            UnstuckPlayer();
            controller.ChangeState(DashingState.INSTANCE);
        }
    }

    public override void ToggleGlitch()
    {
        controller.ChangeState(WallClingingState.INSTANCE);
    }

    private void TogglePlayerStuck()
    {
        isStuck = !isStuck;

        controller.Animator.SetBool("WallCling", !isStuck);
        controller.Animator.SetBool("Idle", isStuck);

        float offset = SPRITE_POS_OFFSET * (int)controller.WallTrigger;

        Vector3 animPos = controller.SkeletonMecanim.gameObject.transform.localPosition;
        animPos.x += isStuck ? -offset : offset;
        controller.SkeletonMecanim.gameObject.transform.localPosition = animPos;
    }

    private void UnstuckPlayer()
    {
        isStuck = false;
        controller.Animator.SetBool("WallCling", true);
        controller.SkeletonMecanim.gameObject.transform.localPosition = defaultSpinePos;
    }
}
