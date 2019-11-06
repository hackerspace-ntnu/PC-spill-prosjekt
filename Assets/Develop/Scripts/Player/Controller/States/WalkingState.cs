using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : PlayerState
{
    public static readonly WalkingState INSTANCE = new WalkingState();

    protected float idleSpeedThreshold = 0.1f;
    private float maxVelocityY = 12;
    private float maxVelocityFix;

    private bool isGrounded;
    private float newGravityScale; // for setting velocity in FixedUpdate()

    public override string Name => "WALKING";

    protected WalkingState() {}

    public override void Enter() {}

    public override void Update()
    {
        HandleHorizontalInput();

        if (Input.GetKeyDown("Jump"))
        {
            controller.ChangeState(JumpingState.INSTANCE);
        }
        else if (rigidBody.velocity.magnitude < idleSpeedThreshold)
            controller.ChangeState(IdleState.INSTANCE);
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

    public override void Exit() {}
}
