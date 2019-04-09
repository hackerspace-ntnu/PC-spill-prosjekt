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
    WALL_JUMPING,
	GRAPPLING,
    DAMAGED,
}

public class Movement : MonoBehaviour
{
	private const float MINIMUM_TIME_BEFORE_AIR_JUMP = 0.1f;
	private const float HORIZONTAL_INPUT_RUNNING_THRESHOLD = 0.3f;
	private const float JUMPING_GRAVITY_SCALE_MULTIPLIER = 0.8f;
    private const float WALL_SLIDE_GRAVITY_SCALE_MULTIPLIER = 0.6f;
    private const float WALL_JUMP_DURATION = 0.2f;

    public KeyCode jumpKey = KeyCode.Space;
	public KeyCode dashKey = KeyCode.LeftShift;

    public int spriteDirection; // direction the character is facing, set in PlayerAnim

    public float movementSpeed = 7;
	public float jumpSpeed = 13.5f;
    public float dashSpeed = 13;
	public float baseGravityScale = 5;
	public float maxVelocityY = 12;

    private float lastActionTime;
    private float dashDuration = 0.2f;

    private int wallJumpDirection;

	private MovementState state;
	private Rigidbody2D rigidBody;

	// TODO: ensure dirty bit is always set
	private bool isVelocityDirty = false;
	private Vector2 newVelocity; // for setting velocity in FixedUpdate()
	private float newGravityScale; // for setting velocity in FixedUpdate()
    private float horizontalInput; // input from controller in x-axis

    private bool isGrounded;
	private bool hasAirJumped = false;
    private bool hasDashed = false;
	private float jumpTime;
    private float wallJumpTime;
    private float maxVelocityFix;

    private int wallTrigger;

    void Start()
	{
		state = MovementState.STANDARD;
		rigidBody = GetComponent<Rigidbody2D>();
		newVelocity = rigidBody.velocity;
		newGravityScale = baseGravityScale;
        maxVelocityFix = 1f;
	}

	void Update()
	{
		if (!isVelocityDirty)
			newVelocity = rigidBody.velocity;

        maxVelocityY = 12f;
        HandleChangeState();

		switch (state)
		{
			case MovementState.STANDARD:
				StandardState();
				break;

			case MovementState.AIR_JUMPING:
			case MovementState.JUMPING:
				StandardState();
                JumpingState();
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

            case MovementState.WALL_JUMPING:
                WallJumpingState();
                break;

            case MovementState.DAMAGED:
                DamagedState();
                break;
        }
	}

    private void HandleChangeState()
	{
        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        maxVelocityFix = 1f;

        if (state == MovementState.WALL_JUMPING)
        {
            return;
        }
        else if (state == MovementState.DASHING)
        {
            if (wallTrigger != 0)
            {
                if (!isGrounded)
                    state = MovementState.WALL_CLINGING;
                else
                    state = MovementState.STANDARD;
            }
        }
        else if (Input.GetKeyDown(jumpKey) && wallTrigger == 0) // If just pressed jumpKey and there's no collision detected by the wallTriggers:
        {
            if (isGrounded)
                state = MovementState.JUMPING;
            else if (!hasAirJumped && Time.time >= jumpTime + MINIMUM_TIME_BEFORE_AIR_JUMP)
                state = MovementState.AIR_JUMPING;
        }
        else if (Input.GetKeyDown(dashKey) && !hasDashed)
        {
            lastActionTime = Time.time;
            state = MovementState.DASHING;
        }
        // if F: grapple
        else if (wallTrigger != 0 && !isGrounded)
        {
            state = MovementState.WALL_CLINGING;
        }
        else
            state = MovementState.STANDARD;
    }

	private void StandardState()
	{
        HandleHorizontalInput();
        isVelocityDirty = true;
	}

