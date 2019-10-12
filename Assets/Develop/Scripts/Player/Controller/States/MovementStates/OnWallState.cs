using System;
using UnityEngine;

public class OnWallState : BaseState
{
    private float lastInput;

    private Rigidbody2D rigidbody;

    public float LastInput { get => lastInput; set => lastInput = value; }
    public Rigidbody2D Rigidbody { get => rigidbody; set => rigidbody = value; }

    protected override BaseState TargetTransitionState { get => base.TargetTransitionState; set => base.TargetTransitionState = value; }

    protected override BaseState CheckTriggers<T>(Rigidbody2D rigidbody)
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
            else if ((!StateMachine.JumpInput && PlayerModel.WallTrigger == 0) && Math.Abs(rigidbody.velocity.y * Math.Sign(rigidbody.gravityScale)) > 0)
            {
                // is velocity going upwards?
                if (rigidbody.velocity.y * Math.Sign(rigidbody.gravityScale) > 0)
                {
                    temp = StateMachine.UpwardsInAirState;
                }
                // is velocity going downwards?
                else if (rigidbody.velocity.y * Math.Sign(rigidbody.gravityScale) < 0)
                {
                    temp = StateMachine.FallingInAirState;
                }
                else
                {
                    temp = StateMachine.HoveringInAirState;
                }

            }
            else if (PlayerModel.IsGrounded && rigidbody.velocity.y == 0)
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
        IsActive = true;
        LastInput = 0;
    }

    internal override void ExitAction()
    {
        this.TargetTransitionState = null;
        IsActive = false;
        LastInput = 0;
    }

    protected override void FixedUpdate()
    {
    }

    protected override void Start()
    {
        StateName = "On wall state";
        Rigidbody = GameObject.Find("View").GetComponent<Rigidbody2D>();
        base.Start();
    }

    protected override void Update()
    {
        if (IsActive)
        {
            // check if any other states can be transitioned into
            this.TargetTransitionState = CheckTriggers<BaseState>(Rigidbody);

            // if no targeted states is found, handle horizontal movement input, other input (jump/dash etc) is handled in current actionstate.
            if (this.TargetTransitionState == null)
            {
                HandleHorizontalInput();
            }
        }
    }
    private void HandleHorizontalInput()
    {
        if (Math.Abs(StateMachine.HorizontalInput) <= 0.1) // Beholde?
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
}