﻿using System;
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


    internal void WallJumpingState(in float input)
    {
       MovementStat temp = MovementStat.STANDARD;
        if(iWallClingRules.PlayerWallClingState == WallClingState.CLINGING) {
            if (Math.Abs(input) >= 0.3) // "Move-jump"
                iWallClingRules.NewVelocity = new Vector2(iWallClingRules.WallJumpDirection * iWallClingRules.DashSpeed, iWallClingRules.GroundJumpSpeed * 0.64f) * iWallClingRules.FlipGravityScale; //jumpSpeed * 0.64f * Math.Sign(newGravityScale)
            else // Actually jump.. 
                iWallClingRules.NewVelocity = new Vector2(iWallClingRules.WallJumpDirection * iWallClingRules.MovementSpeed, iWallClingRules.GroundJumpSpeed * 0.75f) * iWallClingRules.FlipGravityScale; // jumpSpeed * 0.75f * Math.Sign(newGravityScale)
            iWallClingRules.IsVelocityDirty = true;
            iWallClingRules.MoveState = temp;
            if(iWallClingRules.WallJumpDirection == 1)
            {
                iWallClingRules.PlayerWallClingState = WallClingState.LEAVING_LEFT;
            }
            if(iWallClingRules.WallJumpDirection == -1)
            {
                iWallClingRules.PlayerWallClingState = WallClingState.LEAVING_RIGHT;
            }

        }
        else
        {
            iWallClingRules.PlayerWallClingState = WallClingState.DEFAULT;
            iWallClingRules.MoveState = temp;
        }
        iWallClingRules.MoveState = temp;
    }

}
