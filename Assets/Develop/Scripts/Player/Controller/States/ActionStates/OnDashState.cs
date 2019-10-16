using UnityEngine;

public class OnDashState : BaseState
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
        StateName = "Dashing!";
        IsActive = false;
        Rigidbody = GameObject.Find("View").GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        if (IsActive)
        {
            // check if any other states can be transitioned into
            this.TargetTransitionState = CheckTriggers<OnDashState>(Rigidbody);
            Dash();
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


    internal void Dash()
    {
        PlayerModel.NewVelocity = new Vector2(PlayerModel.SpriteDirection * PlayerModel.DashSpeed * PlayerModel.FlipGravityScale, -Rigidbody.velocity.y);
        PlayerModel.NewGravityScale = 0;
        PlayerModel.LastDashTime = Time.time;
        PlayerModel.HasDashed = true;
    }
}