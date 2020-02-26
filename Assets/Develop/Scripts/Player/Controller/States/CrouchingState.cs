﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchingState : PlayerState
{
    public static readonly CrouchingState INSTANCE = new CrouchingState();

    private const float CROUCH_HEIGHT_RATIO = 0.5f;
    private const float CROUCH_SPEED = 0.5f;

    public override string Name => "CROUCHING";

    protected virtual string AnimatorParameterName => "Crouch";

    // Game objects for crouching and uncrouching
    protected BoxCollider2D collider;
    protected GameObject crouchCeilingDetector;

    protected Vector2 baseColliderSize;
    protected Vector2 baseColliderOffset;
    protected Vector2 crouchColliderSize;
    protected Vector2 crouchColliderOffset;

    protected int animatorParameterId;

    public override void Init(PlayerController controller) {
        base.Init(controller);

        crouchCeilingDetector = controller.transform.Find("CrouchCeilingDetector").gameObject;
        if (!crouchCeilingDetector) {
            Debug.LogError("CROUCHING STATE: Crouch Ceiling Check is empty");
        }

        collider = controller.GetComponent<BoxCollider2D>();

        baseColliderSize = collider.size;
        baseColliderOffset = collider.offset;

        crouchColliderSize = baseColliderSize;
        crouchColliderSize.y *= CROUCH_HEIGHT_RATIO;

        crouchColliderOffset = baseColliderOffset;
        float heightDifference = baseColliderSize.y - crouchColliderSize.y;
        crouchColliderOffset.y = -heightDifference / 2f;

        animatorParameterId = Animator.StringToHash(AnimatorParameterName);
    }


    public override void Enter()
    {
        collider.size = crouchColliderSize;
        collider.offset = crouchColliderOffset;

        crouchCeilingDetector.SetActive(true);

        controller.CanUncrouch = true;

        controller.Animator.SetBool(animatorParameterId, true);
    }

    public override void Update()
    {
        if (!Input.GetButton("Crouch") && controller.CanUncrouch) {
            controller.ChangeState(IdleState.INSTANCE);
        }

        if (rigidbody.velocity.y * controller.FlipGravityScale < 0.0f) {
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
        collider.size = baseColliderSize;
        collider.offset = baseColliderOffset;

        crouchCeilingDetector.SetActive(false);

        controller.Animator.SetBool(animatorParameterId, false);
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
        controller.ChangeState(GlitchCrouchingState.INSTANCE);
    }
}
