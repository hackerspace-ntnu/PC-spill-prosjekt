using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class Patroling : MonoBehaviour
{

    public Transform[] points; // Array of points where the enemy should go
    private int destPoint = 0; // Index of current destination in points
    private Animator animator;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Vector2 currentPosition;
    private Vector2 destinationPosition; 
    public float moveTime = 1f; // How fast do you move
    public float vicinityMargin = 0.01f; // How close is close enough to a point
    //private float inverseMoveTime; 



    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        currentPosition = transform.position;
        destinationPosition = points[destPoint].position;
        //inverseMoveTime = 1 / moveTime;
    }

    void Update()
    {
        if (Mathf.Abs(currentPosition.x - destinationPosition.x) <= vicinityMargin) // If the enemy is close enough to the point according to vicinitymargin
        {
            GotoNextPoint();
        }

        AttemptMove();
        Debug.Log(currentPosition.x - destinationPosition.x);
    }


    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
        destinationPosition = points[destPoint].position;
    }

    void AttemptMove()
    {
        bool right = CheckDirection();

        if (!CheckObstacle(right)) // if there is nothing in the pathway 
        {
            //Vector2 newPosition = Vector2.MoveTowards(currentPosition, new Vector2(destinationPosition.x, currentPosition.y), inverseMoveTime * Time.deltaTime);
            //rb.MovePosition(newPosition);
            if (right) // If destination is to the right
                rb.velocity = new Vector2(moveTime, 0); // move right
            else
                rb.velocity = new Vector2(-moveTime, 0); // move left
            currentPosition = transform.position; // reset currentPosition
        }
    }

    bool CheckDirection() // Checks which direction the enemy is facing. True = right, false = left
    {
        if (currentPosition.x - destinationPosition.x < 0)
            return true;
        else
            return false;
    }

    bool CheckObstacle(bool right) // Checks if something is in the way of the enemy's route. Can be changed to point to point.
    {
        Vector2 destination;
        if (right)
        {
            destination = currentPosition + new Vector2(moveTime, 0);
        }
        else
        {
            destination = currentPosition - new Vector2(moveTime, 0);
        }

        boxCollider.enabled = false; // So that boxcollider doesn't interfere
        RaycastHit2D hit = Physics2D.Linecast(currentPosition, destination); // Need layer check here
        boxCollider.enabled = true;
        if (hit.collider == null) // If nothing is detected by ray
        {
            return false; 
        }
        Debug.Log("Obstacle in the pathway");
        return true; 
    }
}
