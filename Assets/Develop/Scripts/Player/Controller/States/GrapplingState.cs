using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class GrapplingState : PlayerState
{
    public static readonly GrapplingState INSTANCE = new GrapplingState();

    public override string Name => "GRAPPLING";

    private float lastGrapplingStoppedTime;
    private HookHead firedHook;

    private Vector2? lastHookDirection;
    private bool hasReachedHook;
    private Vector2? targetVelocityWhenReachedHook;

    private GrapplingState() {}

    public override void Init(PlayerController controller)
    {
        base.Init(controller);
        // Lets the player grapple as soon as the game starts
        lastGrapplingStoppedTime = Time.time - controller.delayBetweenGrapplingAttempts;
    }

    public void FireGrapplingHook()
    {
        if (firedHook != null || Time.time - lastGrapplingStoppedTime < controller.delayBetweenGrapplingAttempts)
            return;

        // Instantiate and initialize grappling hook
        firedHook = Object.Instantiate(
                              controller.grapplingHookPrefab, controller.transform.position, Quaternion.identity, controller.transform.parent
                          )
                          .GetComponentInChildren<HookHead>();
        firedHook.grapplingState = this;
        firedHook.playerController = controller;
    }

    public override void Enter()
    {
        rigidbody.gravityScale = 0f;
        lastHookDirection = null;
        hasReachedHook = false;
        targetVelocityWhenReachedHook = null;
    }

    public override void Update()
    {
        if (Input.GetButtonDown("Jump"))
            StopGrappling();
    }

    public override void FixedUpdate()
    {

        Vector2 hookDirection = rigidbody.position.DirectionTo(firedHook.transform.position.To2());
        // Use the same velocity as when reached hook, to not prevent sliding (over walls, and on floors and ceilings)
        Vector2 targetVelocity = targetVelocityWhenReachedHook ?? controller.grapplingSpeed * hookDirection;

        // Set variable once hook has been reached
        if (hasReachedHook && targetVelocityWhenReachedHook == null)
            targetVelocityWhenReachedHook = targetVelocity;

        // Wait for player to move at least one frame before deciding whether to stop
        if (lastHookDirection != null)
        {
            if (hasReachedHook
                // Wait for player to change direction (after having reached hook) before stopping
                && !hookDirection.HasEqualSignAs(lastHookDirection.Value))
            {
                StopGrappling();
                return;
            }
        }

        // Equation: rigidbody.velocity + newVelocity = targetVelocity
        Vector2 newVelocity = targetVelocity - rigidbody.velocity;
        rigidbody.AddForce(newVelocity, ForceMode2D.Impulse);

        lastHookDirection = hookDirection;
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
