using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : AVitalState
{
    internal override void EntryAction()
    {
        IsActive = true;
    }

    internal override void ExitAction()
    {
        IsActive = false;
    }

    protected override void FixedUpdate()
    {
    }

    protected override void Start()
    {
        Body = GameObject.Find("View").GetComponent<Rigidbody2D>();
        StateName = "Player is dead ";
        base.Start();
    }

    protected override void Update()
    {
        if (IsActive)
        {
            // check if any other states can be transitioned into
            this.TargetTransitionState = CheckTriggers();

            if (this.TargetTransitionState == null || this.TargetTransitionState == this)
            {
                // TODO: Any logic that handles input while player is dead (for example a button click that says "respawn").
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
