using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollAI : MonoBehaviour
{
    // Start is called before the first frame update


    public Vector2[] nodes;
    public float speed, jumpHeight, pauseBetweenNodes, nodeMargin;
    private int currentNodeNum = 0;

    private enum movementState
    {
        walking,
        jumping,
        alert
    }
    enum dirs
    {
        right,
        left
    }
    private void Start()
    {
        lookAt(nodes[currentNodeNum]);
    }

    private dirs movementDirection = dirs.right;
    private movementState currentMovementState, nextMovementState;
    

    private void FixedUpdate()
    {
        switch (currentMovementState)
        {
            case movementState.walking:
                if (checkForObstacles())
                {
                    jump();
                }
                else if (checkforPlayer())
                {
                    attack();
                }
                else
                {
                    strafe();
                }
                break;
            case movementState.jumping:
                break;
            case movementState.alert:
                break;
            default:
                break;
        }
    }

    void strafe()
    {
        transform.Translate(new Vector3(this.speed * Time.fixedDeltaTime * (this.movementDirection == dirs.right ? 1:-1), 0, 0));
        if((transform.position.x - nodes[currentNodeNum].x) * (this.movementDirection == dirs.right ? -1 : 1) < this.nodeMargin)
        {
            currentNodeNum = (currentNodeNum + 1) % nodes.Length;
            lookAt(nodes[currentNodeNum]);
        }
    }

    void attack()
    {

    }
    void jump()
    {

    }
    
    void lookAt(Vector2 point)
    {
        dirs newDir = point.x - transform.position.x > 0 ? dirs.right : dirs.left;
        if(newDir != movementDirection)
        {
            transform.localScale = new Vector3(transform.localScale.x*-1, transform.localScale.y, transform.localScale.z);
        }
        movementDirection = newDir;
    }

    bool checkForObstacles()
    {
        return false;
    }

    bool checkforPlayer()
    {
        return false;
    }
}
