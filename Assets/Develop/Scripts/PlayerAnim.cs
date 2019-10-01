using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour {

    private Animator anim;
    private Rigidbody2D body;
    private PlayerModel playerMovement;
    private HorizontalDirectionController horizontalDirectionController;
    private SpriteRenderer spriteRenderer;
    private Vector2 rigidBodyVelocity;


    private bool isGrounded;
    private int playerState;
    private int wallTrigger;
    private int flipGravityScale;
    private float moveHorizontal;


    private void Start()
    {
        horizontalDirectionController = GameObject.Find("Controllers").GetComponent<HorizontalDirectionController>();
        anim = GetComponent<Animator>();
        playerMovement = GameObject.Find("Models").GetComponent<PlayerModel>();
        spriteRenderer = GameObject.Find("View").GetComponent<SpriteRenderer>();
        body = GameObject.Find("View").GetComponent<Rigidbody2D>();
        if (spriteRenderer.flipX)
            playerMovement.SpriteDirection = -1;
        else
            playerMovement.SpriteDirection = 1;
    }

    private void Update()
    {
        isGrounded = playerMovement.IsGrounded;
        moveHorizontal = horizontalDirectionController.LastInput;
        wallTrigger = playerMovement.WallTrigger;
        playerState = (int)playerMovement.MoveState;
        flipGravityScale = playerMovement.FlipGravityScale;

        anim.SetBool("isGrounded", isGrounded);

        rigidBodyVelocity = body.velocity;

        if (wallTrigger != 0 && !isGrounded)
        {
            /*
            anim.SetBool("wallHit", true);
            anim.SetBool("isJumping", false);
            anim.SetBool("isDashing", false);
            */

            anim.SetBool("isDashing", true);
            anim.SetBool("isRunning", false);
            anim.SetBool("isJumping", false);
            anim.SetBool("isGrounded", false);

            if (wallTrigger == -1)
            {
                spriteRenderer.flipX = true;
                playerMovement.SpriteDirection = -1;
            }

            else
            {
                spriteRenderer.flipX = false;
                playerMovement.SpriteDirection = 1;
            }

            return;
        }

        else if (System.Math.Sign(moveHorizontal) * flipGravityScale == -1 && playerState != 3)
        {
            spriteRenderer.flipX = true;
            playerMovement.SpriteDirection = -1;
        }

        else if (System.Math.Sign(moveHorizontal) * flipGravityScale == 1 && playerState != 3)
        {
            spriteRenderer.flipX = false;
            playerMovement.SpriteDirection = 1;
        }

        if (playerState == 3)
        {
            anim.SetBool("isDashing", true);
            anim.SetBool("isRunning", false);
            anim.SetBool("isJumping", false);
            anim.SetBool("isGrounded", false);
            return;
        }
        else
        {
            anim.SetBool("isDashing", false);
        }

        if (rigidBodyVelocity.y * System.Math.Sign(playerMovement.NewGravityScale) < -1)
        {
            /*
            anim.SetBool("isFalling", true);
            anim.SetBool("isJumping", false);
            */
            anim.SetBool("isDashing", true);
            anim.SetBool("isRunning", false);
            anim.SetBool("isJumping", false);
            anim.SetBool("isGrounded", false);
        }

        if (rigidBodyVelocity.y * System.Math.Sign(playerMovement.NewGravityScale) > 0 && !isGrounded)
        {
            anim.SetBool("isJumping", true);
            anim.SetBool("isGrounded", false);
            return;
        }

        else
        {
            anim.SetBool("isJumping", false);
        }

        if (rigidBodyVelocity.x != 0 && moveHorizontal != 0)
        {
            anim.SetBool("isRunning", true);
        }

        else
        {
            anim.SetBool("isRunning", false);
        }
    }
}
