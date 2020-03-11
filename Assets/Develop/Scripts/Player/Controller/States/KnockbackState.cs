using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackState : PlayerState
{
    public static readonly KnockbackState INSTANCE = new KnockbackState();

    public override string Name => "KNOCKBACK";

    private KnockbackState() {}

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

