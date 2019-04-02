using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{

    private Movement playerMovement;

    // Use this for initialization
    void Start()
    {
        playerMovement = transform.parent.GetComponent<Movement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().tag == "Standard")
        {
            if (this.gameObject.name == "Ground Trigger")
            {
                playerMovement.GetComponent<Movement>().isGrounded = true;
            }


            else if (this.gameObject.name == "Wall Trigger Left")
            {
                playerMovement.GetComponent<Movement>().wallTrigger = 1;
                playerMovement.GetComponent<Movement>().wallHit = true;
            }
            else
            {
                playerMovement.GetComponent<Movement>().wallTrigger = -1;
                playerMovement.GetComponent<Movement>().wallHit = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (this.gameObject.name == "Ground Trigger")
        {
            playerMovement.GetComponent<Movement>().isGrounded = false;
        }
        else
        {
            playerMovement.GetComponent<Movement>().wallHit = false;
        }
    }

}
