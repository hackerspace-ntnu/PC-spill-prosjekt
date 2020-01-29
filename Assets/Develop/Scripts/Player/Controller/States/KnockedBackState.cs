using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockedBackState : PlayerState
{
    // Start is called before the first frame update

    public static readonly KnockedBackState INSTANCE = new KnockedBackState();


    private float TimeOfEnter;
    public override string Name => "KNOCKED_BACK";
    

    public override void Enter()
    {
        base.Enter();
        TimeOfEnter = Time.time;
        controller.HasAirJumped = true;
    }
    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate()
    {
        //base.FixedUpdate();
        if(TimeOfEnter + controller.KnockBackDuration < Time.time)
        {
            if(controller.Grounded)
            {
                controller.ChangeState(IdleState.INSTANCE);
            }
            else
            {
                controller.ChangeState(AirborneState.INSTANCE);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
