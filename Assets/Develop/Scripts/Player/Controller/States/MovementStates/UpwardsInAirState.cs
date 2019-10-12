using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpwardsInAirState : BaseState
{

    public Rigidbody2D Rigidbody { get => rigidbody; private set => rigidbody = value; }

    private Rigidbody2D rigidbody;
    protected override BaseState CheckTriggers<T>(Rigidbody2D body)
    {
        BaseState temp = null;
        if (typeof(T) == typeof(FallingInAirState) || typeof(T) == typeof(HoveringInAirState) ||
            typeof(T) == typeof(UpwardsInAirState) || typeof(T) == typeof(OnWallState) ||
            typeof(T) == typeof(OnGroundState))
        {
            // Player is air jumping. His movement state will be "Moving upwards in air". Note: We don't sett "has air jumped",
            // as action states execute the physical logic for the jump.
            if (StateMachine.JumpInput && !PlayerModel.HasAirJumped &&
                (Time.time >= PlayerModel.JumpTime + PlayerModel.MinimumTimeBeforeAirJump))
            {
                temp = StateMachine.UpwardsInAirState;
            }
            // Player is clinging to wall. His movement state will be "On wall state".
            // Player pressing left input (A) towards the wall on his left side.
            else if (!StateMachine.JumpInput && PlayerModel.WallTrigger == -1 &&
                StateMachine.HorizontalInput < 0)
            {
                temp = StateMachine.OnWallState;
            }
            // Player is clinging to wall. His movement state will be "On wall state".
            // Player pressing left input (D) towards the wall on his right side.
            else if (!StateMachine.JumpInput && PlayerModel.WallTrigger == 1 &&
                StateMachine.HorizontalInput > 0)
            {
                temp = StateMachine.OnWallState;
            }
            // if no input and no walls, check for current rigid body velocity. Might need rework for upside down-gravity.
            else if ((!StateMachine.JumpInput && PlayerModel.WallTrigger == 0) && Math.Abs(body.velocity.y * Math.Sign(body.gravityScale)) > 0)
            {
                // is velocity going upwards?
                if (body.velocity.y * Math.Sign(body.gravityScale) > 0)
                {
                    temp = StateMachine.UpwardsInAirState;
                }
                // is velocity going downwards?
                else if (body.velocity.y * Math.Sign(body.gravityScale) < 0)
                {
                    temp = StateMachine.FallingInAirState;
                }
                else
                {
                    temp = StateMachine.HoveringInAirState;
                }

            }
            else if (PlayerModel.IsGrounded && body.velocity.y == 0)
            {
                temp = StateMachine.OnGroundState;
            }
            else
            {
                temp = null;
            }
        }
        return temp;
    }

    internal override void EntryAction()
    {
    }

    internal override void ExitAction()
    {
        base.ExitAction();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }


    protected override void Start()
    {
        base.Start();
        Rigidbody = GameObject.Find("View").GetComponent<Rigidbody2D>();

    }

    protected override void Update()
    {
    }
}
