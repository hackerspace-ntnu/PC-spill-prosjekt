using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour {

    GameObject playerMovement = GameObject.FindWithTag("Player");

    void OnCollisionEnter2D(Collision2D collision)
    {
        playerMovement.GetComponent<Movement>().enabled = false;

        Collider2D collider = collision.collider;

        Vector3 contactPoint = collision.GetContact(0).point;
        Vector3 center = transform.parent.position;

        /*
        print(RectWidth);
        print(RectHeight);
        print(contactPoint);
        print(center);
        print("");
        */

        if (contactPoint.x < center.x)
        {
            
        }

        else
        {

        }
    }
    
}
