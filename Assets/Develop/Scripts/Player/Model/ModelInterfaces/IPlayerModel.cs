using UnityEngine;

public interface IPlayerModel
{
    int WallTrigger { get; set; }
    float AirJumpSpeed { get; set; }
    float BaseGravityScale { get; set; }
    float DashDuration { get; set; }
    float DashSpeed { get; set; }
    int FlipGravityScale { get; set; }
    GraphlingHookState GraphHookState { get; set; }
    float GroundJumpSpeed { get; set; }
    bool HasAirJumped { get; set; }
    bool HasDashed { get; set; }
    float HorizontalInput { get; set; }
    float HorizontalInputRunningThreshold { get; }
    float HorizontalVelocity { get; set; }
    bool IsGrounded { get; set; }
    bool IsVelocityDirty { get; set; }
    float JumpingGravityScaleMultiplier { get; }
    float JumpTime { get; set; }
    float LastDashTime { get; set; }
    float MaxVelocityFix { get; set; }
    float MaxVelocityY { get; set; }
    float MinimumTimeBeforeAirJump { get; }
    float MovementSpeed { get; set; }
    MovementStat MoveState { get; set; }
    float NewGravityScale { get; set; }
    Vector2 NewVelocity { get; set; }
    ActionState PlayerActionState { get; set; }
    InAirState PlayerInAirState { get; set; }
    LifeState PlayerLifeState { get; set; }
    WalkState PlayerWalkState { get; set; }
    WallClingState PlayerWallClingState { get; set; }
    int SpriteDirection { get; set; }
    TurnDirectionState TurnDirState { get; set; }
    float VerticalVelocity { get; set; }
    int WallJumpDirection { get; set; }
    float WallJumpDuration { get; }
    float WallJumpTime { get; set; }
    float WallslideGravityScaleMultiplier { get; }
    int HealthPoints { get; }
}