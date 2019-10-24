using System;
using UnityEngine;


/// <summary>
///
/// </summary>
public class OnWallClingState : AActionState
{
    // used to calculate input. Always set to 0 at entry and exit functions.
    private float lastInput;

    public float LastInput { get => lastInput; set => lastInput = value; }

    protected override AActionState CheckTriggers()
    {
        AActionState temp = null;
        // player close to wall
        if (PlayerModel.WallTrigger != 0 && !PlayerModel.IsGrounded)
        {
            if (Math.Abs(Body.velocity.y) <= 6 && PlayerModel.WallTrigger == -Math.Sign(StateMachine.HorizontalInput * PlayerModel.FlipGravityScale)
                && !StateMachine.JumpInput)
            {
                temp = this;
            }
            else if (StateMachine.DashInput && (Time.time - PlayerModel.LastDashTime <= PlayerModel.DashDuration))
            {
                temp = StateMachine.OnDashState;
            }
            else if (StateMachine.JumpInput)
            {
                if (Math.Abs(StateMachine.HorizontalInput) >= 0.3)
                {
                    temp = this;
                    //temp = StateMachine.OnJumpState; TODO : Only let players jump from wall if glitching is enabled
                }
                else
                {
                    temp = this;
                    // temp = StateMachine.OnJumpState; TODO : Only let players jump from wall if glitching is enabled
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
        PlayerModel.WallJumpDirection = PlayerModel.WallTrigger;
        StateName = "- Clinging to wall - ";
        IsActive = false;
    }

    protected override void Update()
    {
        if (IsActive)
        {
            // check if any other states can be transitioned into
            this.TargetTransitionState = CheckTriggers();

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
        PlayerModel.NewGravityScale = PlayerModel.BaseGravityScale * PlayerModel.FlipGravityScale; // fix gravity back to normal!
        Body.constraints = RigidbodyConstraints2D.FreezePosition;
        this.TargetTransitionState = null;
        IsActive = false;
        LastInput = 0;
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

    private void HandleInput()
    {
        //if (StateMachine.JumpInput)
        // {
            // PlayerModel.NewVelocity = new Vector2(PlayerModel.WallJumpDirection * PlayerModel.MovementSpeed, PlayerModel.GroundJumpSpeed - Body.velocity.y) * PlayerModel.FlipGravityScale; // jumpSpeed * 0.75f * Math.Sign(newGravityScale)
        // }
        if (Body.velocity.y * Math.Sign(PlayerModel.NewGravityScale) <= 0)
        {
            PlayerModel.NewGravityScale = (PlayerModel.BaseGravityScale/3)  * PlayerModel.FlipGravityScale; // PlayerModel.WallslideGravityScaleMultiplier
        }
        else if (Math.Abs(Body.velocity.y) <= 6 && PlayerModel.WallTrigger == -Math.Sign(StateMachine.HorizontalInput * PlayerModel.FlipGravityScale))
        {
            Body.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            Body.constraints = RigidbodyConstraints2D.FreezeRotation;
            PlayerModel.MaxVelocityY = 2f;
        }
    }
}