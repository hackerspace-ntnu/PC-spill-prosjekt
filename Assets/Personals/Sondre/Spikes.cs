using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes: MonoBehaviour {

    private Movement playerMovement;
    private GameObject thisCollisionObject;

    private Vector2 contactPoint;
    private Vector2 center;

    private float colTime;

    void Start()
    {
        playerMovement = GameObject.FindWithTag("Player").GetComponent<Movement>();
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionObject = GameObject.Find(collision.collider.name);

        GameObject thisCollisionObject = collisionObject;


        if (collisionObject = GameObject.FindWithTag("Player"))
        {
            print("Yeet");
            playerMovement.GetComponent<Movement>().takingDamage = true;

        }

        Collider2D collider = collision.collider;

        Vector2 contactPoint = collision.GetContact(0).point;
        Vector2 center = transform.parent.position;

        collisionObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-10 * System.Math.Sign(contactPoint.x - center.x), 10);

        colTime = Time.time;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        damageAnim();
    }

    private void damageAnim()
    {
        if (Time.time - colTime <= 1)
        {
            thisCollisionObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-10 * System.Math.Sign(contactPoint.x - center.x), 10) * System.Math.Abs(1 - (Time.time - colTime) / 2);
        }
        else if (thisCollisionObject = GameObject.FindWithTag("Player"))
        {
            print("Yeet");
            playerMovement.GetComponent<Movement>().takingDamage = false;
        }
    }
}
