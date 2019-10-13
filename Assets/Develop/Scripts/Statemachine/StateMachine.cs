using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class StateMachine : MonoBehaviour, IStateMachine
{
    private Object owner;

    private PlayerModel model; // All variables related to player. Doesn't do any logic, just hold variables. Used ALOT.

    private Dictionary<int, BaseState> states = new Dictionary<int, BaseState>();

    [Tooltip("Print states on each frame if change in states is detected.")]
    [SerializeField]
    private bool printStates = true;

    private List<BaseState> statesLastFrame = new List<BaseState>();

    // Level 1 states (most important)
    private DeadState deadState;

    private AliveState aliveState;

    // Level 2 states  (important)
    private UpwardsInAirState upwardsInAirState;

    private HoveringInAirState hoveringInAirState;
    private FallingInAirState fallingInAirState;
    private OnWallState onWallState;
    private OnGroundState onGroundState;

    // level 3 states (less important)
    private OnDashState onDashState;

    private OnJumpState onJumpState;
    private OnAirJumpState onAirJumpState;
    private OnNoActionState onNoActionState;
    private OnWallClingState onWallClingState;
    private OnWallJump onWallJump;

    private bool statesModified;
    private bool haveSetActiveInInitialStates = false;

    // Inputs
    private float horizontalInput; // a value between -1 to +1, in float value (0.0043) for example. -1 is to the left. + 1 to the right.

    private float verticalInput; // a value between -1 to +1, in float value (0.0043) for example. -1 is down. + 1 is up.
    private bool dashInput; // is key pressed?
    private bool jumpInput; // is key pressed?
    private bool graphHookInput; // is key pressed?

    public virtual Object Owner { get => owner; set => owner = value; }
    public Dictionary<int, BaseState> States { get => states; set => states = value; }
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

    public bool StatesModified
    {
        get => statesModified;
        set => statesModified = value;
    }

    public bool HaveSetActiveInInitialStates { get => haveSetActiveInInitialStates; set => haveSetActiveInInitialStates = value; }
    public PlayerModel Model { get => model; set => model = value; }
    public float HorizontalInput { get => horizontalInput; set => horizontalInput = value; }
    public float VerticalInput { get => verticalInput; set => verticalInput = value; }
    public bool DashInput { get => dashInput; set => dashInput = value; }
    public bool JumpInput { get => jumpInput; set => jumpInput = value; }
    public bool GraphHookInput { get => graphHookInput; set => graphHookInput = value; }
    public OnWallClingState OnWallClingState { get => onWallClingState; set => onWallClingState = value; }
    public OnWallJump OnWallJump { get => onWallJump; set => onWallJump = value; }
    public List<BaseState> StatesLastFrame { get => statesLastFrame; set => statesLastFrame = value; }

    // Start is called before the first frame update
    public void Start()
    {
        InitializeStandardInputValues();
        InitializeStates();
        InitializeInterfacesForStates();
        PrepareInitialStates();
    }

    #region Start methods called

    private void InitializeStandardInputValues()
    {
        HorizontalInput = 0;
        VerticalInput = 0;
        DashInput = false;
        JumpInput = false; ;
        GraphHookInput = false;
    }

    private void ActivateInitialStates()
    {
        AliveState.IsActive = true;
        HoveringInAirState.IsActive = true;
        OnNoActionState.IsActive = true;
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
        //StatesLastFrame = new List<BaseState>();
        States.Add(1, AliveState);
        StatesLastFrame.Add(AliveState);
        States.Add(2, HoveringInAirState);
        StatesLastFrame.Add(HoveringInAirState);
        States.Add(3, OnNoActionState);
        StatesLastFrame.Add(OnNoActionState);

        AliveState.IsActive = true;
        HoveringInAirState.IsActive = true;
        OnNoActionState.IsActive = true;
    }

    private void InitializeInterfacesForStates()
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

    #endregion Start methods called

    // Update is called once per frame
    public void Update()
    {
        if (!HaveSetActiveInInitialStates)
        {
            ActivateInitialStates();
            HaveSetActiveInInitialStates = true;
        }
        RecordInputs(); // always record inputs each frame!
        ProcessStateTransitions();
    }

    public void LateUpdate()
    {
        if (printStates && StatesModified)
        {
            DebugPrintStates();
        }
    }

    public void ProcessStateTransitions()
    {
        // we should always have three states - one for life/hp, one for position, and one for actions.
        if (States.Count < 3)
        {
            InitializeStates();
        }
        // if one or more transitions to different states is found, this prevent the update function
        ProcessStateTransitionsOnce();
    }

    private void ProcessStateTransitionsOnce()
    {
        BaseState healthState = null;
        BaseState movementState = null;
        BaseState actionState = null;
        foreach (KeyValuePair<int, BaseState> entry in States)
        {
            // Handle level 1 state changes (dead/alive)
            if (entry.Key == 1)
            {
                Transition<BaseState> trans = entry.Value.GetTransition();
                if (trans.Target != entry.Value && trans.Target != null)
                {
                    entry.Value.ExitAction();
                    healthState = trans.Target;
                    healthState.EntryAction();
                    healthState.IsActive = true;
                }
                else
                {
                    healthState = null;
                }
            }
            // Handle level 2 state changes (Movement - on ground, in air, on wall etc.)
            else if (entry.Key == 2)
            {
                Transition<BaseState> trans = entry.Value.GetTransition();
                if (trans.Target != entry.Value && trans.Target != null)
                {
                    entry.Value.ExitAction();
                    movementState = trans.Target;
                    movementState.EntryAction();
                    movementState.IsActive = true;
                }
                else
                {
                    movementState = null;
                }
            }
            // Handle level 3 state changes - actions (Jump, dash, hook etc.)
            else if (entry.Key == 3)
            {
                Transition<BaseState> trans = entry.Value.GetTransition();
                if (trans.Target != entry.Value && trans.Target != null)
                {
                    entry.Value.ExitAction();
                    actionState = trans.Target;
                    actionState.EntryAction();
                    actionState.IsActive = true;
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
    }

    public void UpdateStates(BaseState healthState, BaseState movementState, BaseState actionState)
    {
        // allways check healthstate first!
        if (healthState != null)
        {
            States[1] = healthState;
            StatesLastFrame[0] = healthState;
            StatesModified = true;
        }
        // movement and actionstates are less important and gets checked in same check.
        else if (movementState != null || actionState != null)
        {
            if (movementState != null)
            {
                States[2] = movementState;
                StatesLastFrame[1] = movementState;
                StatesModified = true;
            }
            if (actionState != null)
            {
                States[3] = actionState;
                StatesLastFrame[2] = actionState;
                StatesModified = true;
            }
        }
        else
        {
            StatesModified = false;
        }
    }

    private void DebugPrintStates()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("state of player last frame: ");
        foreach (BaseState item in StatesLastFrame)
        {
            sb.AppendLine(item.StateName);
        }
        Debug.Log(sb.ToString());
    }
}