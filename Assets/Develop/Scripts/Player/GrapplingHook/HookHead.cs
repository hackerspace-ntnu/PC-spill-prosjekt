using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookHead : MonoBehaviour
{
    public GrapplingState grapplingState;
    public PlayerController playerController;

    public float movementSpeed;
    public float maxFiringLength;

    private Vector3 direction;
    private bool stopped = false;

    public void Destroy()
    {
        Destroy(gameObject);
    }

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
            Destroy();
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
