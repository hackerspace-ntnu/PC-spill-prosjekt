using System;
using UnityEngine;

public class OnNoActionState : BaseState
{
    private Rigidbody2D rigidbody;
    public Rigidbody2D Rigidbody { get => rigidbody; set => rigidbody = value; }
    protected override BaseState CheckTriggers<T>(Rigidbody2D body)
    {
        BaseState temp = null;
        if (typeof(T) == typeof(OnDashState) || typeof(T) == typeof(OnJumpState) || typeof(T) == typeof(OnAirJumpState)
            || typeof(T) == typeof(OnNoActionState) || typeof(T) == typeof(OnWallClingState) || typeof(T) == typeof(OnWallJump))
        {
            // Player on ground..
            if (PlayerModel.IsGrounded)
            {
                if (StateMachine.JumpInput)
                {
                    temp = StateMachine.OnJumpState;
                }
                else if (StateMachine.DashInput && (Time.time - PlayerModel.LastDashTime <= PlayerModel.DashDuration))
                {
                    temp = StateMachine.OnDashState;
                }
                else
                {
                    temp = StateMachine.OnNoActionState;
                }
            }
            // Player in air, and not close to any walls
            else if (!PlayerModel.IsGrounded && PlayerModel.WallTrigger == 0)
            {
                if (!PlayerModel.HasAirJumped && StateMachine.JumpInput
                    && (Time.time >= PlayerModel.JumpTime + PlayerModel.MinimumTimeBeforeAirJump && !PlayerModel.HasAirJumped))
                {
                    temp = StateMachine.OnAirJumpState;
                }
                else if (StateMachine.DashInput && (Time.time - PlayerModel.LastDashTime <= PlayerModel.DashDuration))
                {
                    temp = StateMachine.OnDashState;
                }
                else
                {
                    temp = StateMachine.OnNoActionState;
                }
            }
            // player close to wall
            else if (PlayerModel.WallTrigger != 0 && !PlayerModel.IsGrounded)
            {
                if (Math.Abs(body.velocity.y) <= 6 && PlayerModel.WallTrigger == -Math.Sign(StateMachine.HorizontalInput * PlayerModel.FlipGravityScale))
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
                        temp = StateMachine.OnWallJump;
                    }
                }
                else
                {
                    temp = StateMachine.OnNoActionState;
                }
            }
        }
        return temp;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Start()
    {
        IsActive = false;
        Rigidbody = GameObject.Find("View").GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        base.Update();
    }

    internal override void EntryAction()
    {
        base.EntryAction();
    }

    internal override void ExitAction()
    {
        base.ExitAction();
    }
}