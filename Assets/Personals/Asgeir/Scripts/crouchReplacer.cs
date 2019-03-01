using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crouchReplacer : MonoBehaviour {

    public bool isCrouching;

    private void Start()
    {
        isCrouching = false;
    }

    private void Update()
    {
        if (GetComponent<Movement>().isGrounded && Input.GetKey(KeyCode.Z))
        {
            isCrouching = true;
        }
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            isCrouching = false;
        }
    }

}

