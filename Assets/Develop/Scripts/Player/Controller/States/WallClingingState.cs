using System;
using System.Collections;
using System.Collections.Generic;
using GlobalEnums;
using UnityEngine;

public class WallClingingState : PlayerState
{
    public static readonly WallClingingState INSTANCE = new WallClingingState();

    public override string Name => "WALL_CLINGING";

    protected WallClingingState() {}

    public override void Enter()
    {
        if (Time.time - controller.JumpButtonPressTime < 0.2f)
        {
            controller.ChangeState(JumpingState.INSTANCE);
            return;
        }

        controller.Animator.SetBool("WallCling", true);
    }

    public override void Update()
    {
        base.Update();

        if (controller.WallTrigger == WallTrigger.LEFT)
            controller.SkeletonMecanim.skeleton.ScaleX = 1;
        else
            controller.SkeletonMecanim.skeleton.ScaleX = -1;

        if (Math.Sign(horizontalInput) == -(int) controller.WallTrigger)
            maxVelocityY = wallSlideMaxVelocityY;
        else
            maxVelocityY = baseMaxVelocityY;

        if (controller.Grounded)
            controller.ChangeState(IdleState.INSTANCE);
        else if (controller.WallTrigger == WallTrigger.NONE)
            controller.ChangeState(AirborneState.INSTANCE);
    }

    public override void Exit()
    {
        controller.Animator.SetBool("WallCling", false);
        maxVelocityY = baseMaxVelocityY;
    }

    public override void Jump()
    {
        controller.ChangeState(JumpingState.INSTANCE);
    }

    public override void Dash()
    {
        if (!controller.HasDashed)
            controller.ChangeState(DashingState.INSTANCE);
    }

    public override void ToggleGlitch()
    {
        controller.ChangeState(GlitchWallClingingState.INSTANCE);
    }
}
