using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : PlayerState
{
    public static readonly JumpingState INSTANCE = new JumpingState();

    public override string Name => "JUMPING";

    private float maxVelocityY = 12;
    private float maxVelocityFix;

    private bool isGrounded;
    private float newGravityScale;

    public override void Enter()
    {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {
        HandleHorizontalInput();

        if (rigidBody.velocity.y * flipGravityScale < 0.0f)
        {
            controller.ChangeState(AirborneState.INSTANCE);
        }
    }

    public override void FixedUpdate()
    {
        rigidBody.AddForce(new Vector2(0, -rigidBody.velocity.y * rigidBody.mass * 2));
        if (Math.Sign(rigidBody.gravityScale) == 1 && rigidBody.velocity.y <= -maxVelocityY || Math.Sign(rigidBody.gravityScale) == -1 && rigidBody.velocity.y >= maxVelocityY)
        {
            maxVelocityFix = 0.8f;
        }

        else
        {
            maxVelocityFix = 1f;
        }

        // decreases horizontal speed in air while falling ( I think?)
        if (!isGrounded && Math.Sign(targetVelocity.x) != Math.Sign(rigidBody.velocity.x))
        {
            targetVelocity.x *= 0.5f;
        }

        float newVelocityX = targetVelocity.x - rigidBody.velocity.x;
        float newVelocityY = targetVelocity.y - rigidBody.velocity.y * (1 - maxVelocityFix);

        rigidBody.AddForce(new Vector2(newVelocityX, newVelocityY), ForceMode2D.Impulse);
        targetVelocity.y = 0;
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }
}
