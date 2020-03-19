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
        controller.HasDashed = false;
        controller.HasAirJumped = false;

        if (controller.WallTrigger == WallTrigger.LEFT)
            controller.SkeletonMecanim.skeleton.ScaleX = 1;
        else
            controller.SkeletonMecanim.skeleton.ScaleX = -1;

        Debug.Log(controller.WallTrigger);

        // Buffered jump
        if (Time.time - controller.JumpButtonPressTime < 0.125f)
        {
            controller.ChangeState(JumpingState.INSTANCE);
            return;
        }

        controller.Animator.SetBool("WallCling", true);
    }

    public override void Update()
    {
        base.Update();

        // Has to check here too because of weird edgecase
        if (controller.WallTrigger == WallTrigger.LEFT)
            controller.SkeletonMecanim.skeleton.ScaleX = 1;
        else
            controller.SkeletonMecanim.skeleton.ScaleX = -1;

        //Slides down the wall slowly if player holds towards the wall
        if (Math.Sign(horizontalInput) == -(int) controller.WallTrigger)
            maxVelocityY = wallSlideMaxVelocityY;
        else
            maxVelocityY = baseMaxVelocityY;

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

    public override void FixedUpdate()
    {
        if (controller.FlipGravityScale == 1 && rigidbody.velocity.y <= -maxVelocityY
            || controller.FlipGravityScale == -1 && rigidbody.velocity.y >= maxVelocityY)
        {
            maxVelocityFix = 0.2f;
        }
        else
        {
            maxVelocityFix = 0f;
        }

        float newVelocityX = controller.TargetVelocity.x - rigidbody.velocity.x;
        float newVelocityY = controller.TargetVelocity.y - rigidbody.velocity.y * maxVelocityFix;

        rigidbody.AddForce(new Vector2(newVelocityX, newVelocityY), ForceMode2D.Impulse);
        controller.TargetVelocity = new Vector2(controller.TargetVelocity.x, 0);
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
