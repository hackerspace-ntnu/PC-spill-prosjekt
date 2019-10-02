using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController : MonoBehaviour
{
    private IJump jumpRules;

    public IJump JumpRules
    {

        set
        {
            jumpRules = value;
        }
    }

    internal void Jumpstate()
    {
        // TODO: Move Jumpstate code from playercontroller here. 
    }
    internal void Jump(float input, float velocityY, float gravity)
    {
        if(jumpRules.PlayerInAirState == InAirState.ON_GROUND && Time.time >= jumpRules.JumpTime + jumpRules.MinimumTimeBeforeAirJump)
        {
            jumpRules.HasAirJumped = false;
            jumpRules.VerticalVelocity = (jumpRules.JumpSpeed * input + Mathf.Abs(velocityY)) * jumpRules.FlipGravityScale;
            jumpRules.IsVelocityDirty = true;
            jumpRules.IsGrounded = false;
            jumpRules.JumpTime = Time.time;
        }
        else if(jumpRules.PlayerInAirState != InAirState.ON_GROUND)
        {
            if(Time.time >= jumpRules.JumpTime + jumpRules.MinimumTimeBeforeAirJump && !jumpRules.HasAirJumped)
            {
                jumpRules.HasAirJumped = true;
                jumpRules.MoveState = MovementStat.AIR_JUMPING;
                jumpRules.PlayerInAirState = InAirState.UPWARDS;
                jumpRules.VerticalVelocity = (jumpRules.JumpSpeed * input + Mathf.Abs(velocityY)) * jumpRules.FlipGravityScale;
                jumpRules.IsVelocityDirty = true;
                jumpRules.IsGrounded = false;
                jumpRules.JumpTime = Time.time;
            }
            else
            {
                UpdateInAirState(velocityY, gravity);
            }
        }
    }

    private void UpdateInAirState(float input, float gravity)
    {
        if(input * Math.Sign(gravity) > 0)
        {
            jumpRules.PlayerInAirState = InAirState.UPWARDS;
        }
        else if (input * Math.Sign(gravity) < 0)
        {
            jumpRules.PlayerInAirState = InAirState.DOWNWARDS;
        }
        else
        {
            jumpRules.PlayerInAirState = InAirState.HOVERING;
        }
    }

}
