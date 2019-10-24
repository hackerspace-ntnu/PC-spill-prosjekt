using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AliveState : AVitalState
{

    internal override void EntryAction()
    {
        IsActive = true;
    }

    internal override void ExitAction()
    {
        this.TargetTransitionState = null;
        IsActive = false;
    }

    protected override void Start()
    {
        Body = GameObject.Find("View").GetComponent<Rigidbody2D>();
        StateName = "Player is alive.";
        base.Start();
    }

    protected override void Update()
    {
        if (IsActive)
        {
            if (IsActive)
            {
                // check if any other states can be transitioned into
                this.TargetTransitionState = CheckTriggers();


                if(this.TargetTransitionState == null || this.TargetTransitionState == this)
                {
                    // TODO: Any health related logic that decreases/increases life of player, put it here.
                }
            }
        }
    }

    protected override AVitalState CheckTriggers()
    {
        AVitalState temp = null; // defaults to null, but have to be assigned because compiler is annoying me.
        if (PlayerModel.HealthPoints > 0)
        {
            temp = StateMachine.AliveState;
        }
        else
        {
            temp = StateMachine.DeadState;
        }
        return temp;
    }

    internal override StateTransition GetTransition()
    {
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
}
