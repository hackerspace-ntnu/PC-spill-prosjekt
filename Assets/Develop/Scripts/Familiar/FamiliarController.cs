using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

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

    [SerializeField]
    private SkeletonMecanim skeletonMecanim;
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private PlayerController controller;

	// Use this for initialization
	void Start () {
        player = transform.parent.gameObject;
        controller = transform.parent.gameObject.GetComponent<PlayerController>();

        skeletonMecanim = GetComponentInChildren<SkeletonMecanim>();
        if(!skeletonMecanim) {
            Debug.LogError("SKELETONMECANIM is NULL in FAMILIARCONTROLLER");
        }

        animator = GetComponentInChildren<Animator>();
        if(!animator) {
            Debug.LogError("ANIMATOR is NULL in FAMILIARCONTROLLER");
        }

        if(controller == null) {
            Debug.LogError("CONTROLLER is NULL in FAMILIAR");
        }

        // Initialize object to follow. Set to parent object if not set in editor.
        if(objToFollow == null) {
            objToFollow = player;
        }
        rBodyOfObj = objToFollow.GetComponent<Rigidbody2D>();
        thisRBody = GetComponent<Rigidbody2D>();

        worldTargetPos = new Vector3(objToFollow.transform.position.x + targetPos.x, objToFollow.transform.position.y + targetPos.y, 0.0f);
        transform.position = worldTargetPos;
        defaultTarget = targetPos;

        StartCoroutine(GlitchAnimation());
	}
	
	// Update is called once per frame
	void Update () {
        // If player/object is moving to the left, place familiar on the right of player/object.
        if (rBodyOfObj.velocity.x < -0.1f) {
            if (targetPos.x < 0.0f) {
                FlipSide(DIRECTION.LEFT);
            }
        }

        // If player/object is moving to the right, place familiar on the left of player/object.
        if (rBodyOfObj.velocity.x > 0.1f) {
            if (targetPos.x > 0.0f) {
                FlipSide(DIRECTION.RIGHT);
            }
        }

        if(objToFollow.tag != "Player") {
            if(player.transform.position.x < objToFollow.transform.position.x) {
                FlipSide(DIRECTION.LEFT);
            }

            if (player.transform.position.x > objToFollow.transform.position.x) {
                FlipSide(DIRECTION.RIGHT);
            }

        }

        if(player.transform.position.x > this.transform.position.x) {
            skeletonMecanim.skeleton.ScaleX = 1 * controller.FlipGravityScale;
        } else if (player.transform.position.x < this.transform.position.x) {
            skeletonMecanim.skeleton.ScaleX = -1 * controller.FlipGravityScale;
        }

        // Set the target for the familiar in world coordinates.
        worldTargetPos = new Vector3(
            objToFollow.transform.position.x + targetPos.x, 
            objToFollow.transform.position.y + targetPos.y * controller.FlipGravityScale, 
            0.0f);
        
    }

    private void FixedUpdate() {
        // Calculate distance to target position and set velocity towards target with speed increasing with distance.
        Vector2 distanceToTarget = transform.position - worldTargetPos;
        Vector2 newVelocity = distanceToTarget.normalized * Mathf.Pow(distanceToTarget.magnitude, distanceFactor);
        thisRBody.MovePosition(thisRBody.position - newVelocity * Time.fixedDeltaTime);
        //thisRBody.velocity = -newVelocity;

        // If the object to follow is not the player, set a min velocity to speed up approach.
        if (objToFollow.tag != "Player" && thisRBody.velocity.magnitude < minVel) {
            thisRBody.velocity = thisRBody.velocity.normalized * minVel;
        }
    }

    private IEnumerator GlitchAnimation() {
        while (true) {
            
            yield return new WaitForSeconds(Random.Range(4.0f, 12.0f));

            animator.SetBool("Glitch", true);

            yield return new WaitForSeconds(2.0f);

            animator.SetBool("Glitch", false);
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

    private void FlipSide(DIRECTION direction) {
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
