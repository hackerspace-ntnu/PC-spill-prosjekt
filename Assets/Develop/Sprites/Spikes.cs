using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes: MonoBehaviour {

    private Movement playerMovement;
    private GameObject thisCollisionObject;

    private Vector2 contactPoint;
    private Vector2 colliderCenter;

    private float colTime;

    void Start()
    {
        playerMovement = GameObject.FindWithTag("Player").GetComponent<Movement>();
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionObject = GameObject.Find(collision.collider.name);

        thisCollisionObject = collisionObject.transform.parent.gameObject;


        if (collisionObject = GameObject.FindWithTag("Player"))
        {
            playerMovement.GetComponent<Movement>().takingDamage = true;
        }

        Collider2D collider = collision.collider;

        contactPoint = collision.GetContact(0).point;
        colliderCenter = collider.transform.position;

        print("colliderCenter" + colliderCenter);
        print("contactPoint" + contactPoint);

        collisionObject.GetComponent<Movement>().velocity = new Vector2(1 * System.Math.Sign(colliderCenter.x - contactPoint.x), 1) * 4;
        collisionObject.GetComponentInChildren<BoxCollider2D>().isTrigger = true;

        colTime = Time.time;

        StartCoroutine(damageAnim());
    }

    private IEnumerator damageAnim()
    {
        print(thisCollisionObject.GetComponent<Rigidbody2D>().velocity);
        var curTime = 0f;

        while (0.15f > curTime)
        {
            
            thisCollisionObject.GetComponent<Movement>().velocity = new Vector2(1 * System.Math.Sign(colliderCenter.x - contactPoint.x), 1) * 4 * System.Math.Abs(1 - curTime * 2);
            print("Nei");
            yield return null;
            curTime += Time.deltaTime;
        }

        if (thisCollisionObject = GameObject.FindWithTag("Player"))
        {
            playerMovement.GetComponent<Movement>().takingDamage = false;
        }
        thisCollisionObject.GetComponentInChildren<BoxCollider2D>().isTrigger = false;
    }
}
