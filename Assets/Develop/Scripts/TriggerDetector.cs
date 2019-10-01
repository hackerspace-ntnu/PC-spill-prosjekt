using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{

    private PlayerModel model;

    // Use this for initialization
    void Start()
    {
        model = GameObject.Find("Models").GetComponent<PlayerModel>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collider2D>().tag == "Standard")
        {
            if (this.gameObject.name == "Ground Sensor")
            {
                model.WallTrigger = 0;
                model.IsGrounded = true;
            }


            else if (this.gameObject.name == "Left Sensor")
            {
                model.WallTrigger = 1;
            }
            else
            {
                model.WallTrigger = -1;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if ((this.gameObject.name == "Right Sensor") || (this.gameObject.name == "Left Sensor"))
        {
            model.IsGrounded = false;
        }
        else
        {
            model.WallTrigger = 0;
        }
    }
}
