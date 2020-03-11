using System.Collections;
using System.Collections.Generic;
using GlobalEnums;
using UnityEngine;
using Spine.Unity;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private const float MINIMUM_TIME_BEFORE_AIR_JUMP = 0.1f;
    private const float MOVE_TRESHOLD = 0.01f;

    private PlayerState currentState;
    private PlayerState previousState;

    [SerializeField] [ReadOnly] private string currentStateName;
    [SerializeField] [ReadOnly] private string previousStateName;
    [SerializeField] [ReadOnly] private bool hasAirJumped = false;
    [SerializeField] [ReadOnly] private bool hasDashed = false;
    [SerializeField] [ReadOnly] private bool canUncrouch = false;
    [SerializeField] [ReadOnly] private bool glitchActive = false;
    [SerializeField] [ReadOnly] private int flipGravityScale = 1;
    [SerializeField] [ReadOnly] private WallTrigger wallTrigger = WallTrigger.NONE;
    [SerializeField] private Animator animator;
    [SerializeField] private SkeletonMecanim skeletonMecanism;
    [SerializeField] private int maxHealth;
    [SerializeField] private float invunerabilityTime;
    [SerializeField] private int currentHealth;
    [SerializeField] private float knockbackDuration;
    [SerializeField] private bool knockedBack;
    [SerializeField] private PhysicsMaterial2D bouncyMaterial;
    [SerializeField] private Collider2D bodyCollider;
    [SerializeField] private bool invunerable = false;

    public bool KnockedBack { get => knockedBack; set => knockedBack = value; }
    public bool Invunerable { get => invunerable; set => invunerable = value; }
    public float KnockBackDuration { get => knockbackDuration; set => knockbackDuration = value; }
    public float InvunerabilityTime { get => invunerabilityTime; set => invunerabilityTime = value; }
    public int CurrentHealth { get => currentHealth; set => currentHealth = value;}
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    [SerializeField] private SkeletonMecanim skeletonMecanim;

    public bool HasAirJumped { get => hasAirJumped; set => hasAirJumped = value; }
    public bool HasDashed { get => hasDashed; set => hasDashed = value; }
    public bool Grounded { get; set; } = false;
    public bool CanUncrouch { get => canUncrouch; set => canUncrouch = value; }
    public bool GlitchActive { get => glitchActive; set => glitchActive = value; }
    public int FlipGravityScale { get => flipGravityScale; set => flipGravityScale = value; }
    public WallTrigger WallTrigger { get => wallTrigger; set => wallTrigger = value; }
    public float JumpTime { get; set; }
    public float JumpButtonPressTime { get; set; }
    public float DashTime { get; set; }
    public Vector2 TargetVelocity { get; set; }
    public Animator Animator { get => animator; }
    public SkeletonMecanim SkeletonMecanism { get => skeletonMecanism; }
    public PhysicsMaterial2D BouncyMaterial { get => bouncyMaterial; }
    public Collider2D BodyCollider { get => bodyCollider; }
    public SkeletonMecanim SkeletonMecanim => skeletonMecanim;
    public Direction Dir { get; set; }

    public GameObject grapplingHookPrefab;
    public float grapplingSpeed;
    [Tooltip("In seconds.")]
    public float delayBetweenGrapplingAttempts;

    public void ChangeState(PlayerState newState)
    {
        previousState = currentState;
        previousStateName = currentState.Name;
        currentState.Exit();

        currentState = newState;
        currentStateName = newState.Name;
        newState.Enter();
    }

    public void ChangeNewState(PlayerState newState)
    {
        if (newState == currentState)
            return;

        ChangeState(newState);
    }

    void Start()
    {
        AirborneState.INSTANCE.Init(this);
        CrouchingState.INSTANCE.Init(this);
        DashingState.INSTANCE.Init(this);
        GlitchCrouchingState.INSTANCE.Init(this);
        GlitchDashingState.INSTANCE.Init(this);
        GlitchWallClingingState.INSTANCE.Init(this);
        GrapplingState.INSTANCE.Init(this);
        IdleState.INSTANCE.Init(this);
        JumpingState.INSTANCE.Init(this);
        KnockedBackState.INSTANCE.Init(this);
        WalkingState.INSTANCE.Init(this);
        WallClingingState.INSTANCE.Init(this);
        //If new states are added, remember to init them here.

        currentState = IdleState.INSTANCE;
        ChangeState(IdleState.INSTANCE);

        Dir = Direction.RIGHT;
    }

    void Update()
    {
        HandleInput();
        currentState.Update();
        if (!KnockedBack)
        {

        }
        float velocity = currentState.GetXVelocity();
        if (velocity < -MOVE_TRESHOLD)
        {
            Dir = Direction.LEFT;
            skeletonMecanim.skeleton.ScaleX = -1 * flipGravityScale;
        }
        else if (velocity > MOVE_TRESHOLD)
        {
            Dir = Direction.RIGHT;
            skeletonMecanim.skeleton.ScaleX = 1 * flipGravityScale;
        }
    }

    void FixedUpdate()
    {
        currentState.FixedUpdate();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentState.OnCollisionEnter2D(collision);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        currentState.OnTriggerEnter2D(collider);
    }

    void HandleInput()
    {
        if (Input.GetButtonDown("GlitchToggle"))
        {
            glitchActive = !glitchActive;
            currentState.ToggleGlitch();
        }

        if (Input.GetButtonDown("Dash") && !hasDashed)
            currentState.Dash();
        else if (Input.GetButtonDown("Jump"))
            currentState.Jump();
        else if (Input.GetButton("Crouch"))
            currentState.Crouch();
    }

    public PlayerState GetCurrentState()
    {
        return currentState;
    }

    public PlayerState GetPreviousState()
    {
        return previousState;
    }

    public void OnGrapplingHookHit()
    {
        currentState.OnGrapplingHookHit();
    }

    public void ChangeFlipGravity()
    {
        flipGravityScale *= -1;
        currentState.UpdateGravity();
    }
}
