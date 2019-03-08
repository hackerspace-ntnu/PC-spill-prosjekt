using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouching : MonoBehaviour {
   
    // Crouch variables
    public BoxCollider2D boxCollider;
    private Movement Mv;
    private float boxcolliderHeight;
    private float boxcolliderWidth;
    private Vector2 boxcolliderOffset;
    public GameObject[] triggers = new GameObject[2];
    public bool isCrouching;
    public float crouchSpeed = 3f;

    // Slide variables
    private Rigidbody2D rb;
    private bool isSliding;
    public int slideSpeedModifier = 2;
    private float slideSpeed;
    private double slideTimer = 0f;
    private float moveSpeed;
    public float maxSlidingTime = 1f;
    public float timeSpentNotSliding = 2f;
    private float timeSpentMoving = 0f;
    public float timeSpentMovingMinimum = 0.4f;
    public float slideCooldown = 2f;
    

    // Use this for initialization
    void Start () {
        isSliding = false;
        // Henter verdier til boxcollider
        boxcolliderHeight = boxCollider.size.y;
        boxcolliderWidth = boxCollider.size.x;

        // Henter movementspeeds
        Mv = GetComponent<Movement>();
        moveSpeed = Mv.moveSpeed;
        slideSpeed = moveSpeed + slideSpeedModifier;
        // Burde egentlig hente movespeed
        rb = GetComponent<Rigidbody2D>();
        timeSpentNotSliding = 2f;
    }

    // Update is called once per frame
    void Update() {

        // Crouch
        if (Input.GetKeyDown(KeyCode.C) && !isCrouching)
        { 
            Crouch();
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            StopCrouch();
            Debug.Log("Stopped crouching");
        }

        // Slide

        calculateTimeSpentMoving(); // Sjekker hvor lenge man har beveget seg før man prøver å slide

        if (CheckStartSlide())
        {
            isSliding = true;
            slideTimer = 0;
        }

        if (isSliding && slideTimer < maxSlidingTime)
        {
            slideTimer += Time.deltaTime;
        }

        else if (isSliding && slideTimer >= maxSlidingTime)
        {
            isSliding = false;
            slideTimer = 0;
            StopCrouch();
            Debug.Log("No longer sliding");
            timeSpentNotSliding = 0f;
        }

        else
        {
            slideTimer = 0;
            isSliding = false;
        }

        if (!isSliding && !isCrouching && timeSpentNotSliding < slideCooldown)
        {
            timeSpentNotSliding += Time.deltaTime;
        }
    }

    private void calculateTimeSpentMoving()
    {
        if (Mathf.Abs(Mv.moveHorizontal) == 1)
        {
            timeSpentMoving += Time.deltaTime;
        }
        else
        {
            timeSpentMoving = 0;
        }
    }

    private void Crouch()
    {
        boxCollider.offset = new Vector2(0, boxcolliderHeight * 0.125f);
        boxCollider.size = new Vector2(boxcolliderWidth, boxcolliderHeight * 0.75f);
        isCrouching = true;
        ActivateTriggers(triggers, false);
        if (!isSliding)
        {
            Mv.moveSpeed = crouchSpeed;
            //rb.velocity = new Vector2(crouchSpeed, rb.velocity.y);
        }
    }
    private void StopCrouch()
    {
        boxCollider.size = new Vector2(boxcolliderWidth, boxcolliderHeight);
        boxCollider.offset = new Vector2(0, 0);
        isCrouching = false;
        ActivateTriggers(triggers, true);
        Mv.moveSpeed = moveSpeed;
        //rb.velocity = new Vector2(moveSpeed, rb.velocity.y); // Usikker på dette
    }

    
    private bool CheckStartSlide()
    { 
        if (isCrouching && timeSpentMoving >= timeSpentMovingMinimum && !isSliding && Mv.isGrounded && 
            timeSpentNotSliding >= slideCooldown)
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

