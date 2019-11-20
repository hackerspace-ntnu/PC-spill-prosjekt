using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    [SerializeField]
    private PlayerController controller;

    private int CeilingCount = 0;   // Keep track of number of objects above player when crouching.

    // Use this for initialization
    void Start()
    {
        controller = transform.parent.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Collider2D>().tag == "Standard" && this.gameObject.tag == "CeilingDetector") {
            CeilingCount++;
            controller.CanUncrouch = false;
        }

    }

    {
        if (collision.GetComponent<Collider2D>().tag == "Standard")
        {
            if (this.gameObject.name == "Ground Trigger")
            {
                controller.Grounded = true;
            }
            else if (this.gameObject.name == "Wall Trigger Left")
            {
                controller.GetCurrentState().WallTrigger = 1;
            }
            else
            {
                controller.GetCurrentState().WallTrigger = -1;
            }
            print("ENTER" + this.gameObject.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<Collider2D>().tag == "Standard" && this.gameObject.tag == "CeilingDetector") {
            CeilingCount--;

            if(CeilingCount <= 0) {
                controller.CanUncrouch = true;
            }
        }

        {
            controller.GetCurrentState().Grounded = true;
        }
        else
        {
            controller.GetCurrentState().WallTrigger = 0; //Must set this globally, not just for the current state
        }
        print("EXIT " + this.gameObject.name);
    }
}
