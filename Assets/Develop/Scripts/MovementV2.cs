using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

enum MovementState
{
	STANDARD,
	JUMPING,
	AIR_JUMPING,
	DASHING,
	WALL_CLINGING,
	GRAPPLING,
}

public class MovementV2 : MonoBehaviour
{
	private const float MINIMUM_TIME_BEFORE_AIR_JUMP = 0.1f;
	private const float HORIZONTAL_INPUT_RUNNING_THRESHOLD = 0.3f;
	private const float JUMPING_GRAVITY_SCALE_MULTIPLIER = 0.8f;

	public KeyCode jumpKey = KeyCode.Space;
	public KeyCode dashKey = KeyCode.LeftShift;

	public float movementSpeed = 7;
	public float jumpSpeed = 13.5f;
	public float baseGravityScale = 5;
	public float maxVelocityY = 12;

	private MovementState state;
	private Rigidbody2D rigidBody;

	// TODO: ensure dirty bit is always set
	private bool isVelocityDirty = false;
	private Vector2 newVelocity; // for setting velocity in FixedUpdate()
	private float newGravityScale; // for setting velocity in FixedUpdate()

	private bool isGrounded;
	private bool hasAirJumped = false;
	private float jumpTime;

	public void SetGrounded(bool grounded)
	{
		isGrounded = grounded;
		if (grounded)
			hasAirJumped = false;
		Debug.Log("grounded: " + grounded);
	}

	void Start()
	{
		state = MovementState.STANDARD;
		rigidBody = GetComponent<Rigidbody2D>();
		newVelocity = rigidBody.velocity;
		newGravityScale = baseGravityScale;
	}

	void Update()
	{
		if (!isVelocityDirty)
			newVelocity = rigidBody.velocity;

		HandleChangeState();

		switch (state)
		{
			case MovementState.STANDARD:
				StandardState();
				break;

			case MovementState.AIR_JUMPING:
			case MovementState.JUMPING:
				JumpingState();
				StandardState();
				break;

			case MovementState.DASHING:
				DashingState();
				break;

			case MovementState.WALL_CLINGING:
				WallClingingState();
				break;

			case MovementState.GRAPPLING:
				GrapplingState();
				break;
		}
	}

	private void HandleChangeState()
	{
		if (Input.GetKeyDown(jumpKey))
		{
			if (isGrounded)
				state = MovementState.JUMPING;
			else if (!hasAirJumped && Time.time >= jumpTime + MINIMUM_TIME_BEFORE_AIR_JUMP)
				state = MovementState.AIR_JUMPING;
		} else if (Input.GetKeyDown(dashKey))
			state = MovementState.DASHING;
		// if F: grapple
	}

	private void StandardState()
	{
		float horizontalInput = Input.GetAxis("Horizontal");
		if (Math.Abs(horizontalInput) > HORIZONTAL_INPUT_RUNNING_THRESHOLD)
			// Set horizontalInput to max
			horizontalInput = Math.Sign(horizontalInput);

		newVelocity.x = horizontalInput * movementSpeed * Math.Sign(rigidBody.gravityScale);

		if (newVelocity.y > maxVelocityY)
			newVelocity.y = maxVelocityY;

		isVelocityDirty = true;
	}

	private void JumpingState()
	{
		switch (state)
		{
			case MovementState.JUMPING:
				// If just pressed jumpKey:
				if (Input.GetKeyDown(jumpKey))
					Jump(1.0f);
				else if (Input.GetKey(jumpKey) && IsVelocityUpwards())
					newGravityScale = JUMPING_GRAVITY_SCALE_MULTIPLIER * baseGravityScale;
				else
				{
					newGravityScale = baseGravityScale;
					state = MovementState.STANDARD;
				}
				break;

			case MovementState.AIR_JUMPING:
				Jump(0.8f);
				Debug.Log("air jumping");
				hasAirJumped = true;
				state = MovementState.STANDARD;
				break;
		}
	}

	private bool IsVelocityUpwards()
	{
		return rigidBody.velocity.y * Math.Sign(rigidBody.gravityScale) > 0;
	}

	private void Jump(float scale)
	{
		newVelocity.y = jumpSpeed * scale * Math.Sign(rigidBody.gravityScale);
		isVelocityDirty = true;
		jumpTime = Time.time;
	}

	private void DashingState()
	{

	}

	private void WallClingingState()
	{

	}

	private void GrapplingState()
	{

	}

	void FixedUpdate()
	{
		rigidBody.velocity = newVelocity;
		isVelocityDirty = false;
		if (rigidBody.gravityScale != newGravityScale)
			rigidBody.gravityScale = newGravityScale;

		switch (state)
		{
			case MovementState.STANDARD:
				//StandardState_Fixed();
				break;

			case MovementState.JUMPING:
				//JumpingState();
				break;

			case MovementState.DASHING:
				//DashingState();
				break;

			case MovementState.WALL_CLINGING:
				//WallClingingState();
				break;

			case MovementState.GRAPPLING:
				//GrapplingState();
				break;
		}
	}

	//private void StandardState_Fixed()
	//{
	//}
}
