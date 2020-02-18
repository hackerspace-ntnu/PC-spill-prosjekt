using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingState : PlayerState
{
    public static readonly GrapplingState INSTANCE = new GrapplingState();

    public override string Name => "GRAPPLING";

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
