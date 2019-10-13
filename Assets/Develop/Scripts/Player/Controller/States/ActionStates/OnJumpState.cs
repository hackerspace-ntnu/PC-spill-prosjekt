using System;
using UnityEngine;

public class OnJumpState : BaseState
{
    // used to calculate input. Always set to 0 at entry and exit functions.
    private float lastInput;
    private bool isGroundJump;
    private Rigidbody2D rigidbody;



    public float LastInput { get => lastInput; set => lastInput = value; }
    public Rigidbody2D Rigidbody { get => rigidbody; set => rigidbody = value; }
    protected override BaseState TargetTransitionState { get => base.TargetTransitionState; set => base.TargetTransitionState = value; }
    public bool IsGroundJump { get => isGroundJump; set => isGroundJump = value; }

    protected override BaseState CheckTriggers<T>(Rigidbody2D body)
    {
        // We just return "on no action state", because our update method will apply the jump force, and we only need to do this once! :)
        // + you cant jump and dash at the same time anyway (not literary, but you cant press both buttons at the same time and get a "combined" physics push)
        return StateMachine.OnNoActionState;
    }

    protected override void FixedUpdate()
    {
    }

    protected override void Start()
    {
        IsGroundJump = true;
        StateName = "Jumping!";
        IsActive = false;
        Rigidbody = GameObject.Find("View").GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        if (IsActive)
        {
            // check if any other states can be transitioned into
            this.TargetTransitionState = CheckTriggers<BaseState>(Rigidbody);
            HandleJumpInput();
        }
    }

    internal override void EntryAction()
    {
        // TODO: Add force of jumping? Since some state detected jump press and transitioned to this state, we should assume the player should jump!
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
    internal void GroundJump()
    {
        PlayerModel.VerticalVelocity = PlayerModel.GroundJumpSpeed * PlayerModel.FlipGravityScale;
        PlayerModel.IsVelocityDirty = true;
        PlayerModel.IsGrounded = false;
        PlayerModel.JumpTime = Time.time;
        PlayerModel.NewGravityScale = PlayerModel.JumpingGravityScaleMultiplier * PlayerModel.BaseGravityScale * PlayerModel.FlipGravityScale;
    }


    internal void AirJump(float velocityY, float gravity)
    {
        PlayerModel.HasAirJumped = true;
        PlayerModel.VerticalVelocity = (PlayerModel.AirJumpSpeed - velocityY) * PlayerModel.FlipGravityScale;
        PlayerModel.IsVelocityDirty = true;
        PlayerModel.IsGrounded = false;
        PlayerModel.JumpTime = Time.time;
    }
}