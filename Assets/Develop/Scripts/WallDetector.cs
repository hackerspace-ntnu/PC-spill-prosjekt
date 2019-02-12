using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetector : MonoBehaviour {

    private MovementV2 playerMovement;

	// Use this for initialization
	void Start () {
        playerMovement = transform.parent.GetComponent<MovementV2>();
    }

    private void OnTriggerEnter2D(Collider2D col) {
        playerMovement.GetComponent<MovementV2>().wallHit = true;

        if (this.gameObject.name == "Wall Trigger Left")
        {
            playerMovement.GetComponent<MovementV2>().wallTrigger = 1;
        }
        else
        {
            playerMovement.GetComponent<MovementV2>().wallTrigger = -1;
        }

    }

    private void OnTriggerExit2D(Collider2D col) {
        playerMovement.GetComponent<MovementV2>().wallHit = false;
    }

}
