using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouching : MonoBehaviour {

    private BoxCollider2D hitbox;
    private MovementV2 Mv2;

    public bool isCrouching;
    private Vector2 crouchedVector = new Vector2(1, 0.5f);


    private bool isSliding;
    public  float slideSpeed = 12f;
    private double slideTimer = 0f;
    private float maxSlide = 3f;
 
    

    // Use this for initialization
    void Start () {
        hitbox = GetComponent<BoxCollider2D>();
        isCrouching = false;
        isSliding = false;
        Mv2 = GetComponent<MovementV2>();
	}

    // Update is called once per frame
    void Update() {
        // Crouch
        if (Input.GetKey(KeyCode.LeftControl))
        {
            hitbox.size = hitbox.size * crouchedVector;
            isCrouching = true;
        }
        else
        {
            hitbox.size = new Vector2(1, 1);
            isCrouching = false;
        }
            
        // Slide
        if (isCrouching && Mv2.isGrounded && Input.GetKey(KeyCode.A) && !isSliding
            || isCrouching && Mv2.isGrounded && Input.GetKey(KeyCode.S) && !isSliding) 
        {
            isSliding = true;
            slideTimer = 0.0;
        }
        if (isSliding && slideTimer < maxSlide)
        {
            slideTimer += Time.deltaTime;
            Mv2.moveSpeed = slideSpeed;
        }
        else
        {
            isSliding = false;
        }
    }
}

