using System.Collections;
using System.Collections.Generic;
using GlobalEnums;
using UnityEngine;

public class FamiliarController : MonoBehaviour {
    
    public GameObject player;
    public SpeechBubble speechBubble;
    public Vector2 targetPos;

    public float distanceFactor;
    public float minVel;

    private GameObject objToFollow;
    private Rigidbody2D thisRBody;
    private Rigidbody2D rBodyOfObj;
    private Vector3 worldTargetPos;
    private Vector2 defaultTarget;

	// Use this for initialization
	void Start () {
        player = transform.parent.gameObject;

        // Initialize object to follow. Set to parent object if not set in editor.
        if(objToFollow == null) {
            objToFollow = player;
        }
        rBodyOfObj = objToFollow.GetComponent<Rigidbody2D>();
        thisRBody = GetComponent<Rigidbody2D>();

        worldTargetPos = new Vector3(objToFollow.transform.position.x + targetPos.x, objToFollow.transform.position.y + targetPos.y, 0.0f);
        transform.position = worldTargetPos;
        defaultTarget = targetPos;
	}
	
	// Update is called once per frame
	void Update () {
        // If player/object is moving to the left, place familiar on the right of player/object.
        if (rBodyOfObj.velocity.x < -0.1f) {
            if (targetPos.x < 0.0f) {
                FlipSide(Direction.LEFT);
            }
        }

        // If player/object is moving to the right, place familiar on the left of player/object.
        if (rBodyOfObj.velocity.x > 0.1f) {
            if (targetPos.x > 0.0f) {
                FlipSide(Direction.RIGHT);
            }
        }

        if(objToFollow.tag != "Player") {
            if(player.transform.position.x < objToFollow.transform.position.x) {
                FlipSide(Direction.LEFT);
            }

            if (player.transform.position.x > objToFollow.transform.position.x) {
                FlipSide(Direction.RIGHT);
            }

        }

        // Set the target for the familiar in world coordinates.
        worldTargetPos = new Vector3(objToFollow.transform.position.x + targetPos.x, objToFollow.transform.position.y + targetPos.y, 0.0f);
        
    }

    private void FixedUpdate() {
        // Calculate distance to target position and set velocity towards target with speed increasing with distance.
        Vector2 distanceToTarget = transform.position - worldTargetPos;
        Vector2 newVelocity = distanceToTarget.normalized * Mathf.Pow(distanceToTarget.magnitude, distanceFactor);
        thisRBody.velocity = -newVelocity;

        // If the object to follow is not the player, set a min velocity to speed up approach.
        if (objToFollow.tag != "Player" && thisRBody.velocity.magnitude < minVel) {
            thisRBody.velocity = thisRBody.velocity.normalized * minVel;
        }

        if(distanceToTarget.magnitude <= 0.1) {
            thisRBody.velocity = Vector2.zero;
            transform.position = worldTargetPos;
        }
    }

    public void AttachToObject(GameObject obj, Vector2 target) {
        if(objToFollow != obj) {
            objToFollow = obj;
            rBodyOfObj = objToFollow.GetComponent<Rigidbody2D>();

            targetPos = target;
        }
    }

    public void ResetAttachment() {
        if(objToFollow.tag != "Player") {
            objToFollow = player;
            rBodyOfObj = objToFollow.GetComponent<Rigidbody2D>();

            targetPos = defaultTarget;
        }
    }

    private void FlipSide(Direction direction) {
        targetPos = new Vector2(targetPos.x * -1.0f, targetPos.y);
        speechBubble.Flip(direction);
    }

    public Vector3 GetTargetPos() {
        return worldTargetPos;
    }

    public GameObject GetTarget() {
        return objToFollow;
    }

    public void ActivateSpeecBubble(bool active) {
        speechBubble.ActivatePanel(active);
    }

    public void ActivateSpeecBubble(bool active, string text) {
        speechBubble.ActivatePanel(active, text);
    }
}
