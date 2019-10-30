using System;
using UnityEngine;

public class OnWallJump : AActionState
{
    // used to calculate input. Always set to 0 at entry and exit functions.
    private float lastInput;

    public float LastInput { get => lastInput; set => lastInput = value; }
    protected override AActionState CheckTriggers()
    {
        AActionState temp = null;
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
            if (Math.Abs(Body.velocity.y) <= 6 &&
                PlayerModel.WallTrigger == -Math.Sign(StateMachine.HorizontalInput * PlayerModel.FlipGravityScale) &&
                !StateMachine.JumpInput && !StateMachine.DashInput)
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
        return temp;
    }


    protected override void Start()
    {
        Body = GameObject.Find("View").GetComponent<Rigidbody2D>();
        StateName = " - Jumping from wall.";
        IsActive = false;
    }

    protected override void Update()
    {
        if (IsActive)
        {
            // check if any other states can be transitioned into
            this.TargetTransitionState = CheckTriggers();
            HandleJumpInput();
        }
    }

    internal override void EntryAction()
    {
        base.EntryAction();
    }

    internal override void ExitAction()
    {
        this.TargetTransitionState = null;
        IsActive = false;
    }

    internal override StateTransition GetTransition()
    {
        if (this.TargetTransitionState == this || this.TargetTransitionState == null)
        {
            return new StateTransition(null, null, TransitionType.No);
        }
        else
        {
            return new StateTransition(this, TargetTransitionState, TransitionType.Sibling);
        }
    }

    private void HandleJumpInput()
    {
        WallJump();
    }

    private void WallJump()
    {
        Body.constraints = RigidbodyConstraints2D.FreezeRotation;
        PlayerModel.NewGravityScale = PlayerModel.BaseGravityScale * PlayerModel.JumpingGravityScaleMultiplier * PlayerModel.FlipGravityScale;
        PlayerModel.WallJumpTime = Time.time;
        PlayerModel.JumpTime = Time.time;
        if (Math.Abs(StateMachine.HorizontalInput) >= 0.3) // "Move-jump"
            PlayerModel.NewVelocity = new Vector2(PlayerModel.WallJumpDirection * PlayerModel.DashSpeed, 0.8f * PlayerModel.GroundJumpSpeed - Body.velocity.y) * PlayerModel.FlipGravityScale; //jumpSpeed * 0.64f * Math.Sign(newGravityScale)
        else // Actually jump..
            PlayerModel.NewVelocity = new Vector2(PlayerModel.WallJumpDirection * PlayerModel.MovementSpeed, PlayerModel.GroundJumpSpeed - Body.velocity.y) * PlayerModel.FlipGravityScale; // jumpSpeed * 0.75f * Math.Sign(newGravityScale)
    }
}