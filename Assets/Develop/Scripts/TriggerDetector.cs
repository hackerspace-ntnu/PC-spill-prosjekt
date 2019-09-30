using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{

    private PlayerModel playerMovement;

    // Use this for initialization
    void Start()
    {
        playerMovement = GameObject.Find("Models").GetComponent<PlayerModel>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().tag == "Standard")
        {
            if (this.gameObject.name == "Ground Sensor")
            {
                playerMovement.IsGrounded = true;
            }


            else if (this.gameObject.name == "Left Sensor")
            {
                playerMovement.WallTrigger = 1;
            }
            else
            {
                playerMovement.WallTrigger = -1;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (this.gameObject.name == "Right Sensor")
        {
            playerMovement.IsGrounded = false;
        }
        else
        {
            playerMovement.WallTrigger = 0;
        }
    }
}
