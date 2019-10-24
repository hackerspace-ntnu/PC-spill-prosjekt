using System;
using Unity;
using UnityEngine;

public abstract class APositionState : AState
{
    private APositionState targetTransitionState;
    protected virtual APositionState TargetTransitionState
    {
        get => targetTransitionState;
        set
        {
            if (value == null)
            {
                targetTransitionState = value;
            }
            else if (value is APositionState)
            {
                targetTransitionState = value;
            }
            else
            {
                return;
            }
        }
    }
    //  TODO: Add virtual methods/fields relevant only to positional states.
    protected abstract APositionState CheckTriggers();

}

