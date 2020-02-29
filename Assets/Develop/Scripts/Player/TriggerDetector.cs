using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    [SerializeField]
    private PlayerController controller;

    private int ceilingCount = 0;   // Keep track of number of objects above player when crouching.
    private int glitchCeilingCount = 0;   // Keep track of number of objects above player when crouching.

    void Start()
    {
        controller = transform.parent.GetComponent<PlayerController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().tag == "Standard")
        {
            if (this.gameObject.tag == "CeilingDetector") {
                controller.CanUncrouch = false;
                ceilingCount++;
            }

            if(this.gameObject.tag == "GlitchCeilingDetector") {
                controller.CanUnglitch = false;
                glitchCeilingCount++;
            }

            if (this.gameObject.name == "Ground Trigger")
                controller.Grounded = true;
            else if (this.gameObject.name == "Wall Trigger Left")
                controller.WallTrigger = 1;
            else
                controller.WallTrigger = -1;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().tag == "Standard" && this.gameObject.tag == "CeilingDetector") {
            ceilingCount--;

            if (ceilingCount <= 0) {
                controller.CanUncrouch = true;
            }
        }
        
        if (collision.GetComponent<Collider2D>().tag == "Standard" && this.gameObject.tag == "GlitchCeilingDetector") {
            glitchCeilingCount--;

            if (glitchCeilingCount <= 0) {
                controller.CanUnglitch = true;
            }
        }

        if (this.gameObject.name == "Ground Trigger")
            controller.Grounded = false;
        else
            controller.WallTrigger = 0;
    }
}
