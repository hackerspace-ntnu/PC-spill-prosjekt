using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An interface used by HorizontalDirectionController.cs to access data from playermodel.
public interface IMove
{
    WalkState PlayerWalkState { get; set; }
    float HorizontalVelocity { get; set; }
    Vector2 NewVelocity { get; set; }
    int FlipGravityScale { get; }
    bool IsGrounded { get; }
    float MovementSpeed { get; }
    float HorizontalInputRunningThreshold { get; }
    float MaxVelocityFix { get; set; }
    MovementStat MoveState { get; set; }
}
