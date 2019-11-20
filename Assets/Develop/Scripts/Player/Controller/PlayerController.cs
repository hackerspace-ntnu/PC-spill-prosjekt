using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private const float MINIMUM_TIME_BEFORE_AIR_JUMP = 0.1f;

    private PlayerState currentState;
    private PlayerState previousState;
    [SerializeField] private string currentStateName;
    [SerializeField] private string previousStateName;
    [SerializeField] private bool hasAirJumped = false;
    [SerializeField] private bool canUncrouch = false;

    public bool HasAirJumped { get => hasAirJumped; set => hasAirJumped = value; }
    public bool Grounded { get; set; } = false;
    public bool CanUncrouch { get => canUncrouch; set => canUncrouch = value; }
    public float JumpTime { get; set; }
    public Vector2 TargetVelocity { get; set; }

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
    }

    void Update()
    {
        HandleInput();
        currentState.Update();
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
