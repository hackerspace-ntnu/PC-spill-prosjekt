using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour {

    private Animator anim;
    private MovementV2 playerMovement;
    private SpriteRenderer spriteRenderer;

    private bool isGrounded;
    private bool wallHit;
    private float moveHorizontal;


    private void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<MovementV2>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        isGrounded = playerMovement.GetComponent<MovementV2>().isGrounded;
        moveHorizontal = playerMovement.GetComponent<MovementV2>().moveHorizontal;
        wallHit = playerMovement.GetComponent<MovementV2>().wallHit;

        anim.SetBool("isGrounded", isGrounded);

        if (wallHit)
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

            if (playerMovement.GetComponent<MovementV2>().wallTrigger == -1)
            {
                spriteRenderer.flipX = true;
            }

            else
            {
                spriteRenderer.flipX = false;
            }

            return;
        }

        if (System.Math.Sign(moveHorizontal) == -1)
        {
            spriteRenderer.flipX = true;
        }

        else if (System.Math.Sign(moveHorizontal) == 1)
        {
            spriteRenderer.flipX = false;
        }

        if (playerMovement.GetComponent<MovementV2>().dashing)
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

        if (playerMovement.GetComponent<MovementV2>().rb.velocity.y * System.Math.Sign(playerMovement.GetComponent<MovementV2>().ourGravity) < -1)
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

        if (playerMovement.GetComponent<MovementV2>().rb.velocity.y * System.Math.Sign(playerMovement.GetComponent<MovementV2>().ourGravity) > 0 && !isGrounded)
        {
            anim.SetBool("isJumping", true);
            anim.SetBool("isGrounded", false);
            return;
        }

        else
        {
            anim.SetBool("isJumping", false);
        }

        if (isGrounded && moveHorizontal != 0)
        {
            anim.SetBool("isRunning", true);
        }

        else
        {
            anim.SetBool("isRunning", false);
        }
    }
}
