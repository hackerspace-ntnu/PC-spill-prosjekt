using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Player enums
internal enum MovementStat
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


internal enum TurnDirectionState
{
    LEFT, // character is facing left
    RIGHT // character is facing right
}

internal enum WalkState
{
    IDLE,
    WALKING
}

internal enum InAirState
{
    ON_GROUND, // character is either on ground or on a wall
    UPWARDS, //character have momentum upwards
    HOVERING, // character reaches peak for a brief moment
    DOWNWARDS // character is falling downwards
}

internal enum WallClingState
{
    DEFAULT,
    CLINGING,
    LEAVING_LEFT,
    LEAVING_RIGHT
}

internal enum LifeState
{
    ALIVE,
    DAMAGED,
    DEAD
}

internal enum ActionState
{
    DEFAULT,
    ATTACKING,
    WITHDRAWING
}

internal enum GraphlingHookState
{
    DEFAULT,
    SHOOTING,
    HOOKED,
    PULLING,
    SWINGING
}
#endregion

public class PlayerModel : MonoBehaviour, IJump, IMove, IDash, IAction, IWallCling, ILife
{


    // States(in the form of enums) covering the various states the model can have. 
    internal MovementStat moveState;
    internal WalkState walkState;
    internal TurnDirectionState turnDirState;
    internal InAirState inAirState;
    internal WallClingState wallClingState;
    internal LifeState lifeState;
    internal ActionState actionState;
    internal GraphlingHookState graphHookState;

    // Input keycods. Can be changed in unity editor.
    [SerializeField]
    private KeyCode jumpKey = KeyCode.Space;
    [SerializeField]
    private KeyCode dashKey = KeyCode.LeftShift;
    [SerializeField]
    private KeyCode graphHookKey = KeyCode.F;

    [SerializeField]
    private int spriteDirection; // direction the character is facing, set in PlayerAnim

    [SerializeField]
    private float movementSpeed = 7;
    [SerializeField]
    private float jumpSpeed = 13.5f;
    [SerializeField]
    private float dashSpeed = 13;
    [SerializeField]
    private float baseGravityScale = 5;
    [SerializeField]
    private float maxVelocityY = 12;

    // Constants
    private readonly float minimumTimeBeforeAirJump = 0.1f;
    private readonly float horizontalInputRunningThreshold = 0.3f;
    private readonly float jumpingGravityScaleMultiplier = 0.8f;
    private readonly float wallslideGravityScaleMultiplier = 0.6f;
    private readonly float wallJumpDuration = 0.2f;

    [SerializeField]
    private float lastActionTime;
    [SerializeField]
    private float dashDuration = 0.2f;

    [SerializeField]
    private int wallJumpDirection;
    [SerializeField]
    private int flipGravityScale = 1;

    private MovementState state;
    private Rigidbody2D rigidBody;

    [SerializeField]
    private bool isVelocityDirty = false;
    private Vector2 newVelocity; // for setting velocity in FixedUpdate()
    [SerializeField]
    private float newGravityScale; // for setting velocity in FixedUpdate()
    [SerializeField]
    private float horizontalInput; // input from controller in x-axis

    [SerializeField]
    private bool isGrounded;
    [SerializeField]
    private bool hasAirJumped = false;
    [SerializeField]
    private bool hasDashed = false;
    [SerializeField]
    private float jumpTime;
    [SerializeField]
    private float wallJumpTime;
    [SerializeField]
    private float maxVelocityFix;

    [SerializeField]
    private int wallTrigger;

    #region Getters and setters
   

    public float MovementSpeed
    {
        get
        {
            return movementSpeed;
        }

        set
        {
            movementSpeed = value;
        }
    }

    public float JumpSpeed
    {
        get
        {
            return jumpSpeed;
        }

        set
        {
            jumpSpeed = value;
        }
    }

    public float DashSpeed
    {
        get
        {
            return dashSpeed;
        }

        set
        {
            dashSpeed = value;
        }
    }

    public float BaseGravityScale
    {
        get
        {
            return baseGravityScale;
        }

        set
        {
            baseGravityScale = value;
        }
    }

    public float MaxVelocityY
    {
        get
        {
            return maxVelocityY;
        }

        set
        {
            maxVelocityY = value;
        }
    }

    public float LastActionTime
    {
        get
        {
            return lastActionTime;
        }

        set
        {
            lastActionTime = value;
        }
    }

    public float DashDuration
    {
        get
        {
            return dashDuration;
        }

        set
        {
            dashDuration = value;
        }
    }


    public bool IsVelocityDirty
    {
        get
        {
            return isVelocityDirty;
        }

        set
        {
            isVelocityDirty = value;
        }
    }


    public float HorizontalInput
    {
        get
        {
            return HorizontalInput;
        }

        set
        {
            HorizontalInput = value;
        }
    }

    public KeyCode DashKey
    {
        get
        {
            return dashKey;
        }

        set
        {
            dashKey = value;
        }
    }

