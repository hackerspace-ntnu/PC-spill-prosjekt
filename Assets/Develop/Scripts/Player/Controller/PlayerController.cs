using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private const float MINIMUM_TIME_BEFORE_AIR_JUMP = 0.1f;

    private PlayerState currentState;
    private PlayerState previousState;

    private DIRECTION dir;

    [SerializeField] private string currentStateName;
    [SerializeField] private string previousStateName;
    [SerializeField] private bool hasAirJumped = false;
    [SerializeField] private bool hasDashed = false;
    [SerializeField] private bool canUncrouch = false;
    [SerializeField] private int wallTrigger = 0;
    [SerializeField] private Animator animator;
    [SerializeField] private SkeletonMecanim skeletonMecanism;
    [SerializeField] private int maxHealth;
    [SerializeField] private float invunerabilityTime;


    private int currentHealth;
    private bool invunerable = false;

    public bool Invunerable { get => invunerable; set => invunerable = value; }

    public float InvunerabilityTime { get => invunerabilityTime; set => invunerabilityTime = value; }
    public int CurrentHealth { get => currentHealth; set => currentHealth = value;}
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public bool HasAirJumped { get => hasAirJumped; set => hasAirJumped = value; }
    public bool HasDashed { get => hasDashed; set => hasDashed = value; }
    public bool Grounded { get; set; } = false;
    public bool CanUncrouch { get => canUncrouch; set => canUncrouch = value; }
    public int WallTrigger { get => wallTrigger; set => wallTrigger = value; }
    public float JumpTime { get; set; }
    public Vector2 TargetVelocity { get; set; }
    public Animator Animator { get => animator; }
    public SkeletonMecanim SkeletonMecanism { get => skeletonMecanism; }
    public DIRECTION Dir { get => dir; set => dir = value; }

    public void ChangeState(PlayerState newState)
    {
        previousState = currentState;
        previousStateName = currentState.Name;
        currentState.Exit();

        currentState = newState;
        currentStateName = newState.Name;
        newState.Enter();
    }

    void Start()
    {
        AirborneState.INSTANCE.Init(this);
        CrouchingState.INSTANCE.Init(this);
        DashingState.INSTANCE.Init(this);
        GrapplingState.INSTANCE.Init(this);
        IdleState.INSTANCE.Init(this);
        JumpingState.INSTANCE.Init(this);
        KnockbackState.INSTANCE.Init(this);
        WalkingState.INSTANCE.Init(this);
        WallClingingState.INSTANCE.Init(this);
        //If new states are added, remember to init them here.

        currentState = IdleState.INSTANCE;
        ChangeState(IdleState.INSTANCE);

        Dir = DIRECTION.RIGHT;
    }

    void Update()
    {
        HandleInput();
        currentState.Update();

        float velocity = currentState.GetXVelocity();
        if(velocity < -0.01f) {
            Dir = DIRECTION.LEFT;
            skeletonMecanism.skeleton.ScaleX = -1;
        } else if (velocity > 0.01f) {
            Dir = DIRECTION.RIGHT;
            skeletonMecanism.skeleton.ScaleX = 1;
        }
    }

    void FixedUpdate()
    {
        currentState.FixedUpdate();
    }

    void HandleInput() {

        if(Input.GetButtonDown("Jump") && !hasAirJumped && Time.time >= JumpTime + MINIMUM_TIME_BEFORE_AIR_JUMP) {
            currentState.Jump();
        } else if(Input.GetButtonDown("Crouch")) {
            currentState.Crouch();
        }
    }

    public PlayerState GetCurrentState() {
        return currentState;
    } 
    public PlayerState GetPreviousState() {
        return previousState;
    }
}
