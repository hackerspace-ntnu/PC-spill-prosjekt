using UnityEngine;

public class OnAirJumpState : AActionState
{
    // used to calculate input. Always set to 0 at entry and exit functions.
    private float lastInput;


    public float LastInput { get => lastInput; set => lastInput = value; }

    protected override AActionState CheckTriggers()
    {
        return StateMachine.OnNoActionState;
    }

    protected override void Start()
    {
        Body = GameObject.Find("View").GetComponent<Rigidbody2D>();
        StateName = "Air jumping.";
        IsActive = false;
    }

    protected override void Update()
    {
        if (IsActive)
        {
            // check if any other states can be transitioned into
            this.TargetTransitionState = CheckTriggers();

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
        PlayerModel.HasAirJumped = true;
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
        PlayerModel.VerticalVelocity = (PlayerModel.AirJumpSpeed - Body.velocity.y) * PlayerModel.FlipGravityScale;
        PlayerModel.IsVelocityDirty = true;
        PlayerModel.JumpTime = Time.time;
    }

    internal override StateTransition GetTransition()
    {
        if (this.TargetTransitionState == this || this.TargetTransitionState == null)
        {
            return new StateTransition(null, null, TransitionType.No);
        }
        else
        {
            return new StateTransition(this, TargetTransitionState, TransitionType.Sibling);
        }
    }
}