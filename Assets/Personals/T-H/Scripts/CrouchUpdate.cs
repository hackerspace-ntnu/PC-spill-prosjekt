using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchUpdate : MonoBehaviour
{
    // Crouch variables
    public BoxCollider2D boxCollider;
    private float boxcolliderHeight;
    private float boxcolliderWidth;
    private Vector2 boxcolliderOffset;
    public GameObject[] triggers = new GameObject[2];
    private bool isCrouching;

    // Slide variables
    private bool isSliding;
    private double slideTimer = 0f;
    public float maxSlidingTime = 1f;
    public float timeSpentNotSliding = 2f;
    private float timeSpentMoving = 0f;
    public float timeSpentMovingMinimum = 0.4f;
    public float slideCooldown = 2f;

    // Raycast
    private RaycastHit2D raycast;
    private float roofDistance;
    private int Player_layer;
    private bool hasReleased;

    // Use this for initialization
    void Start()
    {
        isSliding = false;

        // Henter verdier til boxcollider
        boxcolliderHeight = boxCollider.size.y;
        boxcolliderWidth = boxCollider.size.x;

        // Henter movementspeeds
        timeSpentNotSliding = 2f;

        // Raycast
        roofDistance = boxcolliderHeight - (boxcolliderHeight * 0.75f);
        Player_layer = ~(LayerMask.GetMask("Player"));
        hasReleased = false;
    }

    // Update is called once per frame
    void Update()
    {

        // RayCast
        raycast = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + boxcolliderHeight), Vector2.up, roofDistance, Player_layer);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + boxcolliderHeight), Vector2.up * roofDistance, Color.red, 0, false);

        // Crouch        
        if (Input.GetKeyDown(KeyCode.C) && !isCrouching) // Funker ikke med control i unity editor
            Crouch();
        else if ((hasReleased && raycast.collider == null) || (Input.GetKeyUp(KeyCode.C) && raycast.collider == null)) { // Hvis man slipper C og er ikke under tak
            StopCrouch();
            hasReleased = false;
        }
        else if (Input.GetKeyUp(KeyCode.C)) // Hvis man har sluppet C men er fortsatt under et tak
            hasReleased = true;
        


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

        //if (!isSliding)
            // Set crouchspeed
    }
    private void StopCrouch()
    {
        boxCollider.size = new Vector2(boxcolliderWidth, boxcolliderHeight); // Restorer vanlig boxcol
        boxCollider.offset = new Vector2(0, 0); // Restorer posisjon til boxcol
        isCrouching = false;
        ActivateTriggers(triggers, true); // reaktiverer walltriggers
    }
    private void ActivateTriggers(GameObject[] triggers, bool boolean) // For å deaktivere wall triggers
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
            timeSpentNotSliding >= slideCooldown) // Skal egentlig ha med Movement.IsGrounded
        {
            Debug.Log("Sliding");
            Crouch();
            isSliding = true;
            slideTimer = 0;
        }
    }
    private void calculateTimeSpentMoving() 
    {
        float moveHorizontal = Mathf.Abs(Input.GetAxis("Horizontal"));
        if (moveHorizontal == 1 && !isCrouching) // Hvis man beveger seg maks i en retning og ikke allerede croucher
            timeSpentMoving += Time.deltaTime;

        else
            timeSpentMoving = 0;

    }

    // Getters

    public bool getIsCrouching()
    {
        return isCrouching;
    }

    public bool getIsSliding()
    {
        return isSliding;
    }
}
