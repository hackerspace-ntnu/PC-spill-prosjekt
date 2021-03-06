﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamiliarTwitchController : MonoBehaviour
{
    public FamiliarController scriptRef;
    public float twitchFactor;
    public float twitchTolerance;

    private Rigidbody2D pRBody;
    private GameObject objToFollow;

    void Start()
    {
        pRBody = GetComponentInParent<Rigidbody2D>();
    }

    void Update()
    {
        objToFollow = scriptRef.GetTarget();

        // Set random twitch movement when familiar is close to player position.
        if (objToFollow.CompareTag("Player") && pRBody.velocity.magnitude < twitchTolerance) {
            
            if(objToFollow.GetComponent<Rigidbody2D>().velocity.magnitude < 0.01f) {
                transform.localPosition = new Vector3(Random.Range(-twitchFactor, twitchFactor), Random.Range(-twitchFactor, twitchFactor));
            }
        } else {
            transform.localPosition = new Vector3(0.0f, 0.0f);
        }
    }
}
