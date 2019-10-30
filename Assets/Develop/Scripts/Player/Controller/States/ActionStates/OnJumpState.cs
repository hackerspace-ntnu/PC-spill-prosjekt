using System;
using UnityEngine;

public class OnJumpState : AActionState
{
    // used to calculate input. Always set to 0 at entry and exit functions.
    private float lastInput;
    private bool isGroundJump;



    public float LastInput { get => lastInput; set => lastInput = value; }
    public bool IsGroundJump { get => isGroundJump; set => isGroundJump = value; }

    protected override AActionState CheckTriggers()
    {
        // We just return "on no action state", because our update method will apply the jump force, and we only need to do this once! :)
        // + you cant jump and dash at the same time anyway (not literary, but you cant press both buttons at the same time and get a "combined" physics push)
        return StateMachine.OnNoActionState;
    }


    protected override void Start()
    {
        Body = GameObject.Find("View").GetComponent<Rigidbody2D>();
        IsGroundJump = true;
        StateName = "Jumping!";
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
        IsActive = true;
        LastInput = 0;
    }

    internal override void ExitAction()
    {
        this.TargetTransitionState = null;
        IsActive = false;
        LastInput = 0;
    }
    private void HandleJumpInput()
    {
        if (PlayerModel.IsGrounded)
        {
            GroundJump();
        }
    }
    private void GroundJump()
    {
        PlayerModel.VerticalVelocity = PlayerModel.GroundJumpSpeed * PlayerModel.FlipGravityScale;
        PlayerModel.IsVelocityDirty = true;
        PlayerModel.IsGrounded = false;
        PlayerModel.JumpTime = Time.time;
        PlayerModel.NewGravityScale = PlayerModel.JumpingGravityScaleMultiplier * PlayerModel.BaseGravityScale * PlayerModel.FlipGravityScale;
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