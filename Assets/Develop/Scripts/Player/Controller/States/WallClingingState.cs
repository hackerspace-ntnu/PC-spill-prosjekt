using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClingingState : PlayerState
{
    public static readonly WallClingingState INSTANCE = new WallClingingState();

    public override string Name => "WALL_CLINGING";
    private int wallJumpDirection;

    public override void Enter()
    {
        HandleHorizontalInput();
    }

    public override void Update()
    {
        HandleHorizontalInput();

        if (rigidBody.velocity.y * flipGravityScale <= 0)
            rigidBody.gravityScale = WALL_SLIDE_GRAVITY_SCALE * flipGravityScale;

        if (Input.GetButtonDown("Jump"))
        {
            //rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            jumpTime = Time.time;
            hasDashed = false;
            hasAirJumped = false;
            controller.ChangeState(JumpingState.INSTANCE);
        }
        else
        {
            //rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
           //maxVelocityY = 2f;
        }

        if (Grounded)
        {
            controller.ChangeState(IdleState.INSTANCE);
        }
        else if (wallTrigger == 0)
        {
            controller.ChangeState(AirborneState.INSTANCE);
        }
    }

    public override void FixedUpdate()
    {

    }

    public override void Exit()
    {

    }
}
