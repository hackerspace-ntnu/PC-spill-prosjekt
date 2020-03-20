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

        // Set the sprite direction to face away from the wall they are clinging to
        controller.SkeletonMecanim.skeleton.ScaleX = (int)controller.WallTrigger;

        //Slides down the wall slowly if player holds towards the wall
        if (Math.Sign(horizontalInput) == -(int)controller.WallTrigger)
        {
            maxVelocityY = wallSlideMaxVelocityY;
            if (Math.Sign(rigidbody.velocity.y) != controller.FlipGravityScale)
            {
                rigidbody.gravityScale = controller.FlipGravityScale;
            }
        }
        else {
            maxVelocityY = baseMaxVelocityY;
            rigidbody.gravityScale = baseGravityScale * controller.FlipGravityScale;
        }

        if (controller.Grounded)
        {
            controller.ChangeState(IdleState.INSTANCE);
        }
        else if (controller.WallTrigger == WallTrigger.NONE)
        {
            controller.ChangeState(AirborneState.INSTANCE);
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
        float newVelocityY = - rigidbody.velocity.y * maxVelocityFix;

        if (Math.Sign(rigidbody.velocity.y) == controller.FlipGravityScale) // To ensure speed buildup when doing repeated buildups doesn't happen (as much)
        {
            newVelocityY = controller.TargetVelocity.y - rigidbody.velocity.y * 0.4f;
        }

        rigidbody.AddForce(new Vector2(newVelocityX, newVelocityY), ForceMode2D.Impulse);
        controller.TargetVelocity = new Vector2(controller.TargetVelocity.x, 0);
    }

    public override void Exit()
    {
        controller.Animator.SetBool("WallCling", false);
        maxVelocityY = baseMaxVelocityY;
        rigidbody.gravityScale = baseGravityScale * controller.FlipGravityScale;
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
