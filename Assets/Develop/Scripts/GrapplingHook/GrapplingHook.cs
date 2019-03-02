using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
	public GrapplingHookController controller;
	public float movementSpeed = 0.2f;
	public float maxFiringLength = 4.0f;

	private Vector3 direction;
	private bool stopped = false;

	void Start()
	{
		direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - controller.transform.position;
		direction.z = 0; // because it's not used
		direction.Normalize();
	}

	void FixedUpdate()
	{
		if (stopped)
			return;

		Vector3 nextPosition = transform.position;
		nextPosition.x += direction.x * movementSpeed;
		nextPosition.y += direction.y * movementSpeed;
		transform.position = nextPosition;

		Vector3 firingDistance = transform.position - controller.transform.position;
		if (firingDistance.magnitude >= maxFiringLength)
			Stop();
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		Stop();
	}

	private void Stop()
	{
		stopped = true;
		controller.OnGrapplingHookStopped();
		GetComponent<BoxCollider2D>().enabled = false;
	}
}
