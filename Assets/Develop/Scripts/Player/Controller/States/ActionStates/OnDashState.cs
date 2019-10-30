using UnityEngine;

public class OnDashState : AActionState
{
    // used to calculate input. Always set to 0 at entry and exit functions.
    private float lastInput;


    public float LastInput { get => lastInput; set => lastInput = value; }

    protected override AActionState CheckTriggers()
    {
        if(PlayerModel.WallTrigger != 0)
        {
            return StateMachine.OnWallClingState;
        }
        else if(Time.time >= PlayerModel.LastDashTime + PlayerModel.DashDuration)
        {
            return StateMachine.OnNoActionState;
        }
        else
        {
            return null;
        }
    }


    protected override void Start()
    {
        Body = GameObject.Find("View").GetComponent<Rigidbody2D>();
        StateName = "Dashing!";
        base.Start();
    }

    protected override void Update()
    {
        if (IsActive)
        {
            // check if any other states can be transitioned into
            this.TargetTransitionState = CheckTriggers();

            if(!PlayerModel.HasDashed && Time.time >= PlayerModel.LastDashTime + PlayerModel.DashDuration && StateMachine.DashInput)
            {
                Dash();
            }
        }
    }

    internal override void EntryAction()
    {
        IsActive = true;
        LastInput = 0;
    }

    internal override void ExitAction()
    {
        PlayerModel.NewGravityScale = PlayerModel.BaseGravityScale * PlayerModel.FlipGravityScale; // fix gravity back to normal!
        this.TargetTransitionState = null;
        IsActive = false;
        LastInput = 0;
    }


    private void Dash()
    {
        PlayerModel.NewVelocity = new Vector2(PlayerModel.SpriteDirection * PlayerModel.DashSpeed * PlayerModel.FlipGravityScale, 0);
        PlayerModel.NewGravityScale = 0;
        PlayerModel.LastDashTime = Time.time;
        PlayerModel.HasDashed = true;
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