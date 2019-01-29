using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetectorV2 : MonoBehaviour
{
    private MovementV2 playerMovement;
    private GameObject groundObject;
    //Sjekke om groundObject er faktisk bakke (og ikke f.eks vann)

    void Start()
    {
        playerMovement = transform.parent.GetComponent<MovementV2>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        print(gameObject.name);
        playerMovement.GetComponent<MovementV2>().isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        playerMovement.GetComponent<MovementV2>().isGrounded = false;
    }
}
