using System;
using UnityEngine;
public abstract class AVitalState : AState
{
    //  TODO: Add virtual methods/fields relevant only to vital states.
    // defaults to null, but have to be assigned because compiler is annoying me.
    private AVitalState targetTransitionState;
    protected virtual AVitalState TargetTransitionState
    {
        get => targetTransitionState;
        set
        {
            if (value == null)
            {
                targetTransitionState = value;
            }
            else if (value is AVitalState)
            {
                targetTransitionState = value;
            }
            else
            {
                return;
            }
        }
    }

    protected abstract AVitalState CheckTriggers();
}

