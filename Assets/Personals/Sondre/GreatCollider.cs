using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatCollider : MonoBehaviour {

    private MovementV2 playerMovement;

    public bool isColliding = false;
    private int gravitySign;

    void Start () {
        playerMovement = transform.parent.GetComponent<MovementV2>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        isColliding = true;

        Collider2D collider = collision.collider;
        float RectWidth = GetComponent<BoxCollider2D>().bounds.size.x;
        float RectHeight = GetComponent<BoxCollider2D>().bounds.size.y;

        Vector3 contactPoint = collision.GetContact(0).point;
        Vector3 center = transform.parent.position;

        gravitySign = System.Math.Sign(playerMovement.GetComponent<MovementV2>().ourGravity);

        print(RectWidth);
        print(RectHeight);
        print(contactPoint);
        print(center);
        print("");

        if (contactPoint.y * gravitySign < center.y - RectHeight / 2 && playerMovement.GetComponent<MovementV2>().rb.velocity.y <= 0 &&
            (contactPoint.x < center.x + RectWidth / 2 || contactPoint.x > center.x - RectWidth / 2))
        {
            playerMovement.GetComponent<MovementV2>().isGrounded = true;
        }
        else if (contactPoint.x != center.x &&
            (contactPoint.y < center.y + RectHeight / 2 || contactPoint.y > center.y - RectHeight / 2))
        {
            playerMovement.GetComponent<MovementV2>().wallCollision = true;
        }

        /*
        else if (contactPoint.x < center.x &&
            (contactPoint.y < center.y + RectHeight / 2 || contactPoint.y > center.y - RectHeight / 2))
        {
            playerMovement.GetComponent<MovementV2>().wallCollision = true;
        }

        /*
        if (contactPoint.y > center.y && //checks that circle is on top of rectangle
            (contactPoint.x < center.x + RectWidth / 2 && contactPoint.x > center.x - RectWidth / 2))
        {
            roofHit
        }
        */
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isColliding = false;
        playerMovement.GetComponent<MovementV2>().isGrounded = false;
        playerMovement.GetComponent<MovementV2>().wallCollision = false;

    }
}
