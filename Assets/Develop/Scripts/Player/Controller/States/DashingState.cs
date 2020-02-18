using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingState : PlayerState
{
    public static readonly DashingState INSTANCE = new DashingState();

    public override string Name => "DASHING";

    private float dashDuration = 0.2f;

    public override void Enter()
    {
        controller.HasDashed = true;
        controller.DashTime = Time.time;

        controller.TargetVelocity = new Vector2((int)controller.Dir * dashSpeed * flipGravityScale, 0);
        rigidBody.gravityScale = 0;
    }

    public override void Update()
    {
        if (controller.WallTrigger != 0)
            controller.ChangeState(WallClingingState.INSTANCE);

        else if (Time.time - controller.DashTime >= dashDuration)
        {
            if (controller.Grounded)
                controller.ChangeState(IdleState.INSTANCE);
            else
                controller.ChangeState(AirborneState.INSTANCE);
        }
    }

    public override void FixedUpdate()
    {
        float newVelocityX = controller.TargetVelocity.x - rigidBody.velocity.x;
        float newVelocityY = controller.TargetVelocity.y - rigidBody.velocity.y;

        rigidBody.AddForce(new Vector2(newVelocityX, newVelocityY), ForceMode2D.Impulse);
    }

    public override void Exit()
    {
        rigidBody.gravityScale = baseGravityScale * flipGravityScale;
    }
}
