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
	private const float HORIZONTAL_INPUT_RUNNING_THRESHOLD = 0.3f; // Minimum input needed to move left/right
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
    private int flipGravityScale = 1;

	private MovementState state;
	private Rigidbody2D rigidBody;

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
		newGravityScale = baseGravityScale * flipGravityScale;
        maxVelocityFix = 1f;
	}

	void Update()
	{
        /*
		if (!isVelocityDirty)
			newVelocity = rigidBody.velocity;*/

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
        else if (Input.GetKeyDown(jumpKey) && (wallTrigger == 0 || isGrounded)) // If just pressed jumpKey and there's no collision detected by the wallTriggers:
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
        if (isGrounded)
        {
            hasDashed = false;
        }
    }

	private void JumpingState()
	{
        switch (state)
		{
			case MovementState.JUMPING:
				Jump(1.0f);
				if (Input.GetKey(jumpKey) && IsVelocityUpwards())
					newGravityScale = JUMPING_GRAVITY_SCALE_MULTIPLIER * baseGravityScale * flipGravityScale;
				else
				{
					newGravityScale = baseGravityScale * flipGravityScale;
					state = MovementState.STANDARD;
				}
				break;

			case MovementState.AIR_JUMPING:
				Jump(0.8f);
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
            newVelocity = new Vector2(spriteDirection * dashSpeed * flipGravityScale, -rigidBody.velocity.y);
            newGravityScale = 0;
        }
        else
        {
            state = MovementState.STANDARD;
            newGravityScale = baseGravityScale * flipGravityScale;
            newVelocity.x = 0;
        }
    }

    private void WallClingingState()
	{
        HandleHorizontalInput();

        wallJumpDirection = wallTrigger;

        if (rigidBody.velocity.y * Math.Sign(newGravityScale) <= 0)
            newGravityScale = baseGravityScale * WALL_SLIDE_GRAVITY_SCALE_MULTIPLIER * flipGravityScale;

        if (Input.GetKeyDown(jumpKey))
        {
            rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            newGravityScale = baseGravityScale * JUMPING_GRAVITY_SCALE_MULTIPLIER * flipGravityScale;
            wallJumpTime = Time.time;
            jumpTime = Time.time;
            WallJumpingState();

            hasDashed = false;
            hasAirJumped = false;

            state = MovementState.WALL_JUMPING;

            return;
        }
        else if (Math.Abs(rigidBody.velocity.y) <= 6 && wallTrigger == -Math.Sign(newVelocity.x * flipGravityScale)) //wallcling if input towards wall
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
        if (Math.Abs(horizontalInput) >= 0.3)
            newVelocity = new Vector2(wallJumpDirection * dashSpeed, jumpSpeed * 0.64f) * flipGravityScale; //jumpSpeed * 0.64f * Math.Sign(newGravityScale)
        else
            newVelocity = new Vector2(wallJumpDirection * movementSpeed, jumpSpeed * 0.75f) * flipGravityScale; // jumpSpeed * 0.75f * Math.Sign(newGravityScale)
        state = MovementState.STANDARD;
        isVelocityDirty = true;
    }

    private void GrapplingState()
	{

	}

    private void DamagedState() //Fix this shit
    {
        hasAirJumped = false;
        hasDashed = false;
    }

    void FixedUpdate()
	{
        rigidBody.AddForce(new Vector2(0, -rigidBody.velocity.y * rigidBody.mass * 2));
        if (Math.Sign(rigidBody.gravityScale) == 1 && rigidBody.velocity.y <= -maxVelocityY || Math.Sign(rigidBody.gravityScale) == -1 && rigidBody.velocity.y >= maxVelocityY)
        {
            maxVelocityFix = 0.8f;
        }

        else
        {
            maxVelocityFix = 1f;
        }
        // decreases horizontal speed in air while falling ( I think?)
        if (!isGrounded && Math.Sign(newVelocity.x) != Math.Sign(rigidBody.velocity.x))
        {
            newVelocity.x *= 0.5f;
        }
        
        if (newVelocity.x == 0 && state != MovementState.DASHING && state != MovementState.JUMPING) //Check speed issues, also latency
        {
            rigidBody.AddForce(new Vector2(-rigidBody.velocity.x * rigidBody.mass, 0), ForceMode2D.Impulse);
        }
        else /*if (Math.Abs(rigidBody.velocity.x) <= movementSpeed || Math.Sign(newVelocity.x) != Math.Sign(rigidBody.velocity.x))*/
        {
            rigidBody.AddForce(new Vector2(newVelocity.x - rigidBody.velocity.x, -rigidBody.velocity.y * (1 - maxVelocityFix)), ForceMode2D.Impulse);
        }

        if (Math.Abs(newVelocity.y) > 0) //Might have to multiply by flipGravityScale instead
        {
            rigidBody.AddForce(new Vector2(0, newVelocity.y), ForceMode2D.Impulse);
            newVelocity.y = 0;
        }
        //rigidBody.velocity = new Vector2(newVelocity.x, newVelocity.y * maxVelocityFix); // Ta inn maxVelocityFix, mye renere
        isVelocityDirty = false;
		if (rigidBody.gravityScale != newGravityScale)
			rigidBody.gravityScale = newGravityScale;
        //Legg til air drag
        //Gjør at når du prøver å endre retning i luften, så blir kraften mot deg ganget med en konstant,
        //slik at man ikke kan endre retning umiddelbart
	}

    private void HandleHorizontalInput()
    {
        if (Math.Abs(Input.GetAxis("Horizontal")) <= 0.1) //Beholde?
        {
            horizontalInput = 0;
        }
        else if (Math.Abs(horizontalInput) <= Math.Abs(Input.GetAxis("Horizontal")))
        {
            horizontalInput = Input.GetAxis("Horizontal");
            if (Math.Abs(horizontalInput) > HORIZONTAL_INPUT_RUNNING_THRESHOLD)
            {
                newVelocity.x = Math.Sign(horizontalInput) * movementSpeed * flipGravityScale; // Set horizontalInput to max
            }
            else
            {
                newVelocity.x = horizontalInput * movementSpeed * flipGravityScale;
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
        newVelocity.y = (jumpSpeed * scale + Math.Abs(rigidBody.velocity.y)) * flipGravityScale;
        isVelocityDirty = true;
        jumpTime = Time.time;
    }

    //VIDERE ER DET BARE GETTERS

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

    public float GetHorizontalInputRaw()
    {
        return horizontalInput;
    }

    public float GetHorizontalInput()
    {
        return horizontalInput * flipGravityScale;
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
            newGravityScale = baseGravityScale * flipGravityScale;
        }
    }

    public void SetWallTrigger(int trigger)
    {
        wallTrigger = trigger;
    }

    public void SetFlipGravity() //Ta en titt på dette senere
    {
        flipGravityScale *= -1;
        if (flipGravityScale == 1)
        {
            newGravityScale *= -1;
        }
        else
        {
            newGravityScale *= flipGravityScale;
        }
    }

    public int GetFlipGravity()
    {
        return flipGravityScale;
    }

    public float GetGravityScale()
    {
        return newGravityScale;
    }
}
