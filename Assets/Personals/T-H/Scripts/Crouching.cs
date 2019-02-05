using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouching : MonoBehaviour {
   
    // Crouch variables
    private BoxCollider2D boxCollider;
    private MovementV2 Mv2;
    public bool isCrouching;
    public float boxcolliderHeight;
    public float boxcolliderWidth;
    private Vector2 boxcolliderOffset;
    private BoxCollider2D[] boxcolliderChildren;

    // Slide variables
    private bool isSliding;
    public float slideSpeedModifier = 2f;
    private float slideSpeed;
    private double slideTimer = 0f;
    public float maxSlidingTime = 3f;
 
    

    // Use this for initialization
    void Start () {
        isSliding = false;
        boxCollider = GetComponent<BoxCollider2D>();
        boxcolliderChildren = boxCollider.GetComponentsInChildren<BoxCollider2D>();
        boxcolliderHeight = boxCollider.size.y;
        boxcolliderWidth = boxCollider.size.x;
        Mv2 = GetComponent<MovementV2>();
    }

    // Update is called once per frame
    void Update() {
        // Crouch
        if (Input.GetKey(KeyCode.LeftControl))
        {
            boxCollider.offset = new Vector2(0, boxcolliderHeight * 0.125f);
            boxCollider.size = new Vector2(boxcolliderWidth, boxcolliderHeight * 0.75f);
            isCrouching = true;

            for (int i = 1; i < 4; i++)
            {
                boxcolliderChildren[i].offset = new Vector2(0, boxcolliderHeight);
                // Trenger å endre størrelse somehow
            }
        }
        else
        {
            boxCollider.size = new Vector2(boxcolliderWidth, boxcolliderHeight);
            boxCollider.offset = new Vector2(0, 0);
            isCrouching = false;

            for (int i = 0; i < 4; i++)
            {
                boxcolliderChildren[i].offset = new Vector2(0, 0);
                // Trenger å endre størrelse somehow
            }
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

