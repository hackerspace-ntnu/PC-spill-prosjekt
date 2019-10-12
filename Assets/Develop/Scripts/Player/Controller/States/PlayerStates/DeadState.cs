using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : BaseState
{
    private Rigidbody2D rigidbody;
    public Rigidbody2D Rigidbody { get => rigidbody; set => rigidbody = value; }
    protected override bool CanTransitionTo { get => base.CanTransitionTo; set => base.CanTransitionTo = value; }
    protected override bool CanTransitionFrom { get => base.CanTransitionFrom; set => base.CanTransitionFrom = value; }
    protected override BaseState TargetTransitionState
    {
        get => base.TargetTransitionState;
        set
        {
            if (value == null)
            {
                base.TargetTransitionState = value;
            }
            else if (value.GetType() == typeof(DeadState) || value.GetType() == typeof(AliveState))
            {
                base.TargetTransitionState = value;
            }
            else
            {
                return;
            }
        }
    }

    internal override void EntryAction()
    {
    }

    internal override void ExitAction()
    {
        this.TargetTransitionState = null;
        IsActive = false;
    }

    protected override void FixedUpdate()
    {
    }

    protected override void Start()
    {
        StateName = " Player is dead ";
        Rigidbody = GameObject.Find("View").GetComponent<Rigidbody2D>();
        base.Start();
    }

    protected override void Update()
    {
    }


    protected override BaseState CheckTriggers<T>(Rigidbody2D body)
    {
        BaseState temp = null; // defaults to null, but have to be assigned because compiler is annoying me.
        if (typeof(T) == typeof(DeadState) || typeof(T) == typeof(AliveState))
        {
            if (PlayerModel.HealthPoints > 0)
            {
                temp = StateMachine.AliveState;
            }
            else
            {
                temp = StateMachine.DeadState;
            }
        }
        return temp;
    }
}