	private void JumpingState()
	{
		switch (state)
		{
			case MovementState.JUMPING:
				Jump(1.0f);
				if (Input.GetKey(jumpKey) && IsVelocityUpwards())
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

	private void DashingState()
	{
        hasDashed = true;
        if (Time.time - lastActionTime <= dashDuration)
        {
            newVelocity = new Vector2(spriteDirection * dashSpeed, 0);
            newGravityScale = 0;
        }
        else
        {
            state = MovementState.STANDARD;
            newGravityScale = baseGravityScale;
        }
    }

    private void WallClingingState()
	{
        HandleHorizontalInput();

        wallJumpDirection = wallTrigger;

        if (rigidBody.velocity.y * Math.Sign(newGravityScale) <= 0)
            newGravityScale = baseGravityScale * WALL_SLIDE_GRAVITY_SCALE_MULTIPLIER;

        if (Input.GetKeyDown(jumpKey))
        {
            rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            newGravityScale = baseGravityScale * JUMPING_GRAVITY_SCALE_MULTIPLIER;
            wallJumpTime = Time.time;
            jumpTime = Time.time;
            WallJumpingState();

            hasDashed = false;
            hasAirJumped = false;

            state = MovementState.WALL_JUMPING;

            return;
        }
        else if (Math.Abs(rigidBody.velocity.y) <= 6 && wallTrigger == -Math.Sign(newVelocity.x))
        {
            rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            maxVelocityY = 2f;
            print(rigidBody.velocity.y);
        }
    }

    private void WallJumpingState()
    {
        if (Time.time - wallJumpTime <= WALL_JUMP_DURATION / 2)
        {
            if (horizontalInput != 0)
                newVelocity = new Vector2(wallJumpDirection * dashSpeed / 2, jumpSpeed * 0.64f * Math.Sign(newGravityScale));
            else
                newVelocity = new Vector2(wallJumpDirection * movementSpeed / 2, jumpSpeed * 0.75f * Math.Sign(newGravityScale));
        }
        else if (Time.time - wallJumpTime <= WALL_JUMP_DURATION)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            newVelocity = new Vector2(horizontalInput * dashSpeed / 2, jumpSpeed * 0.64f * Math.Sign(newGravityScale));
        }
        else
        {
            newGravityScale = baseGravityScale;
            state = MovementState.STANDARD;
        }

        isVelocityDirty = true;
        Debug.Log("WallJumpingState");
    }

	private void GrapplingState()
	{

	}

    private void DamagedState()
    {

        hasAirJumped = false;
        hasDashed = false;
    }

    void FixedUpdate()
	{
        if (Math.Sign(baseGravityScale) == 1 && rigidBody.velocity.y <= -maxVelocityY || Math.Sign(baseGravityScale) == -1 && rigidBody.velocity.y >= maxVelocityY)
        {
            maxVelocityFix = 0.8f;
        }

        else
        {
            maxVelocityFix = 1f;
        }

        rigidBody.velocity = new Vector2(newVelocity.x, newVelocity.y * maxVelocityFix); // Ta inn MaxVelocityFix, mye renere
		isVelocityDirty = false;
		if (rigidBody.gravityScale != newGravityScale)
			rigidBody.gravityScale = newGravityScale;

	}

    private void HandleHorizontalInput()
    {
        if (Math.Abs(horizontalInput) <= Math.Abs(Input.GetAxis("Horizontal")))
        {
            horizontalInput = Input.GetAxis("Horizontal");
            if (Math.Abs(horizontalInput) > HORIZONTAL_INPUT_RUNNING_THRESHOLD)
            {
                newVelocity.x = Math.Sign(horizontalInput) * movementSpeed; // Set horizontalInput to max
            }
            else
            {
                newVelocity.x = horizontalInput * movementSpeed;
            }
        }
        else
        {
            newVelocity.x = 0;
            horizontalInput = Input.GetAxis("Horizontal");
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

    public Vector2 GetVelocity()
    {
        return rigidBody.velocity;
    }

    public int GetWallTrigger()
    {
        return wallTrigger;
    }

    public bool GetGrounded()
    {
        return isGrounded;
    }

    public float GetHorizontalInput()
    {
        return horizontalInput;
    }

    public int GetPlayerMovementState()
    {
        return ((int) state);
    }

    public void SetGrounded(bool grounded)
    {
        isGrounded = grounded;
        if (grounded)
        {
            hasAirJumped = false;
            hasDashed = false;
            newGravityScale = baseGravityScale;
        }
    }

    public void SetWallTrigger(int trigger)
    {
        wallTrigger = trigger;
    }
}
