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
                playerMovement.SetGrounded(true);
            }


            else if (this.gameObject.name == "Wall Trigger Left")
            {
                playerMovement.SetWallTrigger(1);
            }
            else
            {
                playerMovement.SetWallTrigger(-1);
            }
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
            playerMovement.SetWallTrigger(0);
        }
    }
}
