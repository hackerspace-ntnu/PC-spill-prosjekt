﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneState : PlayerState
{
    public static readonly AirborneState INSTANCE = new AirborneState();

    public override string Name => "AIRBORNE";

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {

        if (controller.Grounded) {
            controller.ChangeState(IdleState.INSTANCE);
        }
        else if (controller.WallTrigger != 0)
        {
            controller.ChangeState(WallClingingState.INSTANCE);
        }

        base.Update();
    }

    public override void FixedUpdate() {
        if (Math.Sign(rigidBody.gravityScale) == 1 && rigidBody.velocity.y <= -maxVelocityY ||
            Math.Sign(rigidBody.gravityScale) == -1 && rigidBody.velocity.y >= maxVelocityY)
        {
            maxVelocityFix = 0.2f;
        }
        else
        {
            maxVelocityFix = 0f;
        }

        float newVelocityX;
        // decreases horizontal acceleration in air while input in opposite direction of velocity
        if (Math.Sign(controller.TargetVelocity.x) != Math.Sign(rigidBody.velocity.x))
        {
            newVelocityX = controller.TargetVelocity.x * 0.2f - rigidBody.velocity.x * 0.1f;
        }
        else
        {
            newVelocityX = controller.TargetVelocity.x - rigidBody.velocity.x;
        }

        float newVelocityY = controller.TargetVelocity.y - rigidBody.velocity.y * maxVelocityFix;

        rigidBody.AddForce(new Vector2(newVelocityX, newVelocityY), ForceMode2D.Impulse);
        controller.TargetVelocity = Vector2.zero;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Jump() {
        controller.ChangeState(JumpingState.INSTANCE);
    }
}
