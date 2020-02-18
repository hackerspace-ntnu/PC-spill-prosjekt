using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitController : MonoBehaviour
{
    public GameObject spitGooBot;
    public GameObject spitGooTop;
    public GameObject spitGooSide;

    public float spitSpeed;
    public float scale;
    public Vector2 target;

    private Vector2 direction;
    private Rigidbody2D rb;
    private float angle;
    private Vector2 lastPos;

    private bool firstFrame = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = target - (Vector2)transform.position;
        direction.Normalize();
        transform.localScale *= scale;
        rb.AddForce(direction * spitSpeed);
    }

    private void Update()
    {
        lastPos = transform.position;
        direction = rb.velocity;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (!firstFrame)
        {
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        firstFrame = false;
    }

    // Handle collisions with different objects
    private void OnTriggerEnter2D(Collider2D col)
    {
        // Handle player damage / knockback / glitchmeter
        if (col.tag == "Player")
        {
            print("Hit Player!");
        }

        // HANDLES ROOM COLLISIOINS. SPAWNS ACID POOLS
        
        // Handle ground collision
        else if (col.tag == "Ground") 
        {
            Bounds bnd = col.bounds;

            Vector2 closestPoint = bnd.ClosestPoint(lastPos);

            GameObject goo = Instantiate(spitGooBot, closestPoint, Quaternion.identity);
            goo.transform.localScale *= scale;
            Destroy(gameObject);
        }

        // Handle roof collision
        else if (col.tag == "Roof")
        {
            Bounds bnd = col.bounds;

            Vector2 closestPoint = bnd.ClosestPoint(lastPos);

            GameObject goo = Instantiate(spitGooTop, closestPoint, Quaternion.identity);
            goo.transform.localScale *= scale;
            Destroy(gameObject);
        }

        // Handle wall collision
        else if (col.tag == "Wall")
        {
            // Finds the closes edge point on the collision.
            Bounds bnd = col.bounds;
            Vector2 closestPoint = bnd.ClosestPoint(lastPos);

            GameObject goo = Instantiate(spitGooSide, closestPoint, Quaternion.identity);
            goo.transform.localScale *= scale;
                
            if (lastPos.x > col.transform.position.x)
            {
                goo.transform.localScale = new Vector3(goo.transform.localScale.x*-1, goo.transform.localScale.y, goo.transform.localScale.z);
            }
            Destroy(gameObject);
        }

        
    }
}
