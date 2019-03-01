using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatCollider : MonoBehaviour {

    private Movement playerMovement;

    public bool isColliding = false;
    private int gravitySign;

    void Start()
    {
        playerMovement = transform.parent.GetComponent<Movement>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        isColliding = true;
        playerMovement.GetComponent<Movement>().dashing = false;

        Collider2D collider = collision.collider;
        float RectWidth = GetComponent<BoxCollider2D>().bounds.size.x;
        float RectHeight = GetComponent<BoxCollider2D>().bounds.size.y;

        Vector3 contactPoint = collision.GetContact(0).point;
        Vector3 center = transform.parent.position;

        gravitySign = System.Math.Sign(playerMovement.GetComponent<Movement>().ourGravity);

        /*
        print(RectWidth);
        print(RectHeight);
        print(contactPoint);
        print(center);
        print("");
        */

        if (contactPoint.y * gravitySign < center.y - RectHeight / 2 && playerMovement.GetComponent<Movement>().rb.velocity.y <= 0 &&
            (contactPoint.x < center.x + RectWidth / 2 || contactPoint.x > center.x - RectWidth / 2))
        {
            playerMovement.GetComponent<Movement>().isGrounded = true;
        }
        /*
        else if (contactPoint.x != center.x &&
            (contactPoint.y < center.y + RectHeight / 2 || contactPoint.y > center.y - RectHeight / 2))
        {
            playerMovement.GetComponent<Movement>().wallCollision = true;
        }*/

        else if (contactPoint.y * gravitySign > center.y - RectHeight / 2 && playerMovement.GetComponent<Movement>().rb.velocity.y <= 0 &&
            (contactPoint.x < center.x + RectWidth / 2 || contactPoint.x > center.x - RectWidth / 2))
        {
            playerMovement.GetComponent<Movement>().roofHit = true;
        }

        /*
        else if (contactPoint.x < center.x &&
            (contactPoint.y < center.y + RectHeight / 2 || contactPoint.y > center.y - RectHeight / 2))
        {
            playerMovement.GetComponent<Movement>().wallCollision = true;
        }

        
        */
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isColliding = false;
        playerMovement.GetComponent<Movement>().isGrounded = false;
        //playerMovement.GetComponent<Movement>().wallCollision = false;
        playerMovement.GetComponent<Movement>().roofHit = false;
        GetComponentInParent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
