using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchingState : PlayerState
{
    public static readonly CrouchingState INSTANCE = new CrouchingState();

    private const float CROUCH_SPEED = 0.5f;

    public override string Name => "CROUCHING";

    // Game objects for crouching and uncrouching
    private GameObject colliderFullHeight;
    private GameObject colliderCrouch;
    private GameObject crouchCeilingDetector;

    public override void Init(PlayerController controller) {
        base.Init(controller);

        // Find the game objects related to crouching
        colliderFullHeight = controller.transform.Find("ColliderFullHeight").gameObject;
        if (!colliderFullHeight) {
            Debug.LogError("CROUCHING STATE: Collider full height is empty");
        }

        colliderCrouch = controller.transform.Find("ColliderCrouch").gameObject;
        if (!colliderCrouch) {
            Debug.LogError("CROUCHING STATE: Collider crouch is empty");
        }

        crouchCeilingDetector = controller.transform.Find("CrouchCeilingDetector").gameObject;
        if (!crouchCeilingDetector) {
            Debug.LogError("CROUCHING STATE: Crouch Ceiling Check is empty");
        }
    }


    public override void Enter()
    {
        colliderCrouch.SetActive(true);
        crouchCeilingDetector.SetActive(true);

        colliderFullHeight.SetActive(false);
        controller.CanUncrouch = true;
    }

    public override void Update()
    {

        if (!Input.GetButton("Crouch") && controller.CanUncrouch) {
            controller.ChangeState(IdleState.INSTANCE);
        }

        if (rigidBody.velocity.y * flipGravityScale < 0.0f) {
            controller.ChangeState(AirborneState.INSTANCE);
        }

        base.Update();
    }

    public override void FixedUpdate()
    {

        // Set movement speed to crouch speed.
        controller.TargetVelocity = new Vector2(controller.TargetVelocity.x * CROUCH_SPEED, controller.TargetVelocity.y);

        base.FixedUpdate();
    }

    public override void Exit()
    {
        colliderFullHeight.SetActive(true);

        colliderCrouch.SetActive(false);
        crouchCeilingDetector.SetActive(false);
    }

    public override void Jump() {
        if (controller.CanUncrouch) {
            controller.ChangeState(JumpingState.INSTANCE);
        }
        else {
            controller.JumpButtonPressTime = Time.time;
        }
    }
}
