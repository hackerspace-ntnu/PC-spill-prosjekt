using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravityChange : MonoBehaviour {

    public MovementV2 movement;
    public Transform TF_character;
    public Transform TF_camera;

    void flipGravity()
    {

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (movement.isGrounded)
        {
            if (Input.GetKey(KeyCode.F))
            {
                movement.ourGravity *= -1;
                TF_camera.Rotate(new Vector3(0,0,180));
                TF_character.Rotate(new Vector3(0,0,180));
            }
        }
	}
}
