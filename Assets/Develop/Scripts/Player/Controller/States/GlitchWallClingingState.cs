using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchWallClingingState : WallClingingState
{
    public new static readonly GlitchWallClingingState INSTANCE = new GlitchWallClingingState();

    public override string Name => "GLITCH_WALL_CLINGING";

    private const float SPRITE_POS_OFFSET = 0.2f;

    private bool isStuck = false;

    private GlitchWallClingingState() {}

    public override void Enter()
    {
        if (Time.time - controller.JumpButtonPressTime < 0.2f)
        {
            controller.ChangeState(JumpingState.INSTANCE);
            return;
        }
    }

    public override void Update()
    {
        base.Update();
        if(Math.Abs(rigidbody.velocity.y) <= IDLE_SPEED_TRESHOLD && !isStuck) {
            isStuck = true;
            TogglePlayerStuck();
        } else if(Math.Abs(rigidbody.velocity.y) > IDLE_SPEED_TRESHOLD && isStuck) {
            isStuck = false;
            TogglePlayerStuck();
        }

        if (Math.Sign(horizontalInput) == -controller.WallTrigger)
        {
            rigidbody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
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

    public override void Exit()
    {
        controller.Animator.SetBool("WallCling", false);
        controller.Animator.SetBool("Idle", false);
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public override void Jump()
    {
        controller.ChangeState(JumpingState.INSTANCE);
    }

    public override void Dash()
    {
        if (!controller.HasDashed)
        {
            controller.ChangeState(DashingState.INSTANCE);
        }
    }

    public override void ToggleGlitch()
    {
        controller.ChangeState(WallClingingState.INSTANCE);
    }

    private void TogglePlayerStuck() {

        controller.Animator.SetBool("WallCling", !isStuck);
        controller.Animator.SetBool("Idle", isStuck);

        Vector3 animPos = controller.SkeletonMecanim.gameObject.transform.position;
        animPos.x =  isStuck ? animPos.x - SPRITE_POS_OFFSET : animPos.x + SPRITE_POS_OFFSET;
        controller.SkeletonMecanim.gameObject.transform.position = animPos;
    }
}
