using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatCollider : MonoBehaviour {

    private Movement playerMovement;

    public bool isColliding = false;
    private bool hasDashed;
    private int gravitySign;

    void Start()
    {
        playerMovement = transform.parent.GetComponent<Movement>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        isColliding = true;
        playerMovement.GetComponent<Movement>().dashing = false;
        hasDashed = playerMovement.GetComponent<Movement>().hasDashed;

        Collider2D collider = collision.collider;
        float rectWidth = GetComponent<BoxCollider2D>().bounds.size.x;
        float rectHeight = GetComponent<BoxCollider2D>().bounds.size.y;

        Vector3 contactPoint1 = collision.GetContact(0).point;
        Vector3 contactPoint2 = collision.GetContact(1).point;
        Vector3 center = transform.parent.position;

        gravitySign = System.Math.Sign(playerMovement.GetComponent<Movement>().ourGravity);

        if (collider.tag == "Standard")
        {
            if (gravitySign == 1)
            {
                if (contactPoint1.y * gravitySign > center.y + rectHeight / 3 && playerMovement.GetComponent<Movement>().rb.velocity.y <= 0 &&
                    (contactPoint1.x < center.x + rectWidth / 2 || contactPoint1.x > center.x - rectWidth / 2))
                {
                    playerMovement.GetComponent<Movement>().roofHit = true;
                }
            }

            else
            {
                if (contactPoint1.y * gravitySign > center.y + rectHeight / 3 && playerMovement.GetComponent<Movement>().rb.velocity.y <= 0 &&
                    (contactPoint1.x < center.x + rectWidth / 2 || contactPoint1.x > center.x - rectWidth / 2))
                {
                    playerMovement.GetComponent<Movement>().roofHit = true;
                }
            }
            print(contactPoint1);
            print(contactPoint2);
            print(center);  
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isColliding = false;
        playerMovement.GetComponent<Movement>().roofHit = false;
        GetComponentInParent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
