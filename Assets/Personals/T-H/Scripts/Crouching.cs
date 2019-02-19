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
    public float crouchSpeed;

    // Slide variables
    private bool isSliding;
    public int slideSpeedModifier = 2;
    private float slideSpeed;
    private double slideTimer = 0f;
    private float moveSpeed;
    public float maxSlidingTime = 1f;
    public float timeSpentNotSliding = 2f;
    public float slideCooldown = 2f;
    

    // Use this for initialization
    void Start () {
        isSliding = false;
        // Henter verdier til boxcollider
        boxCollider = GetComponent<BoxCollider2D>();
        boxcolliderHeight = boxCollider.size.y;
        boxcolliderWidth = boxCollider.size.x;

        // Henter movementspeeds
        Mv2 = GetComponent<MovementV2>();
        moveSpeed = Mv2.moveSpeed;
        slideSpeed = moveSpeed + slideSpeedModifier;
        timeSpentNotSliding = 2f;
    }

    // Update is called once per frame
    void Update() {
        // Crouch
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Crouch();
        }
        else
        {
            StopCrouch();
        }



        // Slide
        if (CheckStartSlide() && timeSpentNotSliding == slideCooldown)
        {
            isSliding = true;
            slideTimer = 0;
            Mv2.moveSpeed = slideSpeed;
        }

        if (isSliding && slideTimer < maxSlidingTime)
        {
            slideTimer += Time.deltaTime;
            Debug.Log("Slidetimer: " + slideTimer);
            Debug.Log("Slidespeed: " + slideSpeed + "Movement: " + moveSpeed);
        }

        else if (isSliding && slideTimer == maxSlidingTime)
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
            timeSpentNotSliding = 0f;
        }

        if (!isSliding)
        {
            if (timeSpentNotSliding < slideCooldown)
            {
                timeSpentNotSliding += Time.deltaTime;
            }
        }
    }

    private void Crouch()
    {
        boxCollider.offset = new Vector2(0, boxcolliderHeight * 0.125f);
        boxCollider.size = new Vector2(boxcolliderWidth, boxcolliderHeight * 0.75f);
        groundCollider.localPosition = new Vector2(0, -boxcolliderHeight / 2 + boxcolliderHeight * 0.25f);
        isCrouching = true;
        ActivateTriggers(triggers, false);
        if (!isSliding)
        {
            Mv2.moveSpeed = crouchSpeed;
        }
    }
    private void StopCrouch()
    {
        boxCollider.size = new Vector2(boxcolliderWidth, boxcolliderHeight);
        boxCollider.offset = new Vector2(0, 0);
        groundCollider.localPosition = new Vector2(0, -boxcolliderHeight / 2);
        isCrouching = false;
        ActivateTriggers(triggers, true);
        Mv2.moveSpeed = moveSpeed;
    }
    private bool CheckStartSlide()
    {  // Fungerer ikke med Mv2.isGrounded. Må egentlig ha med i if statement
        if (isCrouching && Input.GetKey(KeyCode.A) && !isSliding
            || isCrouching && Input.GetKey(KeyCode.S) && !isSliding)
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

