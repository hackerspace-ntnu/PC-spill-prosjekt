using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour {

    private Animator anim;
    private Movement playerMovement;
    private SpriteRenderer spriteRenderer;

    private bool isGrounded;
    private bool wallHit;
    private bool takingDamage;
    private float moveHorizontal;


    private void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<Movement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        isGrounded = playerMovement.GetComponent<Movement>().isGrounded;
        moveHorizontal = playerMovement.GetComponent<Movement>().moveHorizontal;
        wallHit = playerMovement.GetComponent<Movement>().wallHit;
        takingDamage = playerMovement.GetComponent<Movement>().takingDamage;

        anim.SetBool("isGrounded", isGrounded);

        if (wallHit && !isGrounded)
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

            if (playerMovement.GetComponent<Movement>().wallTrigger == -1)
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

        if (System.Math.Sign(moveHorizontal) == -1)
        {
            spriteRenderer.flipX = true;
            playerMovement.GetComponent<Movement>().spriteDirection = -1;
        }

        else if (System.Math.Sign(moveHorizontal) == 1)
        {
            spriteRenderer.flipX = false;
            playerMovement.GetComponent<Movement>().spriteDirection = 1;
        }

        if (playerMovement.GetComponent<Movement>().dashing)
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

        if (playerMovement.GetComponent<Movement>().rb.velocity.y * System.Math.Sign(playerMovement.GetComponent<Movement>().ourGravity) < -1)
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

        if (playerMovement.GetComponent<Movement>().rb.velocity.y * System.Math.Sign(playerMovement.GetComponent<Movement>().ourGravity) > 0 && !isGrounded)
        {
            anim.SetBool("isJumping", true);
            anim.SetBool("isGrounded", false);
            return;
        }

        else
        {
            anim.SetBool("isJumping", false);
        }

        if (isGrounded && playerMovement.GetComponent<Movement>().velocity.x != 0)
        {
            anim.SetBool("isRunning", true);
        }

        else
        {
            anim.SetBool("isRunning", false);
        }
    }
}
