using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    GameObject clone;
    Color32 ghostColor = new Color32(100, 100, 100, 200);
    Color32 regularColor;
    bool lagActive = false;

    public void Start()
    {
        regularColor = this.GetComponent<SpriteRenderer>().color;
    }




    public void manageLagMovement(bool on) // i have no access to the current movement script, so i make one here
    {
        // when lag movement turns on, enemy keeps going in its given direction until lag movement is turned off. 
        // When lag movement turns off, enemy returns to its regular movement pattern.

        lagActive = on; // this variable should make direct changes to the movement pattern of enemies
    }
    

    int movementDir = 2; // 2 = left, 3 = right
    Vector3 movement = new Vector3(0.05f, 0, 0);
    public bool moving = true;
    
    public void toggleLagGlitch(bool on)
    {
        this.GetComponent<BoxCollider2D>().enabled = !on;
        if (on)
        {
            clone = Instantiate(this.gameObject); // clone enemy 
            clone.GetComponent<EnemyMovement>().moving = false; // make clone stand still
            this.GetComponent<SpriteRenderer>().color = ghostColor; // give original enemy ghost-color
            moving = false; // regular movement turns off 
            manageLagMovement(true); // turn on lag glitch - movement
        }
        else
        {
            Destroy(clone);
            this.GetComponent<SpriteRenderer>().color = regularColor;
            manageLagMovement(false); // stop lag glitch - movement
            moving = true; // return to original movement 
        }
    }



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
        } else if (lagActive) // don't switch direction, only move
        {
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
