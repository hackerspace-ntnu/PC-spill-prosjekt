using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected const float JUMPING_GRAVITY_SCALE = 4f;

    public float baseGravityScale = 5; // base gravity affecting the player
    public float movementSpeed = 6;  // Orig value: 7
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
    protected Rigidbody2D rigidBody;

    public abstract string Name { get; }

    public virtual void Init(PlayerController controller)
    {
        this.controller = controller;
        rigidBody = controller.GetComponent<Rigidbody2D>();
        rigidBody.gravityScale = baseGravityScale;
        maxVelocityY = baseMaxVelocityY;
    }

    public virtual void Enter() {}

    public virtual void Update() {
        HandleHorizontalInput();
        controller.Animator.SetFloat("Hinput", Mathf.Abs(horizontalInput));
    }

    public virtual void FixedUpdate() {
        if (controller.FlipGravityScale == 1 && rigidBody.velocity.y <= -maxVelocityY || 
            controller.FlipGravityScale == -1 && rigidBody.velocity.y >= maxVelocityY) {
            maxVelocityFix = 0.2f;
        } else {
            maxVelocityFix = 0f;
        }

        float newVelocityX = controller.TargetVelocity.x - rigidBody.velocity.x;
        float newVelocityY = controller.TargetVelocity.y - rigidBody.velocity.y * maxVelocityFix;

        rigidBody.AddForce(new Vector2(newVelocityX, newVelocityY), ForceMode2D.Impulse);
        controller.TargetVelocity = new Vector2(controller.TargetVelocity.x, 0);
    }

    public virtual void Exit() {}

    protected void HandleHorizontalInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        controller.TargetVelocity = new Vector2(horizontalInput * movementSpeed * controller.FlipGravityScale, controller.TargetVelocity.y);
    }

    public virtual void Jump() { }

    public virtual void Crouch() { }

    public virtual void Dash() { }

    public virtual void UpdateGravity()
    {
        rigidBody.gravityScale = baseGravityScale * controller.FlipGravityScale;
    }

    public float GetXVelocity() {
        return rigidBody.velocity.x;
    }
    public float getHorizontalInput()
    {
        return horizontalInput;
    }
    
}
