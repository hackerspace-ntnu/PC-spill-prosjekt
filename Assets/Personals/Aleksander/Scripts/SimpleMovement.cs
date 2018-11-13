using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour {

    public bool useVelocity;
    public float speed;
    public float jumpForce;

    private float moveHorizontal;
    private float moveVertical;
    private Rigidbody2D rigidBody;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate() {
        moveHorizontal = Input.GetAxis("Horizontal");

        if(useVelocity) {
            rigidBody.velocity = new Vector2(moveHorizontal * speed, rigidBody.velocity.y);
        } else {
            rigidBody.AddForce(new Vector2(moveHorizontal * speed, 0.0f));
        }
        
    }

    private void jump() {

    }
}
