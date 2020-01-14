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
        controller.Animator.SetBool("WallCling", true);
    }

    public override void Update()
    {
        HandleHorizontalInput();

        if (rigidBody.velocity.y * flipGravityScale <= 0)
            rigidBody.gravityScale = WALL_SLIDE_GRAVITY_SCALE * flipGravityScale;

        // if dashinput, Dash

        if (Input.GetButtonDown("Jump"))
        {
            //rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            controller.JumpTime= Time.time;
            controller.HasDashed = false;
            controller.HasAirJumped = false;
            controller.ChangeState(JumpingState.INSTANCE);
        }
        else
        {
            //rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
           //maxVelocityY = 2f;
        }

        if (controller.Grounded)
        {
            controller.ChangeState(IdleState.INSTANCE);
        }
        else if (controller.WallTrigger == 0)
        {
            controller.ChangeState(AirborneState.INSTANCE);
        }
    }

    public override void FixedUpdate()
    {

    }

    public override void Exit()
    {
        controller.Animator.SetBool("WallCling", false);
    }
}
