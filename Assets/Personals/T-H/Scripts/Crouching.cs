using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouching : MonoBehaviour {
   
    // Crouch variables
    private BoxCollider2D hitbox;
    private MovementV2 Mv2;
    public bool isCrouching;
    public float boxcolliderHeigth;
    public float boxcolliderWidth;

    // Slide variables
    private bool isSliding;
    public float slideSpeedModifier = 2f;
    private float slideSpeed;
    private double slideTimer = 0f;
    public float maxSlidingTime = 3f;
 
    

    // Use this for initialization
    void Start () {
        isSliding = false;
        hitbox = GetComponent<BoxCollider2D>();
        boxcolliderHeigth = hitbox.size.y;
        boxcolliderWidth = hitbox.size.x;
        Mv2 = GetComponent<MovementV2>();
    }

    // Update is called once per frame
    void Update() {
        // Crouch
        if (Input.GetKey(KeyCode.LeftControl))
        {
            hitbox.size = new Vector2(boxcolliderWidth, boxcolliderHeigth * 0.5f);
            isCrouching = true;
        }
        else
        {
            hitbox.size = new Vector2(boxcolliderWidth, boxcolliderHeigth);
            isCrouching = false;
        }
        
        
        // Slide
        if (isCrouching && Mv2.isGrounded && Input.GetKey(KeyCode.A) && !isSliding
            || isCrouching && Mv2.isGrounded && Input.GetKey(KeyCode.S) && !isSliding) 
        {
            isSliding = true;
            slideTimer = 0.0;
        }
        if (isSliding && slideTimer < maxSlidingTime)
        {
            slideTimer += Time.deltaTime;
        }
        else
        {
            isSliding = false;
        }
    }
}

