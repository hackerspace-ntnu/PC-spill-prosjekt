using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnWallJump : BaseState
{
    protected override BaseState CheckTriggers<T>(Rigidbody2D body)
    {
        return base.CheckTriggers<T>(body);
    }

    protected override void FixedUpdate()
    {
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
        this.TargetTransitionState = null;
        IsActive = false;
    }

}
