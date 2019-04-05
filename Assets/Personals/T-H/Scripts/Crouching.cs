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
    private bool isSliding;
    public int slideSpeedModifier = 10;
    private float slideSpeed;
    private double slideTimer = 0f;
    private float moveSpeed;
    public float maxSlidingTime = 1f;
    public float timeSpentNotSliding = 2f;
    private float timeSpentMoving = 0f;
    public float timeSpentMovingMinimum = 0.4f;
    public float slideCooldown = 2f;

    // Raycast
    private RaycastHit2D raycast;
    private float roofDistance;
    private int Player_layer;

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
        timeSpentNotSliding = 2f;

        // Raycast
        roofDistance = boxcolliderHeight - (boxcolliderHeight * 0.75f);
        roofDistance = 10; // Bare temporary bugfixing
        Player_layer = ~LayerMask.GetMask("Player");
        raycast = Physics2D.Raycast(transform.position, Vector2.up, roofDistance, Player_layer);
    }

    // Update is called once per frame
    void Update() {

        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + boxcolliderHeight), Vector2.up * roofDistance, Color.red, 0, false);
        isRoof();

        // Crouch

        if (Input.GetKeyDown(KeyCode.C) && !isCrouching) // Funker ikke med control i unity editor
            Crouch();

        else if (Input.GetKeyUp(KeyCode.C))
            StopCrouch();


        // Slide

        CheckStartSlide(); // Sjekker om man skal starte å slide. Kommer før calculateTime pga !isCrouching.
        calculateTimeSpentMoving(); // Sjekker hvor lenge man har beveget seg før man prøver å slide

        if (isSliding && slideTimer < maxSlidingTime) // Sjekker om man er innenfor slidetimeren og kan fortsette å slide
            slideTimer += Time.deltaTime;

        else if (isSliding && slideTimer >= maxSlidingTime) // Hvis man er ferdig med å slide
        {
            isSliding = false;
            slideTimer = 0;
            StopCrouch();
            timeSpentNotSliding = 0f;
        }


        if (!isSliding && !isCrouching && timeSpentNotSliding <= slideCooldown) // Øker timespentnotsliding dersom man ikke slider eller croucher 
            timeSpentNotSliding += Time.deltaTime;

    }



    // Crouch functions
    private void Crouch()
    {
        boxCollider.offset = new Vector2(0, boxcolliderHeight * 0.125f); // Flytter boxcol opp
        boxCollider.size = new Vector2(boxcolliderWidth, boxcolliderHeight * 0.75f); // Gjør boxcol mindre
        isCrouching = true; 
        ActivateTriggers(triggers, false); // deaktiverer walltriggers

        if (!isSliding)
            Mv.moveSpeed = crouchSpeed;
    }
    private void StopCrouch()
    {
        boxCollider.size = new Vector2(boxcolliderWidth, boxcolliderHeight);
        boxCollider.offset = new Vector2(0, 0);
        isCrouching = false;
        ActivateTriggers(triggers, true);
        Mv.moveSpeed = moveSpeed;
    }
    private void ActivateTriggers(GameObject[] triggers, bool boolean)
    {
        foreach (GameObject trigger in triggers)
        {
            trigger.SetActive(boolean);
        }
    }

    // Slide functions
    private void CheckStartSlide()
    { 
        if (isCrouching && timeSpentMoving >= timeSpentMovingMinimum && !isSliding && 
            timeSpentNotSliding >= slideCooldown) // Skal egentlig ha med Mv.IsGrounded
        {
            Debug.Log("Sliding");
            Crouch();
            isSliding = true;
            slideTimer = 0;
            Mv.moveSpeed = slideSpeed;
        }
    }
    private void calculateTimeSpentMoving()
    {
        if (Mathf.Abs(Mv.moveHorizontal) == 1 && !isCrouching)
            timeSpentMoving += Time.deltaTime;

        else
            timeSpentMoving = 0;

    }

    // Raycast functions 
    private bool isRoof()
    {
        if (raycast.collider != null)
        {
            Debug.Log("RayCast: " + raycast.collider.gameObject.tag);
            Debug.Log("Roof detected");
            return true;
        }
        Debug.Log("No roof detected");
        return false;
    }
}

