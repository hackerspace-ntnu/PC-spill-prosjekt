using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalDirectionController : MonoBehaviour
{

    private float lastInput;

    private IMove moveRules;

    internal IMove MoveRules
    {

        set
        {
            moveRules = value;
        }
    }

    public float LastInput
    {
        get
        {
            return lastInput;
        }

        set
        {
            lastInput = value;
        }
    }

    internal void MoveCharacter(float input)
    {
        if (Math.Abs(input) <= 0.1) //Beholde?
        {
            LastInput = 0;
        }
        else if (Math.Abs(LastInput) <= Math.Abs(input))
        {
            LastInput = input;
            if (Math.Abs(LastInput) > moveRules.HorizontalInputRunningThreshold)
            {
                moveRules.PlayerWalkState = WalkState.WALKING;
                moveRules.HorizontalVelocity = Math.Sign(LastInput) * moveRules.MovementSpeed * moveRules.FlipGravityScale; // Set horizontalInput to max
            }
            else
            {
                moveRules.HorizontalVelocity = LastInput * moveRules.MovementSpeed * moveRules.FlipGravityScale;
            }
        }
        else
        {
            moveRules.HorizontalVelocity = 0;
            LastInput = input;
        }
    }

}
