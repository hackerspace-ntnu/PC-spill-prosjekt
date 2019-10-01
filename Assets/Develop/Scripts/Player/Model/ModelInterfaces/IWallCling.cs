﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWallCling
{
    Vector2 NewVelocity { set; get; }

    int WallJumpDirection { set; get; }

    float MovementSpeed { set; get; }
    float JumpSpeed { get; }

    int FlipGravityScale { set; get; }

    bool IsVelocityDirty { set; get; }

    float DashSpeed { set; get; }
    WallClingState PlayerWallClingState { get; set; }
    MovementStat MoveState { get; set; }
}
