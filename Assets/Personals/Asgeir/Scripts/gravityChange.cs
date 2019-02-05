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

    void flipGravity()
    {
        movement.ourGravity *= -1;
        TF_character.Rotate(new Vector3(0, 0, 180));

        if (this.tag == "Player")
        {
            StartCoroutine(RotateCamera(duration));
        }
    }

    void Start()
    {

    }

    void Update()
    {
        if (movement.isGrounded) //Should
        {
            if (Input.GetKey(KeyCode.F))
            {
                flipGravity();
            }
        }
    }

    private IEnumerator RotateCamera(float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            TF_camera.transform.rotation = Quaternion.Slerp(TF_camera.transform.rotation, transform.rotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        TF_camera.transform.rotation = transform.rotation;
    }
}
