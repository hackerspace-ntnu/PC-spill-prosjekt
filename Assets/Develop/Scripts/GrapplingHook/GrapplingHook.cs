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
		direction = GetDirectionToVector(controller.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
	}

	void FixedUpdate()
	{
		if (stopped)
			return;

		transform.position = ExtendVectorInDirection(transform.position, direction, movementSpeed);

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

	private static Vector3 GetDirectionToVector(Vector3 fromVector, Vector3 toVector)
	{
		Vector3 direction = toVector - fromVector;
		direction.z = 0; // because it's not used
		direction.Normalize();
		return direction;
	}


	/// <summary>
	///   <para>Returns a vector that has been extended a distance in the given direction from the base vector.</para>
	/// </summary>
	/// <param name="vec">The base vector.</param>
	/// <param name="direction">The direction to extend in. Is assumed to be a normalized vector.</param>
	/// <param name="distance"></param>
	private static Vector3 ExtendVectorInDirection(Vector3 vec, Vector3 direction, float distance)
	{
		Vector3 extendedVec = vec;
		extendedVec.x += direction.x * distance;
		extendedVec.y += direction.y * distance;
		return extendedVec;
	}
}
