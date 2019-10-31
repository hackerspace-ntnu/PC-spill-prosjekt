using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    public float horizontalInputRunningThreshold = 0.3f; // Minimum input needed to move left/right
    public float movementSpeed = 7;
    private int flipGravityScale = 1;

    protected float horizontalInput; // input from controller in x-axis
    protected Vector2 newVelocity; // for setting velocity in FixedUpdate()

    protected PlayerController controller;
    protected Rigidbody2D rigidBody;

    public abstract string Name { get; }

    public virtual void Init(PlayerController controller)
    {
        this.controller = controller;
        rigidBody = controller.GetComponent<Rigidbody2D>();
    }

    public abstract void Enter();

    public virtual void Update() {}

    public virtual void FixedUpdate() {}

    public abstract void Exit();

    protected void HandleHorizontalInput()
    {
        if (Math.Abs(Input.GetAxis("Horizontal")) <= 0.1) //Beholde?
        {
            horizontalInput = 0;
        }
        else if (Math.Abs(horizontalInput) <= Math.Abs(Input.GetAxis("Horizontal")))
        {
            horizontalInput = Input.GetAxis("Horizontal");
            if (Math.Abs(horizontalInput) > horizontalInputRunningThreshold)
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
}
