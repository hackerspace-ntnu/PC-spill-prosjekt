using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour {

    private Animator anim;
    private Movement playerMovement;
    private SpriteRenderer spriteRenderer;
    private Vector2 rigidBodyVelocity;


    private bool isGrounded;
    private int playerState;
    private int wallTrigger;
    private int flipGravityScale;
    private float moveHorizontal;


    private void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<Movement>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer.flipX)
            playerMovement.GetComponent<Movement>().spriteDirection = -1;
        else
            playerMovement.GetComponent<Movement>().spriteDirection = 1;
    }

    private void Update()
    {
        isGrounded = playerMovement.GetComponent<Movement>().GetGrounded();
        moveHorizontal = playerMovement.GetComponent<Movement>().GetHorizontalInput();
        wallTrigger = playerMovement.GetComponent<Movement>().GetWallTrigger();
        playerState = playerMovement.GetComponent<Movement>().GetPlayerMovementState();
        flipGravityScale = playerMovement.GetComponent<Movement>().GetFlipGravity();

        anim.SetBool("isGrounded", isGrounded);

        rigidBodyVelocity = playerMovement.GetComponent<Movement>().GetVelocity();

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
                playerMovement.GetComponent<Movement>().spriteDirection = -1;
            }

            else
            {
                spriteRenderer.flipX = false;
                playerMovement.GetComponent<Movement>().spriteDirection = 1;
            }

            return;
        }

        else if (System.Math.Sign(moveHorizontal) * flipGravityScale == -1 && playerState != 3)
        {
            spriteRenderer.flipX = true;
            playerMovement.GetComponent<Movement>().spriteDirection = -1;
        }

        else if (System.Math.Sign(moveHorizontal) * flipGravityScale == 1 && playerState != 3)
        {
            spriteRenderer.flipX = false;
            playerMovement.GetComponent<Movement>().spriteDirection = 1;
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

        if (rigidBodyVelocity.y * System.Math.Sign(playerMovement.GetComponent<Movement>().GetGravityScale()) < -1)
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

        if (rigidBodyVelocity.y * System.Math.Sign(playerMovement.GetComponent<Movement>().GetGravityScale()) > 0 && !isGrounded)
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
