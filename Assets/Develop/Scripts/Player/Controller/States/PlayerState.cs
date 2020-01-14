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
    protected int flipGravityScale = 1;
    protected float baseMaxVelocityY = 12;
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
        if (Math.Sign(rigidBody.gravityScale) == 1 && rigidBody.velocity.y <= -maxVelocityY || 
            Math.Sign(rigidBody.gravityScale) == -1 && rigidBody.velocity.y >= maxVelocityY) {
            maxVelocityFix = 0.2f;
        } else {
            maxVelocityFix = 0f;
        }

        float newVelocityX = controller.TargetVelocity.x - rigidBody.velocity.x;
        float newVelocityY = controller.TargetVelocity.y - rigidBody.velocity.y * maxVelocityFix;

        rigidBody.AddForce(new Vector2(newVelocityX, newVelocityY), ForceMode2D.Impulse);
        controller.TargetVelocity = Vector2.zero;
    }

    public virtual void Exit() {}

    protected void HandleHorizontalInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        controller.TargetVelocity = new Vector2(horizontalInput * movementSpeed * flipGravityScale, controller.TargetVelocity.y);

        /*if (Math.Abs(Input.GetAxis("Horizontal")) <= 0.1) //Beholde?
        {
            horizontalInput = 0;
        } else if (Math.Abs(horizontalInput) <= Math.Abs(Input.GetAxis("Horizontal"))) {
            horizontalInput = Input.GetAxis("Horizontal");
            if (Math.Abs(horizontalInput) > horizontalInputRunningThreshold) {
                targetVelocity.x = Math.Sign(horizontalInput) * movementSpeed * flipGravityScale; // Set horizontalInput to max
            } else {
                targetVelocity.x = horizontalInput * movementSpeed * flipGravityScale;
            }
        } else {
            targetVelocity.x = 0;
            horizontalInput = Input.GetAxis("Horizontal");
        }*/
    }

    public virtual void Jump() { }

    public virtual void Crouch() { }

    public float GetXVelocity() {
        return rigidBody.velocity.x;
    }
    public virtual float getHorizontalInput()
    {
        return horizontalInput;
    }
}
