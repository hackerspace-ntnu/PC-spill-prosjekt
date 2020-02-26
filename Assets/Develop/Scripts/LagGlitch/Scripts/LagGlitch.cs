using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LagGlitch : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    public bool isClone;
    public bool ghostActive;
    public SpriteLoad spriteLoad;
    public EnemyMovement enemyMovement;
    public GlobalScript globalScript;
    void Start()
    {
        ghostActive = false;
        rb = this.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        // Powerup - key for now = H
        if(Input.GetKeyDown(KeyCode.H))
        {
            ActivateLagGlitch();
        }
    }

    GameObject clone;
    LagGlitch cloneScript;
    PlayerStats cloneStats;
    public SimpleCameramovement camScript;
    public GameObject lagTimer;
    public GlitchObscuring gObscuring;
    public Cinemachine.CinemachineVirtualCamera cm;
    public Spine.Skeleton skeleton;
    IEnumerator addToGlitchMeter()
    {
        while(true) {
            gObscuring.ChangeGlitchValue(5);
            yield return new WaitForSeconds(0.5f);
        }
    }
    private GameObject setOpacityOfSprite(GameObject obj)
    {
        MeshRenderer renderer = obj.GetComponentInChildren<MeshRenderer>();
        renderer.material.color = new Color32(255, 255, 255, 255);
        
        return obj;
    }
    public Material normalMaterial;
    public Material ghostMaterial;
    public void ActivateLagGlitch()
    {
        if(!ghostActive)
        {
            Debug.Log("ACTIVATING GLITCH");
            this.GetComponent<PlayerController>().Animator.enabled = false;
            this.GetComponent<PlayerController>().enabled = false;
            ghostActive = true;
            Vector2 velocity = rb.velocity*0.5f; // Save velocity to add it to the "ghost sprite" when it spawns
            rb.simulated = false; // turn off physics simulation
            // make clone
            clone = Instantiate(this.gameObject); // Create clone
            cloneScript = clone.GetComponent<LagGlitch>();
            clone.GetComponent<PlayerController>().enabled = true;
            clone.GetComponent<PlayerController>().Animator.enabled = true;

            clone.GetComponentInChildren<MeshRenderer>().material = ghostMaterial;

            clone.GetComponent<PlayerAnim>().enabled = true;
            cloneStats = clone.GetComponent<PlayerStats>();
            cloneStats.playerHealth = this.GetComponent<PlayerStats>().playerHealth;
            cloneScript.enabled = false;
            cm.m_Follow = clone.transform;
            clone = setOpacityOfSprite(clone); // Switch clone's sprite to ghost
            Rigidbody2D cloneRb = clone.GetComponent<Rigidbody2D>(); // Access clone's rigidbody
            cloneRb.simulated = true; // turn on clone's simulation
            cloneRb.velocity = velocity; // give clone the original sprite's velocity vector
            // lagTimer.SetActive(true); // Make lag glitch timer visible 
            StartCoroutine(StartCountdown());
            StartCoroutine(addToGlitchMeter());
            globalScript.glitchActive = true; // Update globalscript

        } else if (ghostActive) // You press H, and the following shall be done to the original sprite:
        {
            Teleport();
        }
    }

    public GlitchMask gMask;
    public void Teleport()
    {
        Debug.Log("DISABLING GLITCH");
        // enemyMovement.toggleLagGlitch(false); // Turn enemy box colliders back on TODO: ADD ENEMIES

        lagTimer.SetActive(false); // Hide lag glitch-timer
        // Debug.Log("Destroy " + this.name);
        cloneStats.playerHealth = this.GetComponent<PlayerStats>().playerHealth;
        // Debug.Log("This.health: " + this.GetComponent<PlayerStats>().playerHealth + ", clone.health: " + cloneStats.playerHealth);
        cloneScript.isClone = false;
        // clone.GetComponent<SpriteRenderer>().sprite = spriteLoad.playerRegular; TODO: Make player non-ghost again
        Destroy(this.gameObject);
        cloneScript.enabled = true;
        // clone.GetComponent<Rigidbody2D>().velocity *= 1.9f; // Get speedboost when converting back to regular 
        //clone.GetComponentInChildren<MeshRenderer>().material = normalMaterial;
        clone.name = "Player";
            StopCoroutine(addToGlitchMeter());
        globalScript.glitchActive = false; // Update globalscript

        ghostActive = false;
    }

    float currCountdownValue = 10f;
    public IEnumerator StartCountdown(float countdownValue = 5)
    {
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
            lagTimer.transform.localScale = new Vector3(currCountdownValue / countdownValue, 1, 1); // Update lag glitch timer
            // Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(0.1f);
            currCountdownValue-=0.1f;
        }
        Teleport();
    }

    
}
