using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchCrouchingState : CrouchingState
{
    public new static readonly GlitchCrouchingState INSTANCE = new GlitchCrouchingState();

    private const float crouchSpeed = 1f;

    public override string Name => "GLITCH_CROUCHING";

    // Game objects for crouching and uncrouching
    private GameObject colliderFullHeight;
    private GameObject colliderCrouch;
    private GameObject crouchCeilingDetector;

    public override void Init(PlayerController controller) {
        base.Init(controller);

        controller.TargetVelocity = rigidBody.velocity;

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
        base.Enter();

        controller.Animator.SetBool("GlitchCrouch", true);
    }

    public override void Update()
    {
        if (!Input.GetButton("Crouch") && controller.CanUncrouch)
        {
            controller.ChangeState(IdleState.INSTANCE);
        }

        if (rigidBody.velocity.y * controller.FlipGravityScale < 0.0f)
        {
            controller.ChangeState(AirborneState.INSTANCE);
        }
    }

    public override void FixedUpdate()
    {
        float newVelocityX = controller.TargetVelocity.x - rigidBody.velocity.x;

        rigidBody.AddForce(new Vector2(newVelocityX, 0), ForceMode2D.Impulse);
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

    public override void Dash()
    {
        if (controller.CanUncrouch)
        {
            if (controller.GlitchActive)
            {
                controller.ChangeState(GlitchDashingState.INSTANCE);
            }
            else
            {
                controller.ChangeState(DashingState.INSTANCE);
            }
        }
    }

    public override void ToggleGlitch()
    {
        controller.ChangeState(CrouchingState.INSTANCE);
    }
}
