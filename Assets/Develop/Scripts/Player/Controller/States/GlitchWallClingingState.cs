using System;
using System.Collections;
using System.Collections.Generic;
using GlobalEnums;
using UnityEngine;

public class GlitchWallClingingState : WallClingingState
{
    public new static readonly GlitchWallClingingState INSTANCE = new GlitchWallClingingState();

    public override string Name => "GLITCH_WALL_CLINGING";

    private GlitchWallClingingState() {}

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

        controller.Animator.SetBool("GlitchWallCling", true);
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

        // It didn't like comparing an int to WallTrigger.LEFT
        if (Math.Sign(horizontalInput) == -controller.SkeletonMecanim.skeleton.ScaleX && horizontalInput != 0)
        {
            rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        if (controller.Grounded)
        {
            controller.ChangeState(IdleState.INSTANCE);
        }
        else if (controller.WallTrigger == WallTrigger.NONE)
        {
            if (rigidbody.velocity.y * controller.FlipGravityScale > 0)
            {
                controller.ChangeState(JumpingState.INSTANCE);
            }
            else
            {
                controller.ChangeState(AirborneState.INSTANCE);
            }
        }
    }

    public override void Exit()
    {
        controller.Animator.SetBool("WallCling", false);
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
            controller.ChangeState(DashingState.INSTANCE);
        }
    }

    public override void ToggleGlitch()
    {
        controller.ChangeState(WallClingingState.INSTANCE);
    }
}
