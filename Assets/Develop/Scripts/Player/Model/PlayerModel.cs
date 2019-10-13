using UnityEngine;

public class PlayerModel : MonoBehaviour, IJump, IMove, IDash, IAction, IWallCling, ILife, IPlayerModel
{
    // States(in the form of enums) covering the various states the model can have.
    // The various states can be found in the file "PlayerStateEnums.cs"
    [Header("State Settings")]
    [SerializeField]
    private MovementStat moveState;

    [SerializeField]
    private WalkState playerWalkState;

    [SerializeField]
    private TurnDirectionState turnDirState;

    [SerializeField]
    private InAirState playerInAirState;

    [SerializeField]
    private WallClingState playerWallClingState;

    [SerializeField]
    private LifeState playerLifeState;

    [SerializeField]
    private ActionState playerActionState;

    [SerializeField]
    private GraphlingHookState graphHookState;

    [Tooltip("Health points of the character.")]
    [RangeAttribute(0, 100)]
    [SerializeField]
    private int healthPoints = 100;

    // Input keycodes. Can be changed in unity editor.
    [Header("Input Settings")]
    [Tooltip("How much input 'force' is required before the player start movement along the horizontal axis.")]
    private readonly float horizontalInputRunningThreshold = 0.3f;

    [Tooltip("How long time character must wait before it can air jump.")]
    private readonly float minimumTimeBeforeAirJump = 0.1f;

    [SerializeField]
    private KeyCode jumpKey = KeyCode.Space;

    [SerializeField]
    private KeyCode dashKey = KeyCode.LeftShift;

    [SerializeField]
    private KeyCode graphHookKey = KeyCode.F;

    [Header("Animation related Settings")]
    [SerializeField]
    [Tooltip("direction the character is facing, set in PlayerAnim")]
    private int spriteDirection;

    [SerializeField]
    private int wallJumpDirection;

    [Header("Physics related Settings")]
    [SerializeField]
    [Tooltip("The maximum movement speed of the character.")]
    private float movementSpeed = 7;

    [SerializeField]
    [Tooltip("The initial jump speed of the character when grounded.")]
    private float groundJumpSpeed = 13.5f;

    [SerializeField]
    [Tooltip("The initial jump speed of the character when airborne.")]
    private float airJumpSpeed = 10.5f;

    [SerializeField]
    [Tooltip("The initial dashSpeed of the character.")]
    private float dashSpeed = 13;

    [SerializeField]
    [Tooltip("The ")]
    private float baseGravityScale = 5;

    [SerializeField]
    [Tooltip("The maximum Y velocity (up and down speed) the character can have.")]
    private float maxVelocityY = 12;

    [SerializeField]
    private float jumpTime;

    [SerializeField]
    private float wallJumpTime;

    [SerializeField]
    private float maxVelocityFix;

    [SerializeField]
    private Vector2 newVelocity; // for setting velocity in FixedUpdate()

    [SerializeField]
    private float newGravityScale; // for setting velocity in FixedUpdate()

    // Constants

    [Tooltip(" TODO ")]
    private readonly float jumpingGravityScaleMultiplier = 0.8f;

    [Tooltip("")]
    private readonly float wallslideGravityScaleMultiplier = 0.6f;

    [Tooltip("How long a wall jump last.")]
    private readonly float wallJumpDuration = 0.2f;

    [SerializeField]
    private float lastDashTime;

    [SerializeField]
    private float dashDuration = 0.2f;

    [RangeAttribute(-1, 1)]
    [SerializeField]
    [Tooltip("What 'way' gravity works. -1 = the player is falling upwards. 1 is normal gravity and 0 is no gravity.")]
    private int flipGravityScale = 1;

    private MovementState state;
    private Rigidbody2D rigidBody;

    [SerializeField]
    private bool isGrounded;

    [SerializeField]
    private bool hasAirJumped = false;

    [SerializeField]
    private bool hasDashed = false;

    [Tooltip("Have New Velocity been modified?")]
    [SerializeField]
    private bool isVelocityDirty = false;

    [RangeAttribute(-1, 1)]
    [Tooltip("Which sensor is detecting a wall? -1 == left sensor, 1 is right sensor, and 0 is none. ")]
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

    public float GroundJumpSpeed
    {
        get
        {
            return groundJumpSpeed;
        }

        set
        {
            groundJumpSpeed = value;
        }
    }

    public float AirJumpSpeed
    {
        get
        {
            return groundJumpSpeed;
        }

        set
        {
            groundJumpSpeed = value;
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

    public float LastDashTime
    {
        get
        {
            return lastDashTime;
        }

        set
        {
            lastDashTime = value;
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
            return maxVelocityFix;
        }

        set
        {
            maxVelocityFix = value;
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

    public WalkState PlayerWalkState { get => playerWalkState; set => playerWalkState = value; }
    public MovementStat MoveState { get => moveState; set => moveState = value; }
    public TurnDirectionState TurnDirState { get => turnDirState; set => turnDirState = value; }
    public InAirState PlayerInAirState { get => playerInAirState; set => playerInAirState = value; }
    public WallClingState PlayerWallClingState { get => playerWallClingState; set => playerWallClingState = value; }
    public LifeState PlayerLifeState { get => playerLifeState; set => playerLifeState = value; }
    public ActionState PlayerActionState { get => playerActionState; set => playerActionState = value; }
    public GraphlingHookState GraphHookState { get => graphHookState; set => graphHookState = value; }
    public int HealthPoints { get => healthPoints; set => healthPoints = value; }

    #endregion Getters and setters
}