using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Player state enums
public enum MovementStat
{
    STANDARD, // "IDLE" stance.
    JUMPING,
    AIR_JUMPING,
    DASHING,
    CROUCHING,
    WALL_CLINGING,
    WALL_JUMPING,
    GRAPPLING,
    DAMAGED,
}


public enum TurnDirectionState
{
    LEFT, // character is facing left
    RIGHT // character is facing right
}

public enum WalkState
{
    IDLE,
    WALKING
}

public enum InAirState
{
    ON_GROUND, // character is either on ground or on a wall
    UPWARDS, //character have momentum upwards
    HOVERING, // character reaches peak for a brief moment
    DOWNWARDS // character is falling downwards
}

public enum WallClingState
{
    DEFAULT,
    CLINGING,
    LEAVING_LEFT,
    LEAVING_RIGHT
}

public enum LifeState
{
    ALIVE,
    DAMAGED,
    DEAD
}


public enum GraphlingHookState
{
    DEFAULT,
    SHOOTING,
    HOOKED,
    PULLING,
    SWINGING
}
#endregion
