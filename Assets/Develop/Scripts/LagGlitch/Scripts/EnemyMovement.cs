using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    GameObject clone;
    public void boxColliderToggle(bool turnOn)
    {
        this.GetComponent<BoxCollider2D>().enabled = turnOn;
        if (turnOn)
        {
            //movementDir /= 2; // turn on movement
            this.GetComponent<SpriteRenderer>().enabled = true; // turn on sprite renderer
            Destroy(clone);
            Debug.Log("1");
        }
        else
        {
            //movementDir *= 2; // turn off movement
            clone = Instantiate(this.gameObject);
            this.GetComponent<SpriteRenderer>().enabled = false; // turn off sprite renderer
            clone.GetComponent<EnemyMovement>().moving = false;
            Debug.Log("2");
        }
    }

    int movementDir = 2; // 2 = left, 3 = right
    Vector3 movement = new Vector3(0.05f, 0, 0);
    public bool moving = true;
    public void Update()
    {
        if(moving)
        {
            if (this.transform.position.x < 0)
            {
                movementDir = 3;
            }
            if (this.transform.position.x > 3)
            {
                movementDir = 2;
            }
            if (movementDir == 2)
            {
                this.transform.position -= movement;
            }
            else if (movementDir == 3)
            {
                this.transform.position += movement;
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<PlayerStats>().GetHit(25);
    }
}
