using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravityChange : MonoBehaviour {

    public MovementV2 movement;
    public Transform TF_character;
    public Transform TF_camera;

    void flipGravity()
    {
        movement.ourGravity *= -1;
        TF_camera.Rotate(new Vector3(0,0,180));
        TF_character.Rotate(new Vector3(0,0,180));
    }

	void Start () {
		
	}
	
	void Update () {
        if (movement.isGrounded) //Should
        {
            if (Input.GetKey(KeyCode.F))
            {
                flipGravity();
            }
        }
	}
}
