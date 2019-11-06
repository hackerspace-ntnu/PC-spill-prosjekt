public interface IStateMachine
{
    DeadState DeadState { get; }
    AliveState AliveState { get; }
    UpwardsInAirState UpwardsInAirState { get; }
    HoveringInAirState HoveringInAirState { get; }
    FallingInAirState FallingInAirState { get;}
    OnWallState OnWallState { get; }
    OnGroundState OnGroundState { get;}
    OnDashState OnDashState { get; set; }
    OnNoActionState OnNoActionState { get; }
    OnAirJumpState OnAirJumpState { get; }
    OnJumpState OnJumpState { get;  }
    float HorizontalInput { get;  }
    float VerticalInput { get; }
    bool DashInput { get;  }
    bool JumpInput { get; }
    bool GrapHookInput { get; }
    OnWallClingState OnWallClingState { get; set; }
    OnWallJump OnWallJump { get; set; }
}