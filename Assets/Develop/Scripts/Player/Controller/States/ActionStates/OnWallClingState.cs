﻿using System;
using UnityEngine;


/// <summary>
///
/// </summary>
public class OnWallClingState : BaseState
{


    // used to calculate input. Always set to 0 at entry and exit functions.
    private float lastInput;

    private Rigidbody2D rigidbody;

    public float LastInput { get => lastInput; set => lastInput = value; }
    public Rigidbody2D Rigidbody { get => rigidbody; set => rigidbody = value; }
    protected override BaseState TargetTransitionState { get => base.TargetTransitionState; set => base.TargetTransitionState = value; }


    protected override BaseState CheckTriggers<T>(Rigidbody2D body)
    {
        BaseState temp = null;
        // player close to wall
        if (PlayerModel.WallTrigger != 0 && !PlayerModel.IsGrounded)
        {
            if (Math.Abs(body.velocity.y) <= 6 && PlayerModel.WallTrigger == -Math.Sign(StateMachine.HorizontalInput * PlayerModel.FlipGravityScale)
                && !StateMachine.JumpInput)
            {
                temp = StateMachine.OnWallClingState;
            }
            else if (StateMachine.DashInput && (Time.time - PlayerModel.LastDashTime <= PlayerModel.DashDuration))
            {
                temp = StateMachine.OnDashState;
            }
            else if (StateMachine.JumpInput)
            {
                if (Math.Abs(StateMachine.HorizontalInput) >= 0.3)
                {
                    temp = StateMachine.OnJumpState;
                }
                else
                {
                    temp = StateMachine.OnJumpState;
                }
            }
            else
            {
                temp = StateMachine.OnNoActionState;
            }
        }
        return temp;
    }

    protected override void FixedUpdate()
    {
    }

    protected override void Start()
    {
        PlayerModel.WallJumpDirection = PlayerModel.WallTrigger;
        StateName = "Spider pig on dat wall.";
        IsActive = false;
        Rigidbody = GameObject.Find("View").GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        if (IsActive)
        {
            // check if any other states can be transitioned into
            this.TargetTransitionState = CheckTriggers<OnWallClingState>(Rigidbody);

            // if no targeted states is found, handle horizontal movement input, other input (jump/dash etc) is handled in current actionstate.
            if (this.TargetTransitionState == null || TargetTransitionState == this)
            {
                HandleInput();
            }
        }
    }

    internal override void EntryAction()
    {
        IsActive = true;
        LastInput = 0;
    }

    internal override void ExitAction()
    {
        Rigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
        this.TargetTransitionState = null;
        IsActive = false;
        LastInput = 0;
    }

    private void HandleInput()
    {
        if (StateMachine.JumpInput)
        {
            PlayerModel.NewVelocity = new Vector2(PlayerModel.WallJumpDirection * PlayerModel.MovementSpeed, PlayerModel.GroundJumpSpeed - Rigidbody.velocity.y) * PlayerModel.FlipGravityScale; // jumpSpeed * 0.75f * Math.Sign(newGravityScale)
        }
        if (Rigidbody.velocity.y * Math.Sign(PlayerModel.NewGravityScale) <= 0)
        {
            PlayerModel.NewGravityScale = PlayerModel.BaseGravityScale * PlayerModel.WallslideGravityScaleMultiplier * PlayerModel.FlipGravityScale;
        }
        if (Math.Abs(Rigidbody.velocity.y) <= 6 && PlayerModel.WallTrigger == -Math.Sign(StateMachine.HorizontalInput * PlayerModel.FlipGravityScale))
        {
            Rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            PlayerModel.MaxVelocityY = 2f;
        }
    }
}