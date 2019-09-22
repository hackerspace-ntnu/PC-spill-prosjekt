﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Cinemachine;


public class gravityChange : MonoBehaviour
{
    public CinemachineVirtualCamera CM_camera;

    public Movement movement;
    public Transform TF_camera;

    public float duration = 1f;

    //Rotation variables
    private bool rotate = false;
    public float elapsed = 0.0f;

    private bool wait = false;

    void flipGravity()
    {
        movement.SetFlipGravity();
        transform.Rotate(new Vector3(0, 0, 180));

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
        if (movement.GetGrounded()) //Should
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                flipGravity();
            }
        }

        //Rotation for player
        if (wait)
        {
            CM_camera.Follow = transform;
            wait = false;
        }

        if (rotate)
        {
            if (duration != 0)
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
            else
            {
                CM_camera.Follow = null;

                rotate = false;

                TF_camera.transform.Rotate(new Vector3(0,0,180));
                TF_camera.transform.position = new Vector3(transform.position.x + (transform.position.x - TF_camera.transform.position.x), transform.position.y + (transform.position.y - TF_camera.transform.position.y), TF_camera.position.z);

                wait = true;
            }
        }
    }
}