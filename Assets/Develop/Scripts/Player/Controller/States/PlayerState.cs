using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    public abstract string Name { get; }

    protected const float JUMPING_GRAVITY_SCALE = 4f;

    public float baseGravityScale = 5; // base gravity affecting the player
    public float movementSpeed = 6; // Orig value: 7
    protected float dashSpeed = 12;
    protected float baseMaxVelocityY = 8;
    protected float wallSlideMaxVelocityY = 2;
    protected float maxVelocityY;
    protected float maxVelocityFix;
    protected float groundJumpSpeed = 13f;
    protected float airJumpSpeed = 10f;

    protected float horizontalInput; // input from controller in x-axis

    protected bool hasDashed = false;

    protected PlayerController controller;
    protected Rigidbody2D rigidbody;

    public virtual void Init(PlayerController controller)
    {
        this.controller = controller;
        rigidbody = controller.GetComponent<Rigidbody2D>();
        rigidbody.gravityScale = baseGravityScale;
        maxVelocityY = baseMaxVelocityY;
    }

    public virtual void Enter() {}

    public virtual void Update()
    {
        HandleHorizontalInput();
        controller.Animator.SetFloat("Hinput", Mathf.Abs(horizontalInput));

        CheckGrappling();
    }

    protected void CheckGrappling()
    {
        if (Input.GetButtonDown("Grapple"))
            GrapplingState.INSTANCE.FireGrapplingHook();
    }

    public virtual void FixedUpdate()
    {
        if (controller.FlipGravityScale == 1 && rigidbody.velocity.y <= -maxVelocityY
            || controller.FlipGravityScale == -1 && rigidbody.velocity.y >= maxVelocityY)
        {
            maxVelocityFix = 0.2f;
        }
        else
        {
            maxVelocityFix = 0f;
        }

        float newVelocityX = controller.TargetVelocity.x - rigidbody.velocity.x;
        float newVelocityY = controller.TargetVelocity.y - rigidbody.velocity.y * maxVelocityFix;

        rigidbody.AddForce(new Vector2(newVelocityX, newVelocityY), ForceMode2D.Impulse);
        controller.TargetVelocity = new Vector2(controller.TargetVelocity.x, 0);
    }

    public virtual void OnTriggerEnter2D(Collider2D collider) {}

    public virtual void Exit() {}

    protected void HandleHorizontalInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        controller.TargetVelocity = new Vector2(horizontalInput * movementSpeed * controller.FlipGravityScale, controller.TargetVelocity.y);
    }

    public virtual void Jump() {}

    public virtual void Crouch() {}

    public virtual void Dash() {}

    public void OnGrapplingHookHit()
    {
        controller.ChangeNewState(GrapplingState.INSTANCE);
    }

    public virtual void ToggleGlitch() {}

    public virtual void UpdateGravity()
    {
        rigidbody.gravityScale = baseGravityScale * controller.FlipGravityScale;
    }

    public float GetXVelocity()
    {
        return rigidbody.velocity.x;
    }
}
