﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour {

    public GameObject objToFollow;
    public Vector2 targetPos;

    public float distanceFactor;
    public float twitchFactor;
    public float twitchTolerance;

    private Rigidbody2D thisRBody;
    private Rigidbody2D rBodyOfObj;
    private Vector3 worldTargetPos;
    private Vector2 distanceToTarget;
    private Vector2 defaultTarget;

	// Use this for initialization
	void Start () {

        // Initialize object to follow. Set to parent object if not set in editor.
        if(objToFollow == null) {
            objToFollow = transform.parent.gameObject;
        }
        rBodyOfObj = objToFollow.GetComponent<Rigidbody2D>();
        thisRBody = GetComponent<Rigidbody2D>();
        worldTargetPos = new Vector3(objToFollow.transform.position.x + targetPos.x, objToFollow.transform.position.y + targetPos.y, 0.0f);
        transform.position = worldTargetPos;
        defaultTarget = targetPos;
	}
	
	// Update is called once per frame
	void Update () {
        // Set the target for the familiar in world coordinates.
        worldTargetPos = new Vector3(objToFollow.transform.position.x + targetPos.x, objToFollow.transform.position.y + targetPos.y, 0.0f);
        
        // If player/object is moving to the left, place familiar on the right of player/object.
        if(rBodyOfObj.velocity.x < 0.0f) {
            if(targetPos.x < 0.0f) {
                targetPos = new Vector2(targetPos.x * -1.0f, targetPos.y);
            }
        }

        // If player/object is moving to the right, place familiar on the left of player/object.
        if (rBodyOfObj.velocity.x > 0.0f) {
            if (targetPos.x > 0.0f) {
                targetPos = new Vector2(targetPos.x * -1.0f, targetPos.y);
            }
        }

        // Set random twitch movment when familiar is close to target position.
        if(thisRBody.velocity.magnitude < twitchTolerance) {
            worldTargetPos = new Vector3(worldTargetPos.x + Random.Range(-twitchFactor, twitchFactor), worldTargetPos.y + Random.Range(-twitchFactor, twitchFactor));
        }

    }

    private void FixedUpdate() {
        // Calculate distance to target position and set velocity towards target with speed increasing with distance.
        distanceToTarget = transform.position - worldTargetPos;
        distanceToTarget = distanceToTarget.normalized * Mathf.Pow(distanceToTarget.magnitude, distanceFactor);
        
        thisRBody.velocity = -distanceToTarget;
    }

    public void AttachToObject(GameObject obj, Vector2 target) {
        if(objToFollow != obj) {
            objToFollow = obj;
            rBodyOfObj = objToFollow.GetComponent<Rigidbody2D>();

            targetPos = target;
        }
    }

    public void ResetAttachment() {
        objToFollow = GameObject.FindGameObjectWithTag("Player");
        rBodyOfObj = objToFollow.GetComponent<Rigidbody2D>();

        targetPos = defaultTarget;
    }
}
