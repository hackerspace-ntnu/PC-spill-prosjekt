using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour
{

    private Movement playerMovement;

    // Use this for initialization
    void Start()
    {
        playerMovement = transform.parent.GetComponent<Movement>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        playerMovement.GetComponent<Movement>().wallHit = true;

        if (this.gameObject.name == "Wall Trigger Left")
        {
            playerMovement.GetComponent<Movement>().wallTrigger = 1;
        }
        else
        {
            playerMovement.GetComponent<Movement>().wallTrigger = -1;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        playerMovement.GetComponent<Movement>().wallHit = false;
    }

}
