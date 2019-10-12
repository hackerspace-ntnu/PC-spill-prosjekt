﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour , IStateMachine
{

    private  Object owner;


    PlayerModel model; // All variables related to player. Doesn't do any logic, just hold variables. Used ALOT.
    private Dictionary<int,BaseState> states = new Dictionary<int, BaseState>();

    // Level 1 states (most important)
    DeadState deadState;
    AliveState aliveState;

    // Level 2 states  (important)
    UpwardsInAirState upwardsInAirState;
    HoveringInAirState hoveringInAirState;
    FallingInAirState fallingInAirState;
    OnWallState onWallState;
    OnGroundState onGroundState;

    // level 3 states (less important)
    OnDashState onDashState;
    OnJumpState onJumpState;
    OnAirJumpState onAirJumpState;
    OnNoActionState onNoActionState;
    OnWallClingState onWallClingState;
    OnWallJump onWallJump;

    private bool dictModifiedUpdate;
    private bool dictModifiedFixedUpdate;


    // Inputs
    private float horizontalInput; // a value between -1 to +1, in float value (0.0043) for example. -1 is to the left. + 1 to the right.
    private float verticalInput; // a value between -1 to +1, in float value (0.0043) for example. -1 is down. + 1 is up.
    private bool dashInput; // is key pressed?
    private bool jumpInput; // is key pressed?
    private bool graphHookInput; // is key pressed?


    public virtual Object Owner { get => owner; set => owner = value; }
    public Dictionary<int,BaseState> States { get => states; set => states = value; }
    public DeadState DeadState { get => deadState; private set => deadState = value; }
    public AliveState AliveState { get => aliveState; private set => aliveState = value; }
    public UpwardsInAirState UpwardsInAirState { get => upwardsInAirState; set => upwardsInAirState = value; }
    public HoveringInAirState HoveringInAirState { get => hoveringInAirState; set => hoveringInAirState = value; }
    public FallingInAirState FallingInAirState { get => fallingInAirState; set => fallingInAirState = value; }
    public OnWallState OnWallState { get => onWallState; set => onWallState = value; }
    public OnGroundState OnGroundState { get => onGroundState; set => onGroundState = value; }
    public OnDashState OnDashState { get => onDashState; set => onDashState = value; }
    public OnJumpState OnJumpState { get => onJumpState; set => onJumpState = value; }
    public OnAirJumpState OnAirJumpState { get => onAirJumpState; set => onAirJumpState = value; }
    public OnNoActionState OnNoActionState { get => onNoActionState; set => onNoActionState = value; }
    public bool DictModifiedUpdate { get => dictModifiedUpdate; set => dictModifiedUpdate = value; }
    public bool DictModifiedFixedUpdate { get => dictModifiedFixedUpdate; set => dictModifiedFixedUpdate = value; }
    public PlayerModel Model { get => model; set => model = value; }
    public float HorizontalInput { get => horizontalInput; set => horizontalInput = value; }
    public float VerticalInput { get => verticalInput; set => verticalInput = value; }
    public bool DashInput { get => dashInput; set => dashInput = value; }
    public bool JumpInput { get => jumpInput; set => jumpInput = value; }
    public bool GraphHookInput { get => graphHookInput; set => graphHookInput = value; }
    public OnWallClingState OnWallClingState { get => onWallClingState; set => onWallClingState = value; }
    public OnWallJump OnWallJump { get => onWallJump; set => onWallJump = value; }



    // Start is called before the first frame update
    public void Start()
    {
        InitializeStandardInputValues();
        InitializeStates();
        InitilizeInterfacesForStates();
        PrepareInitialStates();
    }

    // Update is called once per frame
    public void Update()
    {
        RecordInputs();
        if (!DictModifiedUpdate)
        {
            ProcessStateTransitions();
        }
    }

    private void InitializeStandardInputValues()
    {
        HorizontalInput = 0;
        VerticalInput = 0;
        DashInput = false;
        JumpInput = false; ;
        GraphHookInput = false;
    }
    private void InitializeStates()
    {
        Model = GameObject.Find("Models").GetComponent<PlayerModel>();

        // lvl 1
        DeadState = GetComponent<DeadState>();
        AliveState = GetComponent<AliveState>();
        // lvl 2
        UpwardsInAirState = GetComponent<UpwardsInAirState>();
        HoveringInAirState = GetComponent<HoveringInAirState>();
        FallingInAirState = GetComponent<FallingInAirState>();
        OnGroundState = GetComponent<OnGroundState>();
        OnWallState = GetComponent<OnWallState>();
        //lvl 3
        OnDashState = GetComponent<OnDashState>();
        OnJumpState = GetComponent<OnJumpState>();
        OnAirJumpState = GetComponent<OnAirJumpState>();
        OnNoActionState = GetComponent<OnNoActionState>();
        OnWallClingState = GetComponent<OnWallClingState>();
        OnWallJump = GetComponent<OnWallJump>();
    }

    private void PrepareInitialStates()
    {
        States.Add(1, AliveState);
        States.Add(2, HoveringInAirState);
        States.Add(3, OnNoActionState);

        AliveState.IsActive = true;
        HoveringInAirState.IsActive = true;
        OnNoActionState.IsActive = true;

        DictModifiedUpdate = false;
        DictModifiedFixedUpdate = false;
    }


    private void InitilizeInterfacesForStates()
    {
        // lvl 1
        DeadState.PlayerModel = (IPlayerModel)model;
        DeadState.StateMachine = (IStateMachine)this;
        AliveState.PlayerModel = (IPlayerModel)model;
        AliveState.StateMachine = (IStateMachine)this;
        // lvl 2
        UpwardsInAirState.PlayerModel = (IPlayerModel)model;
        UpwardsInAirState.StateMachine = (IStateMachine)this;
        HoveringInAirState.PlayerModel = (IPlayerModel)model;
        HoveringInAirState.StateMachine = (IStateMachine)this;
        FallingInAirState.PlayerModel = (IPlayerModel)model;
        FallingInAirState.StateMachine = (IStateMachine)this;
        OnGroundState.PlayerModel = (IPlayerModel)model;
        OnGroundState.StateMachine = (IStateMachine)this;
        OnWallState.PlayerModel = (IPlayerModel)model;
        OnWallState.StateMachine = (IStateMachine)this;


        //lvl 3
        OnDashState.PlayerModel = (IPlayerModel)model;
        OnDashState.StateMachine = (IStateMachine)this;
        OnJumpState.PlayerModel = (IPlayerModel)model;
        OnJumpState.StateMachine = (IStateMachine)this;
        OnAirJumpState.PlayerModel = (IPlayerModel)model;
        OnAirJumpState.StateMachine = (IStateMachine)this;
        OnNoActionState.PlayerModel = (IPlayerModel)model;
        OnNoActionState.StateMachine = (IStateMachine)this;
        OnWallClingState.PlayerModel = (IPlayerModel)model;
        OnWallClingState.StateMachine = (IStateMachine)this;
        OnWallJump.PlayerModel = (IPlayerModel)model;
        OnWallJump.StateMachine = (IStateMachine)this;
    }
        private void RecordInputs()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        dashInput = Input.GetKey(Model.DashKey);
        jumpInput = Input.GetKeyDown(Model.JumpKey);
        graphHookInput = Input.GetKey(Model.GraphHookKey);
    }


    public void ProcessStateTransitions()
    {
        if(States.Count == 0)
        {
            InitializeStates();
        }
        DictModifiedUpdate = ProcessStateTransitionsOnce();
        DictModifiedUpdate = false;
    }

    public bool ProcessStateTransitionsOnce()
    {
        BaseState healthState = null;
        BaseState movementState = null;
        BaseState actionState = null;
        bool stateModified = false;
        foreach (KeyValuePair<int, BaseState> entry in States)
        {
            // Handle level 1 state changes (dead/alive)
            if(entry.Key == 1)
            {
                Transition<BaseState> trans = entry.Value.GetTransition();
                if (trans.Target != entry.Value && trans.Target != null)
                {
                    entry.Value.ExitAction();
                    healthState = trans.Target;
                    healthState.EntryAction();
                    stateModified = true;
                }
                else
                {
                    healthState = null;
                }
            }
            // Handle level 2 state changes (Movement - on ground, in air, on wall etc.)
            else if(entry.Key == 2)
            {
                Transition<BaseState> trans = entry.Value.GetTransition();
                if (trans.Target != entry.Value && trans.Target != null)
                {
                    entry.Value.ExitAction();
                    movementState = trans.Target;
                    movementState.EntryAction();
                    // entry.Value = trans.Target; - doesnt work, is readonly.
                    stateModified = true;
                }
                else
                {
                    movementState = null;
                }
            }
            // Handle level 3 state changes - actions (Jump, dash, hook etc.)
            else if(entry.Key == 3)
            {
                Transition<BaseState> trans = entry.Value.GetTransition();
                if (trans.Target != entry.Value && trans.Target != null)
                {
                    entry.Value.ExitAction();
                    actionState = trans.Target;
                    actionState.EntryAction();
                    // entry.Value = trans.Target; - doesn't work, is read-only.
                    stateModified = true;
                }
                else
                {
                    actionState = null;
                }
            }
            else
            {
                break;
            }

        }
        UpdateStates(healthState, movementState, actionState);
        return stateModified;

    }
    public void UpdateStates(BaseState healthState, BaseState movementState, BaseState actionState)
    {

        if(healthState != null)
        {
            States[1] = healthState;
            DictModifiedUpdate = true;
        }
        if(movementState != null)
        {
            DictModifiedUpdate = true;
            States[2] = movementState;
        }
        if(actionState != null)
        {
            DictModifiedUpdate = true;
            States[3] = actionState;
        }
    }
}
