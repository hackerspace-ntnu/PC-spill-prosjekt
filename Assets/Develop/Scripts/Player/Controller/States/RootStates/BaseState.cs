using System;
using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    private bool canTransitionTo;
    private bool canTransitionFrom;
    private bool isActive;

    private string stateName;

    private bool runningInternalUpdate;

    private BaseState targetTransitionState;
    private IStateMachine stateMachine;
    private IPlayerModel playerModel;

    protected virtual bool CanTransitionTo
    {
        get => canTransitionTo;
        set
        {
            canTransitionTo = value;
            canTransitionFrom = !value;
        }
    }

    protected virtual bool CanTransitionFrom
    {
        get => canTransitionFrom;
        set
        {
            canTransitionFrom = value;
            canTransitionTo = !value;
        }
    }

    protected bool RunningInternalUpdate { get => runningInternalUpdate; set => runningInternalUpdate = value; }

    public IStateMachine StateMachine
    {
        get => stateMachine;
        set
        {
            if (value != null)
            {
                stateMachine = value;
            }
            else
            {
                stateMachine = value;
            }
        }
    }

    public bool IsActive { get => isActive; set => isActive = value; }

    public IPlayerModel PlayerModel
    {
        get => playerModel;
        set
        {
            if (value != null)
            {
                playerModel = value;
            }
        }
    }

    protected virtual BaseState TargetTransitionState
    {
        get => targetTransitionState;
        set
        {
            targetTransitionState = value;
        }
    }

    public string StateName { get => stateName; set => stateName = value; }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        isActive = false; // Disable all states at start, let the statemachine.cs control who is active.
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isActive)
        {
            TargetTransitionState = CheckTriggers<BaseState>(null);
        }
    }

    protected virtual void FixedUpdate()
    {
    }

    // An activity executed when entering the state
    internal virtual void EntryAction()
    {
        CanTransitionTo = false;
        IsActive = true;
    }

    // An activity executed when exiting the state
    internal virtual void ExitAction()
    {
        IsActive = false;
        CanTransitionTo = true;
    }

    // Utility method to run all trigger functions. A triggering activity that causes a transition to occur.
    protected virtual BaseState CheckTriggers<T>(Rigidbody2D body) where T : BaseState //
    {
        BaseState temp = null; // defaults to null, but have to be assigned because compiler is annoying me.
        if (typeof(T) == typeof(DeadState) || typeof(T) == typeof(AliveState))
        {
            if (PlayerModel.HealthPoints > 0)
            {
                temp = StateMachine.AliveState;
            }
            else
            {
                temp = StateMachine.DeadState;
            }
        }
        else if (typeof(T) == typeof(FallingInAirState) || typeof(T) == typeof(HoveringInAirState) ||
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
        else if (typeof(T) == typeof(OnDashState) || typeof(T) == typeof(OnJumpState) || typeof(T) == typeof(OnAirJumpState)
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

    internal Transition<BaseState> GetTransition()
    {
        if (this.TargetTransitionState == this || this.TargetTransitionState == null)
        {
            return new Transition<BaseState>(null, null, TransitionType.No);
        }
        else
        {
            return new Transition<BaseState>(this, this.TargetTransitionState, TransitionType.Sibling);
        }
    }
}