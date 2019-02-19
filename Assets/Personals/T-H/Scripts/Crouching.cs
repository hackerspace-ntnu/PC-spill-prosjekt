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
        Debug.Log(timeSpentNotSliding);

        // Crouch
        if (Input.GetKey(KeyCode.LeftControl) && Mv2.isGrounded && !isCrouching) // Må vel egentlig bruke GetKeyDown
        {
            Crouch();
        }
        else if ((Input.GetKey(KeyCode.LeftControl))) 
        {
            //Kode her?
            Debug.Log("Now crouching");
        }
        else // if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            StopCrouch();
            Debug.Log("Stopped crouching");
        }



        // Slide
        if (CheckStartSlide() && timeSpentNotSliding >= slideCooldown)
        {
            isSliding = true;
            slideTimer = 0;
            Mv2.moveSpeed = slideSpeed;
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
    { 
        if (isCrouching && Mathf.Abs(Mv2.moveHorizontal) == 1 && !isSliding && Mv2.isGrounded
            || isCrouching && Input.GetKey(KeyCode.D) && !isSliding && Mv2.isGrounded)
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

