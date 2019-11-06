using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingState : PlayerState
{
    public static readonly DashingState INSTANCE = new DashingState();

    public override string Name => "DASHING";

    public override void Enter()
    {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {

    }

    public override void FixedUpdate()
    {

    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }
}
