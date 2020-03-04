using System;
using System.Collections;
using System.Collections.Generic;
using GlobalEnums;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HookHead : MonoBehaviour
{
    private readonly int COLLIDERS_ONLY_MASK = Layers.MaskFromLayer(Layers.COLLIDERS);

    public GrapplingState grapplingState;

    public PlayerController playerController;
    public Transform containerObject;

    public float movementSpeed;
    [Tooltip("In world units.")]
    public float maxFiringLength;

    private new Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    private float spriteWorldHeight;

    private Vector3 firedDirection;
    private bool stopped;

    public void Destroy()
    {
        // TODO: play sound of stuffing away grappling hook?
        Destroy(containerObject.gameObject);
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteWorldHeight = SpriteUtils.GetWorldSize(spriteRenderer.sprite.bounds.size, transform).y;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        firedDirection = playerController.transform.position.DirectionTo(mouseWorldPos);
        stopped = false;

        // TODO: play throwing sound

        // Move a little bit and update rotation before first frame update to prevent default rotation in first frame
        MoveInFiredDirection(spriteWorldHeight / 2f);
        UpdateRotation();
    }

    void Update()
    {
        // Destroy hook if player presses jump button before hook has hit anything
        if (Input.GetButtonDown("Jump")
            && playerController.GetCurrentState() != GrapplingState.INSTANCE)
        {
            Destroy();
        }
    }

    void FixedUpdate()
    {
        if (stopped)
        {
            UpdateRotation();
            return;
        }

        // Check if hook is about to hit anything
        RaycastHit2D raycastHit = Physics2D.Raycast(rigidbody.position, firedDirection, spriteWorldHeight + movementSpeed, COLLIDERS_ONLY_MASK);
        if (raycastHit.collider != null)
        {
            // Move hook to its final position
            rigidbody.MovePosition(raycastHit.point);
            // Manually trigger collision, because collisions are too inconsistent otherwise
            OnTriggerEnter2D(raycastHit.collider);
            return;
        }

        MoveInFiredDirection(movementSpeed);
        UpdateRotation();

        // Destroy hook if it's too far away from player
        Vector3 firingDistance = SpriteUtils.GetDistanceBetween(playerController.transform, spriteRenderer, SquareEdge.TOP);
        if (firingDistance.magnitude >= maxFiringLength)
        {
            grapplingState.OnGrapplingHookStopped();
            Destroy();
        }
    }

    private void MoveInFiredDirection(float distance)
    {
        Vector3 newPosition = rigidbody.position.ExtendInDirection(firedDirection, distance);
        rigidbody.position = newPosition;
    }

    /// <summary>
    ///   <para>Rotates hook and chain to match player's position.</para>
    /// </summary>
    private void UpdateRotation()
    {
        // Use rigidbody's position instead of transform's, as it's the rigidbody that's moved each physics update
        Vector2 directionFromPlayer = rigidbody.position - playerController.transform.position.To2();
        float directionAngle = Vector2.SignedAngle(Vector3.down, directionFromPlayer);
        transform.localRotation = Quaternion.Euler(0f, 0f, directionAngle);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (stopped)
            return;

        stopped = true;

        // TODO: play hook hit sound

        // Collisions between player and weapons - the layer the hook is normally on - are ignored,
        // so set layer to "Triggers", as player needs to know when it has reached the hook
        gameObject.layer = Layers.TRIGGERS;

        playerController.OnGrapplingHookHit();
    }
}
