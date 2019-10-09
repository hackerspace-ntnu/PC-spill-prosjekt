using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClingController : MonoBehaviour
{
    private IWallCling iWallClingRules;

    public IWallCling IWallClingRules
    {
        set
        {
            iWallClingRules = value;
        }
    }


    internal void WallJumpingState(float input, float rigidBodyVelocityY)
    {
        if (Math.Abs(input) >= 0.3) // "Move-jump"
            iWallClingRules.NewVelocity = new Vector2(iWallClingRules.WallJumpDirection * iWallClingRules.DashSpeed, 0.8f * iWallClingRules.GroundJumpSpeed - rigidBodyVelocityY) * iWallClingRules.FlipGravityScale; //jumpSpeed * 0.64f * Math.Sign(newGravityScale)
        else // Actually jump.. 
            iWallClingRules.NewVelocity = new Vector2(iWallClingRules.WallJumpDirection * iWallClingRules.MovementSpeed, iWallClingRules.GroundJumpSpeed - rigidBodyVelocityY) * iWallClingRules.FlipGravityScale; // jumpSpeed * 0.75f * Math.Sign(newGravityScale)

        if (iWallClingRules.WallJumpDirection == 1)
        {
            iWallClingRules.PlayerWallClingState = WallClingState.LEAVING_LEFT;
        }
        if (iWallClingRules.WallJumpDirection == -1)
        {
            iWallClingRules.PlayerWallClingState = WallClingState.LEAVING_RIGHT;
        }
        if (Time.time - iWallClingRules.WallJumpTime > 0.05f)
        {
            iWallClingRules.MoveState = MovementStat.STANDARD;
        }
    }

}
