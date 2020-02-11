using System;
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


        if (controller.WallTrigger == 1) {
            controller.SkeletonMecanism.skeleton.ScaleX = 1;
        } else {
            controller.SkeletonMecanism.skeleton.ScaleX = -1;
        }

        if (Input.GetButtonDown("Jump") || Time.time - controller.JumpButtonPressTime < 0.2f)
        {
            //rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            //controller.JumpTime = Time.time;
            //controller.HasDashed = false;
            //controller.HasAirJumped = false;
            controller.ChangeState(JumpingState.INSTANCE);
        }

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
        maxVelocityY = baseMaxVelocityY;
    }

    /* Why the fuck doesn't this work?
    public override void Jump()
    {
        controller.ChangeState(JumpingState.INSTANCE);
    }*/

    public override void Dash()
    {
        if (!controller.HasDashed)
        {
            controller.ChangeState(DashingState.INSTANCE);
        }
    }
}