    public KeyCode GraphHookKey
    {
        get
        {
            return graphHookKey;
        }

        set
        {
            graphHookKey = value;
        }
    }


    public bool IsGrounded
    {
        get
        {
            return isGrounded;
        }

        set
        {
            isGrounded = value;
        }
    }

    public bool HasAirJumped
    {
        get
        {
            return hasAirJumped;
        }

        set
        {
            hasAirJumped = value;
        }
    }

    public bool HasDashed
    {
        get
        {
            return hasDashed;
        }

        set
        {
            hasDashed = value;
        }
    }

    public float JumpTime
    {
        get
        {
            return jumpTime;
        }

        set
        {
            jumpTime = value;
        }
    }

    public float WallJumpTime
    {
        get
        {
            return wallJumpTime;
        }

        set
        {
            wallJumpTime = value;
        }
    }

    public float MaxVelocityFix
    {
        get
        {
            return MaxVelocityFix1;
        }

        set
        {
            MaxVelocityFix1 = value;
        }
    }

    public int WallTrigger
    {
        get
        {
            return wallTrigger;
        }

        set
        {
            wallTrigger = value;
        }
    }


    public float HorizontalInputRunningThreshold
    {
        get
        {
            return horizontalInputRunningThreshold;
        }
    }

    public float JumpingGravityScaleMultiplier
    {
        get
        {
            return jumpingGravityScaleMultiplier;
        }
    }

    public float WallslideGravityScaleMultiplier
    {
        get
        {
            return wallslideGravityScaleMultiplier;
        }
    }

    public float WallJumpDuration
    {
        get
        {
            return wallJumpDuration;
        }
    }

    public float MaxVelocityFix1
    {
        get
        {
            return maxVelocityFix;
        }

        set
        {
            maxVelocityFix = value;
        }
    }

    public KeyCode JumpKey
    {
        get
        {
            return jumpKey;
        }

        set
        {
            jumpKey = value;
        }
    }

    public float MinimumTimeBeforeAirJump
    {
        get
        {
            return minimumTimeBeforeAirJump;
        }
    }

    public int FlipGravityScale
    {
        get
        {
            return flipGravityScale;
        }

        set
        {
            flipGravityScale = value;
        }
    }

    public int SpriteDirection
    {
        get
        {
            return spriteDirection;
        }

        set
        {
            spriteDirection = value;
        }
    }

    public Vector2 NewVelocity
    {
        get
        {
            return newVelocity;
        }

        set
        {
            newVelocity = value;
        }
    }

    public float HorizontalVelocity { get { return NewVelocity.x; } set { newVelocity.x = value; } }
    public float VerticalVelocity { get { return NewVelocity.y; } set { newVelocity.y = value; } }

    public float NewGravityScale
    {
        get
        {
            return newGravityScale;
        }

        set
        {
            newGravityScale = value;
        }
    }

    public int WallJumpDirection
    {
        get
        {
            return wallJumpDirection;
        }

        set
        {
            wallJumpDirection = value;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


// Interface used by JumpController to retrieve and update data in PlayerModel (This script). An instance of this interface is created
// in playercontroller and then assigned to a field in Jumpcontroller. 
public interface IJump
{
    bool IsVelocityDirty { get; set; } 
    int FlipGravityScale { get; set; }
    float VerticalVelocity { get; set; }
    float BaseGravityScale { get; }
    float JumpSpeed { get; }
    float MaxVelocityY { get; }
    float JumpingGravityScaleMultiplier { get; }
    bool IsGrounded { get; }
    float JumpTime { get; set; }
}
public interface IMove
{   
    float HorizontalVelocity { get; set; }
    Vector2 NewVelocity { get; set; }
    int FlipGravityScale { get; }
    bool IsGrounded { get; }
    float MovementSpeed { get; }
    float HorizontalInputRunningThreshold { get; }
    float MaxVelocityFix { get; set; }
}
public interface IDash
{
    
    float HorizontalVelocity { get; set; }
    int FlipGravityScale { get; }
    float NewGravityScale { get; set; }
    float BaseGravityScale { get; set; }
    bool HasDashed { get; set; }
    int SpriteDirection { get; set; }
    float LastActionTime { get; set; }
    Vector2 NewVelocity { get; set; }
    int WallTrigger { get; set; }
    int WallJumpDirection { get; set; }
    float DashDuration { get; set; }
    float DashSpeed { get; }
}

public interface ILife
{
    bool HasDashed { get; set; }
    bool HasAirJumped { set; get; }
}

public interface IGraphHook
{

}

public interface IWallCling
{
    Vector2 NewVelocity { set; get; }

    int WallJumpDirection { set; get; }

    float MovementSpeed { set; get; }
    float JumpSpeed { get; }

    int FlipGravityScale { set; get; }

    bool IsVelocityDirty { set; get; }

    float DashSpeed { set; get; }
}

public interface IAction
{

}
