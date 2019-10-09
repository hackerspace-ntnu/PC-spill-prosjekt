using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDash
{

    float HorizontalVelocity { get; set; }
    int FlipGravityScale { get; }
    float NewGravityScale { get; set; }
    float BaseGravityScale { get; set; }
    bool HasDashed { get; set; }
    int SpriteDirection { get; set; }
    float LastDashTime { get; set; }
    Vector2 NewVelocity { get; set; }
    int WallTrigger { get; set; }
    int WallJumpDirection { get; set; }
    float DashDuration { get; set; }
    float DashSpeed { get; }
}
