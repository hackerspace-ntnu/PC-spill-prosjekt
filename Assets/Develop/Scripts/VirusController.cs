using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusController : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Transform movedir;

    [SerializeField]
    private Transform ledgeCheckPos, ledgeCheckDir;

    [SerializeField]
    private Transform ledgeRotatePos;

    [SerializeField]
    private Transform wallCheckPos, wallCheckDir;
    [SerializeField]
    private float wallCheckLen;

    [SerializeField]
    private float groundCheckLen;

    private bool rotating = false;

    [SerializeField]
    private float diffThreshold;

    [SerializeField]
    private bool canRotate = false;
    private float gravity = -9.8f;

    private float rotationSpeed = 360;

    private int mask;
    private Rigidbody2D body;

    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private GameObject deathParticlePrefab;
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        mask = groundLayer;
    }

    // Update is called once per frame
    void Update()
    {
                    
    }
    
    public void die()
    {
        Instantiate(deathParticlePrefab, transform.position,transform.rotation);
        Destroy(transform.gameObject);
    }

    private void FixedUpdate()
    {       
        body.AddForce(transform.up * gravity* body.mass);
        checkForObstacles();
        body.velocity = speed * (movedir.position-transform.position) + transform.up * Vector2.Dot(transform.up, body.velocity) ;
    }
    
    private void checkForObstacles()
    {
        if (!canRotate)
        {
            if (checkForLedge() || checkForWall())
            {
                Vector3 newScale = transform.localScale;
                newScale.Scale(new Vector3(-1f, 1f, 1f));
                transform.localScale = newScale;
            }
        }
        else
        {
            this.transform.eulerAngles = this.transform.eulerAngles + new Vector3(0f, 0f, rotationSpeed * Time.fixedDeltaTime)*checkRotateDir();
            
        }
    }
    private float checkRotateDir()
    {
        
        RaycastHit2D ray = Physics2D.Raycast(ledgeRotatePos.position, -transform.up, groundCheckLen, mask);
        if(ray.collider == null)
        {
            return -1f * Mathf.Sign(transform.localScale.x);
        }
        if(Physics2D.Raycast(wallCheckPos.position, transform.right*transform.localScale.x, wallCheckLen, mask).collider != null)
        {
            return 1f * Mathf.Sign(transform.localScale.x);
        }

        float cross =  ray.normal.y * transform.up.x - ray.normal.x * transform.up.y;

        if (Mathf.Abs(cross) > diffThreshold)
        {
            return Mathf.Sign(cross);
        }
        else
        {
            return 0f;
        }
    }

   
    private bool checkForLedge()
    {
        Debug.DrawRay(ledgeCheckPos.position, ledgeCheckDir.position - ledgeCheckPos.position, Color.green, Time.fixedDeltaTime);
        return Physics2D.Raycast(ledgeCheckPos.position, ledgeCheckDir.position - ledgeCheckPos.position, 0.5f, mask).collider == null;
    }
   
    private bool checkForWall()
    {
        Debug.DrawRay(wallCheckPos.position, wallCheckDir.position-wallCheckPos.position,  Color.green, Time.fixedDeltaTime);
        //print(Physics2D.Raycast(transform.position, Vector2.right * dir, wallCheckLength, mask).collider.name);
        return Physics2D.Raycast(wallCheckPos.position, wallCheckDir.position - wallCheckPos.position, wallCheckLen, mask).collider != null;
    }
}
