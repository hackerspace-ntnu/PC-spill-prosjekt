using System;
using UnityEngine;

public class OnNoActionState : AActionState
{
    // used to calculate input. Always set to 0 at entry and exit functions.
    private float lastInput;


    public float LastInput { get => lastInput; set => lastInput = value; }

    protected override AActionState CheckTriggers( )
    {
        AActionState temp = null;
        // Player on ground..
        if (PlayerModel.IsGrounded)
        {
            if (StateMachine.JumpInput)
            {
                temp = StateMachine.OnJumpState;
            }
            // Time.time >= PlayerModel.LastDashTime + PlayerModel.
            else if (StateMachine.DashInput && (Time.time >= PlayerModel.LastDashTime + PlayerModel.DashDuration))
            {
                temp = StateMachine.OnDashState;
            }
            else if (StateMachine.GraphHookInput)
            {
                return temp;
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
            else if (StateMachine.DashInput && (Time.time >= PlayerModel.LastDashTime + PlayerModel.DashDuration))
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
        StateName = "- No action -";
        IsActive = false;
    }

    protected override void Update()
    {
        if (IsActive)
        {
            // check if any other states can be transitioned into
            this.TargetTransitionState = CheckTriggers();
            UpdateActionVariables();
        }
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

    private void UpdateActionVariables()
    {
        UpdateDashVariables();
        UpdateAirJumpVariables();
    }


    private void UpdateDashVariables()
    {
         if (((Time.time >= PlayerModel.LastDashTime + PlayerModel.DashDuration)
            && PlayerModel.HasDashed) || (PlayerModel.HasDashed && PlayerModel.WallTrigger != 0))
        {
            PlayerModel.HasDashed = false;
            //PlayerModel.NewGravityScale = PlayerModel.BaseGravityScale * PlayerModel.FlipGravityScale;
            PlayerModel.HorizontalVelocity = 0;
        }
        else
        {
            return;
        }
    }
    private void UpdateAirJumpVariables()
    {
        if (PlayerModel.IsGrounded)
        {
            PlayerModel.HasAirJumped = false;
        }
        else
        {
            return;
        }
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
}