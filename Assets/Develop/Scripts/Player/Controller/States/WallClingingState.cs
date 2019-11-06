using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClingingState : PlayerState
{
    public static readonly WallClingingState INSTANCE = new WallClingingState();

    public override string Name => "WALL_CLINGING";

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
