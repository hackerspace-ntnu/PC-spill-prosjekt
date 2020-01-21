﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClingingState : PlayerState
{
    public static readonly WallClingingState INSTANCE = new WallClingingState();

    public override string Name => "WALL_CLINGING";

    public override void Enter()
    {
        HandleHorizontalInput();
        controller.Animator.SetBool("WallCling", true);
    }

    public override void Update()
    {
        HandleHorizontalInput();

        if (Math.Sign(horizontalInput) == -controller.WallTrigger)
        {
            maxVelocityY = wallSlideMaxVelocityY;
        }
        else
        {
            maxVelocityY = baseMaxVelocityY;
        }

        if (controller.Grounded)
        {
            controller.ChangeState(IdleState.INSTANCE);
        }
        else if (controller.WallTrigger == 0)
        {
            controller.ChangeState(AirborneState.INSTANCE);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        controller.Animator.SetBool("WallCling", false);
        rigidBody.gravityScale = baseGravityScale;
        maxVelocityY = baseMaxVelocityY;
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
}
