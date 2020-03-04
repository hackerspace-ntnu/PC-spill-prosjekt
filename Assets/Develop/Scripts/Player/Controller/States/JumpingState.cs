using System;
using System.Collections;
using System.Collections.Generic;
using GlobalEnums;
using UnityEngine;

public class JumpingState : PlayerState
{
    public static readonly JumpingState INSTANCE = new JumpingState();

    public override string Name => "JUMPING";

    private float wallJumpTime;

    private JumpingState() {}

    public override void Enter()
    {
        controller.Animator.SetBool("Jump", true);
        rigidbody.gravityScale = JUMPING_GRAVITY_SCALE * controller.FlipGravityScale;
        PlayerState prevInstance = controller.GetPreviousState();

        //Since all other logic is tested in these states, this logic is all we need
        if (prevInstance == AirborneState.INSTANCE)
            AirJump();
        else if (prevInstance == WallClingingState.INSTANCE || prevInstance == GlitchWallClingingState.INSTANCE)
            WallJump();
        else
            GroundJump();
    }

    public override void Update()
    {
        CheckGrappling();

        if (controller.WallTrigger != WallTrigger.NONE && Time.time - controller.JumpTime > 0.2f)
        {
            if (controller.GlitchActive)
                controller.ChangeState(GlitchWallClingingState.INSTANCE);
            else
                controller.ChangeState(WallClingingState.INSTANCE);
        }
        else if (IsHeadingDownwards())
            controller.ChangeState(AirborneState.INSTANCE);

        if (controller.Grounded)
            controller.ChangeState(IdleState.INSTANCE);

        if (Time.time - wallJumpTime > 0.05f)
            base.Update();
    }

    public bool IsHeadingDownwards()
    {
        return rigidbody.velocity.y * controller.FlipGravityScale < 0f && controller.TargetVelocity.y == 0f;
    }

    public override void FixedUpdate()
    {
        float newVelocityX;

        // decreases horizontal acceleration in air while input in opposite direction of velocity
        if (Math.Sign(controller.TargetVelocity.x) != Math.Sign(rigidbody.velocity.x))
        {
            newVelocityX = controller.TargetVelocity.x * 0.2f - rigidbody.velocity.x * 0.1f;
        }
        else
        {
            newVelocityX = controller.TargetVelocity.x - rigidbody.velocity.x;
        }


        float newVelocityY = 0f;
        if (controller.TargetVelocity.y != 0)
        {
            newVelocityY = controller.TargetVelocity.y - rigidbody.velocity.y;
        }

        rigidbody.AddForce(new Vector2(newVelocityX, newVelocityY), ForceMode2D.Impulse);
        controller.TargetVelocity = new Vector2(newVelocityX, 0);
    }

    public override void Exit()
    {
        controller.Animator.SetBool("Jump", false);
        rigidbody.gravityScale = baseGravityScale * controller.FlipGravityScale;
    }

    public override void Jump()
    {
        if (!controller.HasAirJumped && controller.GlitchActive) // && Time.time - controller.JumpTime > 0.2f)
            AirJump();
        else
            controller.JumpButtonPressTime = Time.time;
    }

    private void GroundJump()
    {
        controller.Grounded = false;

        controller.TargetVelocity = new Vector2(controller.TargetVelocity.x, groundJumpSpeed * controller.FlipGravityScale);
        controller.JumpTime = Time.time;

        Debug.Log("GroundJump");
    }

    private void AirJump()
    {
        controller.HasAirJumped = true;
        controller.TargetVelocity = new Vector2(controller.TargetVelocity.x, airJumpSpeed * controller.FlipGravityScale);
        controller.JumpTime = Time.time;
        Debug.Log("AirJump");
    }

    private void WallJump()
    {
        // Uses ScaleX instead of WallTrigger for edgecase where Jump() is called in WallClingingState before it realizes that WallTrigger = 0
        // This also makes the input slightly more lenient for the player if at the bottom of a wall
        if (Math.Abs(horizontalInput) >= 0.3f && Math.Sign(horizontalInput) == -controller.SkeletonMecanim.skeleton.ScaleX)
            controller.TargetVelocity = new Vector2(controller.SkeletonMecanim.skeleton.ScaleX * dashSpeed * 2f, airJumpSpeed) * controller.FlipGravityScale * 1.2f;
        else
            controller.TargetVelocity = new Vector2(controller.SkeletonMecanim.skeleton.ScaleX * movementSpeed * 1.5f, groundJumpSpeed) * controller.FlipGravityScale * 1.1f;

        controller.HasDashed = false;
        controller.HasAirJumped = false;
        wallJumpTime = Time.time;
        controller.JumpTime = Time.time;
        Debug.Log("WallJump");
        Debug.Log(controller.TargetVelocity);
    }

    public override void Dash()
    {
        if (controller.GlitchActive)
            controller.ChangeState(GlitchDashingState.INSTANCE);
        else
            controller.ChangeState(DashingState.INSTANCE);
    }
}
