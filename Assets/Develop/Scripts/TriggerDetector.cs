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
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Collider2D>().tag == "Standard" && this.gameObject.tag == "CeilingDetector") {
            CeilingCount++;
            controller.CanUncrouch = false;
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().tag == "Standard")
        {
            if (this.gameObject.name == "Ground Trigger")
            {
                controller.Grounded = true;
                /*model.MoveState = MovementStat.STANDARD;
                model.PlayerInAirState = InAirState.ON_GROUND;
                model.WallTrigger = 0;
                model.IsGrounded = true;*/
            }
            else if (this.gameObject.name == "Left Sensor")
            {
                //model.WallTrigger = 1;
            }
            else
            {
                //model.WallTrigger = -1;
            }
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

        if ((this.gameObject.name == "Ground Sensor"))
        {
            //model.IsGrounded = false;
        }
        else
        {
            //model.WallTrigger = 0;
        }
    }
}
