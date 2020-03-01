using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class GrapplingState : PlayerState
{
    public static readonly GrapplingState INSTANCE = new GrapplingState();

    public override string Name => "GRAPPLING";

    private GameObject grapplingHookPrefab;
    private float lastGrapplingStoppedTime;
    private HookHead firedHook;

    private Vector2 lastTargetVelocity;
    private bool hasReachedHook;

    private GrapplingState() {}

    public override void Init(PlayerController controller)
    {
        base.Init(controller);
        grapplingHookPrefab = controller.grapplingHookPrefab;
        // Lets the player grapple as soon as the game starts
        lastGrapplingStoppedTime = Time.time - controller.delayBetweenGrapplingAttempts;
    }

    public void FireGrapplingHook()
    {
        if (firedHook != null || Time.time - lastGrapplingStoppedTime < controller.delayBetweenGrapplingAttempts)
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
        lastTargetVelocity = Vector2.zero;
        hasReachedHook = false;
    }

    public override void Update()
    {
        if (Input.GetButtonDown("Jump"))
            StopGrappling();
    }

    public override void FixedUpdate()
    {
        // Move towards the hook
        Vector2 hookDirection = VectorUtils.GetDirectionToVector(rigidbody.position, firedHook.transform.position);
        Vector2 targetVelocity = controller.grapplingSpeed * hookDirection;

        // Wait for player to change direction (after having reached hook) before stopping
        if (hasReachedHook && VectorUtils.HaveDifferentSigns(targetVelocity, lastTargetVelocity))
        {
            StopGrappling();
            return;
        }

        // Equation: rigidbody.velocity + newVelocity = targetVelocity
        Vector2 newVelocity = targetVelocity - rigidbody.velocity;
        rigidbody.AddForce(newVelocity, ForceMode2D.Impulse);

        lastTargetVelocity = targetVelocity;
    }

    private void StopGrappling()
    {
        firedHook.Destroy();
        firedHook = null;
        lastGrapplingStoppedTime = Time.time;
        controller.ChangeState(AirborneState.INSTANCE);
    }

    public override void OnTriggerEnter2D(Collider2D collider)
    {
        HookHead hitHook = collider.GetComponent<HookHead>();
        if (hitHook != null && hitHook == firedHook)
            hasReachedHook = true;
    }

    public override void Exit()
    {
        UpdateGravity();
    }

    public void OnGrapplingHookStopped()
    {
        firedHook = null;
    }
}
