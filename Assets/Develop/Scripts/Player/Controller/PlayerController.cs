using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    protected const float MINIMUM_TIME_BEFORE_AIR_JUMP = 0.1f;

    private PlayerState currentState;
    private PlayerState previousState;
    [SerializeField] private string currentStateName;
    [SerializeField] private string previousStateName;
    [SerializeField] private bool hasAirJumped = false;

    private bool grounded = false;
    private float jumpTime;

    private Vector2 targetVelocity; // for setting velocity in FixedUpdate()

    public bool HasAirJumped { get => hasAirJumped; set => hasAirJumped = value; }
    public bool Grounded { get => grounded; set => grounded = value; }
    public float JumpTime { get => jumpTime; set => jumpTime = value; }
    public Vector2 TargetVelocity { get => targetVelocity; set => targetVelocity = value; }

    public void ChangeState(PlayerState newState)
    {
        previousState = currentState;
        previousStateName = currentState.Name;
        currentState.Exit();

        currentState = newState;
        currentStateName = newState.Name;
        newState.Enter();
    }

    void Awake()
    {
        currentState = IdleState.INSTANCE;
        currentStateName = currentState.Name;
        previousState = IdleState.INSTANCE;
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
        }
    }

    public PlayerState GetCurrentState() {
        return currentState;
    } 
    public PlayerState GetPreviousState() {
        return previousState;
    }
}
