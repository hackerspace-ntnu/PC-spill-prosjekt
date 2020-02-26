﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class GrapplingState : PlayerState
{
    public static readonly GrapplingState INSTANCE = new GrapplingState();

    public override string Name => "GRAPPLING";

    private GameObject grapplingHookPrefab;
    private HookHead firedHook;

    private bool hasGrappledToHook;
    private bool shouldStopMoving;

    private GrapplingState() {}

    public override void Init(PlayerController controller)
    {
        base.Init(controller);
        grapplingHookPrefab = controller.grapplingHookPrefab;
    }

    public void FireGrapplingHook()
    {
        if (firedHook != null)
            return;

        // Instantiate and initialize grappling hook
        firedHook = Object.Instantiate(
                              grapplingHookPrefab, controller.transform.position, Quaternion.identity, controller.transform.parent
                          )
                          .GetComponentInChildren<HookHead>();
        firedHook.grapplingState = this;
        firedHook.playerController = controller;
    }

    public override void Enter()
    {
        rigidbody.gravityScale = 0f;
        hasGrappledToHook = false;
        shouldStopMoving = false;
    }

    public override void Update()
    {
        if (controller.WallTrigger != 0)
        {
            firedHook.Destroy();

            if (controller.GlitchActive)
            {
                controller.ChangeState(GlitchWallClingingState.INSTANCE);
            }
            else
            {
                controller.ChangeState(WallClingingState.INSTANCE);
            }
        }
        else if (Input.GetButtonDown("Grapple"))
        {
            firedHook.Destroy();
            firedHook = null;
            FireGrapplingHook();
            hasGrappledToHook = false;
            controller.ChangeState(AirborneState.INSTANCE);
        }
    }

    public override void FixedUpdate()
    {
        // Move towards the hook
        Vector2 hookDirection = VectorUtils.GetDirectionToVector(rigidbody.position, firedHook.transform.position);
        Vector2 targetVelocity = controller.grapplingSpeed * hookDirection;
        // Equation: rigidbody.velocity + newVelocity = targetVelocity
        Vector2 newVelocity = targetVelocity - rigidbody.velocity;
        rigidbody.AddForce(newVelocity, ForceMode2D.Impulse);
    }

    // Exits state and destroys hook if player reaches hook
    public override void OnTriggerEnter2D(Collider2D collider)
    {
        HookHead hitHook = collider.GetComponent<HookHead>();
        if (hitHook != null && hitHook == firedHook)
        {
            firedHook.Destroy();
            
            if (controller.Grounded)
            {
                controller.ChangeState(IdleState.INSTANCE);
            }
            else if (controller.WallTrigger != 0)
            {
                if (controller.GlitchActive)
                {
                    controller.ChangeState(GlitchWallClingingState.INSTANCE);
                }
                else
                {
                    controller.ChangeState(WallClingingState.INSTANCE);
                }
            }
            else
            {
                controller.ChangeState(AirborneState.INSTANCE);
            }
        }
    }

    public override void Exit()
    {
        UpdateGravity();
    }

    public void OnGrapplingHookStopped()
    {
        firedHook = null;
    }

    public override void Jump()
    {
        firedHook.Destroy();
        if (controller.GlitchActive)
        {
            controller.ChangeState(JumpingState.INSTANCE);
        }
        else
        {
            controller.ChangeState(AirborneState.INSTANCE);
        }
    }
}
