using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockedBackState : PlayerState
{
    // Start is called before the first frame update

    public static readonly KnockedBackState INSTANCE = new KnockedBackState();

    public override string Name => "KNOCKED_BACK";

    public override void Enter()
    {
        base.Enter();
        controller.HasAirJumped = true;

    }
    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();

    }
}
