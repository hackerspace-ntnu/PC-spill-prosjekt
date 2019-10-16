using UnityEngine;

public class OnAirJumpState : BaseState
{
    // used to calculate input. Always set to 0 at entry and exit functions.
    private float lastInput;

    private Rigidbody2D rigidbody;

    public float LastInput { get => lastInput; set => lastInput = value; }
    public Rigidbody2D Rigidbody { get => rigidbody; set => rigidbody = value; }
    protected override BaseState TargetTransitionState { get => base.TargetTransitionState; set => base.TargetTransitionState = value; }

    protected override BaseState CheckTriggers<T>(Rigidbody2D body)
    {
        return StateMachine.OnNoActionState;
    }

    protected override void FixedUpdate()
    {
    }

    protected override void Start()
    {
        StateName = "Air jumping. Whoop whoop!.";
        IsActive = false;
        Rigidbody = GameObject.Find("View").GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        if (IsActive)
        {
            // check if any other states can be transitioned into
            this.TargetTransitionState = CheckTriggers<BaseState>(Rigidbody);

            // if no targeted states is found, handle horizontal movement input, other input (jump/dash etc) is handled in current actionstate.
            HandleJumpInput();
        }
    }

    internal override void EntryAction()
    {
        IsActive = true;
        LastInput = 0;
    }

    internal override void ExitAction()
    {
        this.TargetTransitionState = null;
        IsActive = false;
        LastInput = 0;
    }
    private void HandleJumpInput()
    {
        if (!PlayerModel.IsGrounded && PlayerModel.WallTrigger == 0)
        {
            AirJump();
        }
    }
    internal void AirJump()
    {
        PlayerModel.HasAirJumped = true;
        PlayerModel.VerticalVelocity = (PlayerModel.AirJumpSpeed - Rigidbody.velocity.y) * PlayerModel.FlipGravityScale;
        PlayerModel.IsVelocityDirty = true;
        PlayerModel.JumpTime = Time.time;
    }
}