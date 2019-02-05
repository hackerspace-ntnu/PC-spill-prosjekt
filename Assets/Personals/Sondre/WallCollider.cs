using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollider : MonoBehaviour {

    private MovementV2 playerMovement;

    // Use this for initialization
    void Start()
    {
        playerMovement = transform.parent.GetComponent<MovementV2>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        playerMovement.GetComponent<MovementV2>().wallCollision = true;
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        playerMovement.GetComponent<MovementV2>().wallCollision = false;
    }
}
