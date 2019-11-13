using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    [SerializeField]
    private PlayerController controller;

    // Use this for initialization
    void Start()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().tag == "Standard")
        {
            if (this.gameObject.name == "Ground Trigger")
            {
                controller.GetCurrentState().Grounded = true;
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

    private void OnTriggerExit2D(Collider2D col)
    {
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
