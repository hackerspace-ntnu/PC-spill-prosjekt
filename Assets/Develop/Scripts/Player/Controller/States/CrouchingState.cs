using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchingState : PlayerState
{
    public static readonly CrouchingState INSTANCE = new CrouchingState();

    public override string Name => "CROUCHING";

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
