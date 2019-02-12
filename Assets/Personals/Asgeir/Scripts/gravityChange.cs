using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class gravityChange : MonoBehaviour
{

    public MovementV2 movement;
    public Transform TF_character;
    public Transform TF_camera;

    public float duration = 1f;

    //Rotation variables
    private bool rotate = false;
    public float elapsed = 0.0f;


    void flipGravity()
    {
        movement.ourGravity *= -1;
        TF_character.Rotate(new Vector3(0, 0, 180));

        if (this.tag == "Player")
        {
            elapsed = 0.0f;
            rotate = true;
        //This can be optimized later
            /*if (rotate)
                elapsed = 1 - elapsed;
            else
            {
                elapsed = 0.0f;
                rotate = true;
            }*/
        }
    }

    void Start()
    {

    }

    void Update()
    {
        if (movement.isGrounded) //Should
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                flipGravity();
            }
        }

        //Rotation for player
        if (rotate)
        {
            if (elapsed < duration)
            {
                TF_camera.transform.rotation = Quaternion.Slerp(TF_camera.transform.rotation, transform.rotation, elapsed / duration);
                elapsed += Time.deltaTime;
            }
            else
            {
                TF_camera.transform.rotation = transform.rotation;
                elapsed = 0.0f;
                rotate = false;
            }
        }
    }
}
