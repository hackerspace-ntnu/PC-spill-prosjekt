using System;
using UnityEngine;

public class OnWallJump : BaseState
{
    // used to calculate input. Always set to 0 at entry and exit functions.
    private float lastInput;

    private Rigidbody2D rigidbody;

    public float LastInput { get => lastInput; set => lastInput = value; }
    public Rigidbody2D Rigidbody { get => rigidbody; set => rigidbody = value; }
    protected override BaseState CheckTriggers<T>(Rigidbody2D body)
    {
        return base.CheckTriggers<T>(body);
    }

    protected override void FixedUpdate()
    {
    }

    protected override void Start()
    {
        StateName = "Jumping from wall. No puns here. Sorry.";
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
        base.EntryAction();
    }

    internal override void ExitAction()
    {
        this.TargetTransitionState = null;
        IsActive = false;
    }

    private void HandleJumpInput()
    {
        WallJump();
    }

    private void WallJump()
    {
        Rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        PlayerModel.NewGravityScale = PlayerModel.BaseGravityScale * PlayerModel.JumpingGravityScaleMultiplier * PlayerModel.FlipGravityScale;
        PlayerModel.WallJumpTime = Time.time;
        PlayerModel.JumpTime = Time.time;
        if (Math.Abs(StateMachine.HorizontalInput) >= 0.3) // "Move-jump"
            PlayerModel.NewVelocity = new Vector2(PlayerModel.WallJumpDirection * PlayerModel.DashSpeed, 0.8f * PlayerModel.GroundJumpSpeed - Rigidbody.velocity.y) * PlayerModel.FlipGravityScale; //jumpSpeed * 0.64f * Math.Sign(newGravityScale)
        else // Actually jump..
            PlayerModel.NewVelocity = new Vector2(PlayerModel.WallJumpDirection * PlayerModel.MovementSpeed, PlayerModel.GroundJumpSpeed - Rigidbody.velocity.y) * PlayerModel.FlipGravityScale; // jumpSpeed * 0.75f * Math.Sign(newGravityScale)
    }
}