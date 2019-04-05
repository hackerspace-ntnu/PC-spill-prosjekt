using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{

    private MovementV2 playerMovement;

    // Use this for initialization
    void Start()
    {
        playerMovement = transform.parent.GetComponent<MovementV2>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().tag == "Standard")
        {
            if (this.gameObject.name == "Ground Trigger")
            {
                playerMovement.SetGrounded(true);
            }


            //else if (this.gameObject.name == "Wall Trigger Left")
            //{
            //    playerMovement.wallTrigger = 1;
            //    playerMovement.wallHit = true;
            //}
            //else
            //{
            //    playerMovement.wallTrigger = -1;
            //    playerMovement.wallHit = true;
            //}
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (this.gameObject.name == "Ground Trigger")
        {
            playerMovement.SetGrounded(false);
        }
        else
        {
            //playerMovement.wallHit = false;
        }
    }

}
