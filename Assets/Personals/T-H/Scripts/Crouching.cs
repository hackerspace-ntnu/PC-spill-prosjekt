using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouching : MonoBehaviour {
   
    // Crouch variables
    private BoxCollider2D boxCollider;
    private MovementV2 Mv2;
    private float boxcolliderHeight;
    private float boxcolliderWidth;
    private Vector2 boxcolliderOffset;
    public EdgeCollider2D[] colliders = new EdgeCollider2D[4];
    public Transform groundCollider;
    public GameObject[] triggers = new GameObject[2];
    public bool isCrouching;

    // Slide variables
    private bool isSliding;
    public float slideSpeedModifier = 2f;
    private float slideSpeed;
    private double slideTimer = 0f;
    private float moveSpeed;
    public float maxSlidingTime = 3f;
    
 
    

    // Use this for initialization
    void Start () {
        isSliding = false;
        boxCollider = GetComponent<BoxCollider2D>();
        boxcolliderHeight = boxCollider.size.y;
        boxcolliderWidth = boxCollider.size.x;
        Mv2 = GetComponent<MovementV2>();
        moveSpeed = Mv2.moveSpeed;
        slideSpeed = moveSpeed * slideSpeedModifier;
    }

    // Update is called once per frame
    void Update() {
        // Crouch
        if (Input.GetKey(KeyCode.LeftControl))
        {
            boxCollider.offset = new Vector2(0, boxcolliderHeight * 0.125f);
            boxCollider.size = new Vector2(boxcolliderWidth, boxcolliderHeight * 0.75f);
            isCrouching = true;
            ActivateTriggers(triggers, false);
        }
        else
        {
            boxCollider.size = new Vector2(boxcolliderWidth, boxcolliderHeight);
            boxCollider.offset = new Vector2(0, 0);
            isCrouching = false;
            ActivateTriggers(triggers, true);
        }


        // Slide
        if (CheckStartSlide())
        {
            isSliding = true;
            slideTimer = 0.0;
            Mv2.moveSpeed = slideSpeed;
        }

        if (isSliding && slideTimer < maxSlidingTime)
        {
            slideTimer += Time.deltaTime;
        }

        else
        {
            isSliding = false;
            Mv2.moveSpeed = moveSpeed;
        }
    }


    private bool CheckStartSlide()
    {
        if (isCrouching && Mv2.isGrounded && Input.GetKey(KeyCode.A) && !isSliding
            || isCrouching && Mv2.isGrounded && Input.GetKey(KeyCode.S) && !isSliding)
        {
            return true;
        }
        return false;
    }
    private void ActivateTriggers(GameObject[] triggers, bool boolean)
    {
        foreach (GameObject trigger in triggers)
        {
            trigger.SetActive(boolean);
        }
    }
}

