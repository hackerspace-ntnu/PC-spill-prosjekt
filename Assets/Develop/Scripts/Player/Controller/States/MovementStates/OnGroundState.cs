﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class OnGroundState : APositionState
{
    // used to calculate input. Always set to 0 at entry and exit functions.
    private float lastInput;

    public float LastInput { get => lastInput; set => lastInput = value; }


    protected override APositionState CheckTriggers()
    {
        APositionState temp = null;
        // Player is air jumping. His movement state will be "Moving upwards in air". Note: We don't sett "has air jumped",
        // as action states execute the physical logic for the jump.
        if (StateMachine.JumpInput && !PlayerModel.HasAirJumped &&
            (Time.time >= PlayerModel.JumpTime + PlayerModel.MinimumTimeBeforeAirJump && !PlayerModel.IsGrounded))
        {
            temp = StateMachine.UpwardsInAirState;
        }
        // Player is clinging to wall. His movement state will be "On wall state".
        // Player pressing left input (A) towards the wall on his left side.
        else if (!StateMachine.JumpInput && PlayerModel.WallTrigger == -1 &&
            StateMachine.HorizontalInput < 0 && !PlayerModel.IsGrounded)
        {
            temp = StateMachine.OnWallState;
        }
        // Player is clinging to wall. His movement state will be "On wall state".
        // Player pressing left input (D) towards the wall on his right side.
        else if (!StateMachine.JumpInput && PlayerModel.WallTrigger == 1 &&
            StateMachine.HorizontalInput > 0 && !PlayerModel.IsGrounded)
        {
            temp = StateMachine.OnWallState;
        }
        // if no input and no walls, check for current rigid body velocity. Might need rework for upside down-gravity.
        else if ((!StateMachine.JumpInput && PlayerModel.WallTrigger == 0) && Math.Abs(Body.velocity.y * Math.Sign(Body.gravityScale)) > 0)
        {
            // is velocity going upwards?
            if (Body.velocity.y * Math.Sign(Body.gravityScale) > 0.2f)
            {
                temp = StateMachine.UpwardsInAirState;
            }
            // is velocity going downwards?
            else if (Body.velocity.y * Math.Sign(Body.gravityScale) < -0.2f)
            {
                temp = StateMachine.FallingInAirState;
            }
            else
            {
                temp = StateMachine.HoveringInAirState;
            }

        }
        else if (PlayerModel.IsGrounded && Body.velocity.y == 0)
        {
            temp = StateMachine.OnGroundState;
        }
        else
        {
            temp = null;
        }
        return temp;
    }


    internal override void EntryAction()
    {
        IsActive = true;
        LastInput = 0;
    }

    internal override void ExitAction()
    {
        this.TargetTransitionState = null;
        IsActive = false;
        LastInput = 0;
    }

    protected override void Start()
    {
        Body = GameObject.Find("View").GetComponent<Rigidbody2D>();
        StateName = "- On the ground -";
        IsActive = false;
    }

    protected override void Update()
    {
        if (IsActive)
        {
            TargetTransitionState = CheckTriggers();

            if (this.TargetTransitionState == null || this.TargetTransitionState == this)
            {
                HandleHorizontalInput();
            }
        }
    }

    private void HandleHorizontalInput()
    {
        if (Math.Abs(StateMachine.HorizontalInput) <= 0.1) //Beholde?
        {
            LastInput = 0;
        }
        else if (Math.Abs(LastInput) <= Math.Abs(StateMachine.HorizontalInput))
        {
            LastInput = StateMachine.HorizontalInput;
            if (Math.Abs(LastInput) > PlayerModel.HorizontalInputRunningThreshold)
            {
                PlayerModel.PlayerWalkState = WalkState.WALKING;
                PlayerModel.HorizontalVelocity = Math.Sign(LastInput) * PlayerModel.MovementSpeed * PlayerModel.FlipGravityScale; // Set horizontalInput to max
            }
            else
            {
                PlayerModel.HorizontalVelocity = LastInput * PlayerModel.MovementSpeed * PlayerModel.FlipGravityScale;
            }
        }
        else
        {
            PlayerModel.HorizontalVelocity = 0;
            LastInput = StateMachine.HorizontalInput;
        }
    }

    internal override StateTransition GetTransition()
    {
        if (TargetTransitionState == this || TargetTransitionState == null)
        {
            return new StateTransition(null, null, TransitionType.No);
        }
        else
        {
            return new StateTransition(this, TargetTransitionState, TransitionType.Sibling);
        }
    }
}
