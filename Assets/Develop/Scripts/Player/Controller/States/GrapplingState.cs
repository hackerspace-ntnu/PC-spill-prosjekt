using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingState : PlayerState
{
    public static readonly GrapplingState INSTANCE = new GrapplingState();

    public override string Name => "GRAPPLING";

    private GameObject grapplingHookPrefab;
    private GrapplingHook firedHook;

    private Vector3 hookHitPos;

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

        firedHook = Object.Instantiate(
                              grapplingHookPrefab, controller.transform.position, Quaternion.identity, controller.transform.parent
                          )
                          .GetComponent<GrapplingHook>();
        firedHook.grapplingState = this;
        firedHook.playerController = controller;
    }

    public override void Enter()
    {
        hookHitPos = firedHook.transform.position;
        rigidBody.gravityScale = 0;
    }

    public override void Update()
    {

    }

    public override void FixedUpdate()
    {

        Vector2 hookDirection = VectorUtils.GetDirectionToVector(rigidBody.position, firedHook.transform.position);
        Vector2 targetVelocity = controller.grapplingSpeed * hookDirection;
        // rigidbody.velocity + newVelocity = targetVelocity
        Vector2 newVelocity = targetVelocity - rigidBody.velocity;
        rigidBody.AddForce(newVelocity, ForceMode2D.Impulse);
    }

    public override void OnTriggerEnter2D(Collider2D collider)
    {
        GrapplingHook hitHook = collider.GetComponent<GrapplingHook>();
        if (hitHook == null || hitHook != firedHook)
            return;
    }

    public override void Exit()
    {
        rigidBody.gravityScale = baseGravityScale;
    }

    public void OnGrapplingHookStopped()
    {
        firedHook = null;
    }
}
