using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJump
{
    bool IsVelocityDirty { get; set; }
    int FlipGravityScale { get; set; }
    float VerticalVelocity { get; set; }
    float BaseGravityScale { get; }
    float JumpSpeed { get; }
    float MaxVelocityY { get; }
    float JumpingGravityScaleMultiplier { get; }
    bool IsGrounded { get; set; }
    float JumpTime { get; set; }
    InAirState PlayerInAirState { get; set; }
    float MinimumTimeBeforeAirJump { get; }
    bool HasAirJumped { get; set; }
    MovementStat MoveState { get; set; }
}
