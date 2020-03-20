using System;
using System.Collections;
using System.Collections.Generic;
using GlobalEnums;
using UnityEngine;
using Spine.Unity;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private const float MINIMUM_TIME_BEFORE_AIR_JUMP = 0.1f;
    private const float CHANGE_FACING_DIRECTION_THRESHOLD = 0.01f;

    private PlayerState currentState;
    private PlayerState previousState;

#pragma warning disable 649 // disable unassigned field warning
    [SerializeField] [ReadOnly] private string currentStateName;
    [SerializeField] [ReadOnly] private string previousStateName;
    [SerializeField] [ReadOnly] private bool hasAirJumped = false;
    [SerializeField] [ReadOnly] private bool hasDashed = false;
    [SerializeField] [ReadOnly] private bool canUncrouch = true;        // is also set true in enter() because Unity hates me
    [SerializeField] [ReadOnly] private bool glitchActive = false;
    [SerializeField] [ReadOnly] private bool gravityFlipEnabled = true; // is also set true in enter() because Unity hates me
    [SerializeField] [ReadOnly] private bool canUnglitch = true;        // is not set true in enter() because Unity doesn't hate Aleksander
    [SerializeField] [ReadOnly] private int flipGravityScale = 1;
    [SerializeField] [ReadOnly] private WallTrigger wallTrigger = WallTrigger.NONE;
    [SerializeField] private Animator animator;
    [SerializeField] private SkeletonMecanim skeletonMecanim;
#pragma warning restore 649

    public bool HasAirJumped { get => hasAirJumped; set => hasAirJumped = value; }
    public bool HasDashed { get => hasDashed; set => hasDashed = value; }
    public bool Grounded { get; set; } = false;
    public bool CanUncrouch { get => canUncrouch; set => canUncrouch = value; }
    public bool GlitchActive { get => glitchActive; set => glitchActive = value; }
    public bool GravityFlipEnabled { get => gravityFlipEnabled; set => gravityFlipEnabled = value; }
    public int FlipGravityScale { get => flipGravityScale; set => flipGravityScale = value; }
    public WallTrigger WallTrigger { get => wallTrigger; set => wallTrigger = value; }
    public float JumpTime { get; set; }
    public float JumpButtonPressTime { get; set; }
    public float DashTime { get; set; }
    public Vector2 TargetVelocity { get; set; }
    public Animator Animator => animator;
    public SkeletonMecanim SkeletonMecanim => skeletonMecanim;
    public Direction FacingDirection { get; set; }
    public bool CanUnglitch { get => canUnglitch; set => canUnglitch = value; }

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
        KnockbackState.INSTANCE.Init(this);
        WalkingState.INSTANCE.Init(this);
        WallClingingState.INSTANCE.Init(this);
        //If new states are added, remember to init them here.

        currentState = IdleState.INSTANCE;
        ChangeState(IdleState.INSTANCE);

        FacingDirection = Direction.RIGHT;

        // Because Unity fucking hates me, and I'm too tired to find out why
        canUncrouch = true;
        gravityFlipEnabled = true;
    }

    void Update()
    {
        HandleInput();
        currentState.Update();

        float xVelocity = currentState.GetXVelocity();
        // Change direction character is facing only when moving faster than threshold
        if (Mathf.Abs(xVelocity) > CHANGE_FACING_DIRECTION_THRESHOLD)
        {
            int movingDirection = Math.Sign(xVelocity) * flipGravityScale;
            FacingDirection = DirectionUtils.Parse(movingDirection);
        }
    }

    void FixedUpdate()
    {
        currentState.FixedUpdate();
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
        if (gravityFlipEnabled)
        {
            flipGravityScale *= -1;
            currentState.UpdateGravity();
        }
    }
}
