using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{

    public float baseGravityScale = 5; // base gravity affecting the player
    public float movementSpeed = 7;
    protected int flipGravityScale = 1;
    private float maxVelocityY = 12;
    private float maxVelocityFix;
    protected float groundJumpSpeed = 13.5f;
    protected float airJumpSpeed = 11.5f;

    protected float horizontalInput; // input from controller in x-axis
    

    protected PlayerController controller;
    protected Rigidbody2D rigidBody;

    public abstract string Name { get; }

    public virtual void Init(PlayerController controller)
    {
        this.controller = controller;
        rigidBody = controller.GetComponent<Rigidbody2D>();
        rigidBody.gravityScale = baseGravityScale;
    }

    public virtual void Enter() {}

    public virtual void Update() {
        HandleHorizontalInput();
    }

    public virtual void FixedUpdate() {

        if (Math.Sign(rigidBody.gravityScale) == 1 && rigidBody.velocity.y <= -maxVelocityY || 
            Math.Sign(rigidBody.gravityScale) == -1 && rigidBody.velocity.y >= maxVelocityY) {
            maxVelocityFix = 0.8f;
        } else {
            maxVelocityFix = 1f;
        }

        // decreases horizontal acceleration in air while input in opposite direction of velocity
        if (!controller.Grounded && Math.Sign(controller.TargetVelocity.x) != Math.Sign(rigidBody.velocity.x)) {
            controller.TargetVelocity = new Vector2 (controller.TargetVelocity.x * 0.5f, controller.TargetVelocity.y);
        }

        float newVelocityX = controller.TargetVelocity.x - rigidBody.velocity.x;
        float newVelocityY = controller.TargetVelocity.y - rigidBody.velocity.y * (1 - maxVelocityFix);

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
}
