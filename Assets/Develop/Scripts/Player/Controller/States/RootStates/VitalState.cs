using System;
using UnityEngine;
class VitalState : BaseState
{
    protected override BaseState TargetTransitionState { get => base.TargetTransitionState; set => base.TargetTransitionState = value; }

    protected override BaseState CheckTriggers<T>(Rigidbody2D body)
    {
        return base.CheckTriggers<T>(body);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    internal override void EntryAction()
    {
        base.EntryAction();
    }

    internal override void ExitAction()
    {
        base.ExitAction();
    }
}

