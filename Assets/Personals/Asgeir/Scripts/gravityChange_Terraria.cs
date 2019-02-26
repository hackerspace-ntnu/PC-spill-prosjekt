using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class gravityChange_Terraria : MonoBehaviour {
    public Movement movement;
    public Transform TF_camera;

    //Rotation variables
    private bool rotate = false;

    void flipGravity()
    {
        movement.ourGravity *= -1;
        transform.Rotate(new Vector3(0, 0, 180));

        if (this.tag == "Player")
        {
            rotate = true;
        }
    }

    void Update()
    {
        //if (movement.isGrounded) //Should
        //{
            if (Input.GetKeyDown(KeyCode.F))
            {
                flipGravity();
            }
        //}

        //Rotation for player

        if (rotate)
        {
            TF_camera.transform.Rotate(new Vector3(0, 180, 180));
            rotate = false;
        }
    }
}
