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

        if (rigidBody.velocity.magnitude < idleSpeedThreshold)
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
        if (!isGrounded && Math.Sign(newVelocity.x) != Math.Sign(rigidBody.velocity.x))
        {
            newVelocity.x *= 0.5f;
        }

        if (newVelocity.x == 0) //Check speed issues, also latency
        {
            rigidBody.AddForce(new Vector2(-rigidBody.velocity.x * rigidBody.mass, 0), ForceMode2D.Impulse);
        }
        else /*if (Math.Abs(rigidBody.velocity.x) <= movementSpeed || Math.Sign(newVelocity.x) != Math.Sign(rigidBody.velocity.x))*/
        {
            rigidBody.AddForce(new Vector2(newVelocity.x - rigidBody.velocity.x, -rigidBody.velocity.y * (1 - maxVelocityFix)), ForceMode2D.Impulse);
        }

        if (Math.Abs(newVelocity.y) > 0) //Might have to multiply by flipGravityScale instead
        {
            rigidBody.AddForce(new Vector2(0, newVelocity.y), ForceMode2D.Impulse);
            newVelocity.y = 0;
        }

        //rigidBody.velocity = new Vector2(newVelocity.x, newVelocity.y * maxVelocityFix); // Ta inn maxVelocityFix, mye renere
        if (rigidBody.gravityScale != newGravityScale)
            rigidBody.gravityScale = newGravityScale;
    }

    public override void Exit() {}
}
