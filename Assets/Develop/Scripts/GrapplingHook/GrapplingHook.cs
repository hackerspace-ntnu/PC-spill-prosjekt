using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public GrapplingState grapplingState;
    public PlayerController playerController;

    public float movementSpeed;
    public float maxFiringLength;

    private Vector3 direction;
    private bool stopped = false;

    void Start()
    {
        direction = VectorUtils.GetDirectionToVector(playerController.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    void FixedUpdate()
    {
        if (stopped)
            return;

        transform.position = VectorUtils.ExtendVectorInDirection(transform.position, direction, movementSpeed);

        Vector3 playerPos = playerController.transform.position;
        Vector3 firingDistance = transform.position - playerPos;
        if (firingDistance.magnitude >= maxFiringLength)
        {
            Stop();
            grapplingState.OnGrapplingHookStopped();
            // Place the hook in the final position:
            Vector3 directionFromController = VectorUtils.GetDirectionToVector(playerPos, transform.position);
            transform.position = VectorUtils.ExtendVectorInDirection(playerPos, directionFromController, maxFiringLength);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Stop();
        playerController.OnGrapplingHookHit();
    }

    private void Stop()
    {
        stopped = true;
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
