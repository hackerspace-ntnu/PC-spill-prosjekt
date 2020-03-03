using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusController : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Vector2 ledgeCheckPos;

    [SerializeField]
    private Vector2 ledgeRotatePos;

    [SerializeField]
    private float wallCheckLength;

    private bool rotating = false;

    [SerializeField]
    private bool canRotate = false;

    private Vector2[] verticalDir = new Vector2[] { new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0) };
    private Vector2[] horizontalDir = new Vector2[] { new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0), new Vector2(0, 1) };
    private int currentRotation = 0;
    private float gravity = -9.8f;

    private float rotationTime = 0.5f;
    private float rotationAngle = 90;

    private int mask;
    private Rigidbody2D body;

    [SerializeField]
    private int dir = 1;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        mask = LayerMask.GetMask("Platforms");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void die()
    {
        //TODO: implement 
    }

    private void FixedUpdate()
    {
        if (!rotating)
        {
            body.AddForce(verticalDir[currentRotation] * gravity* body.mass);
            checkForObstacles();
            body.velocity = speed * dir * horizontalDir[currentRotation] + rotateVector(body.velocity).y * verticalDir[currentRotation];

        }
        else
        {
            body.velocity = Vector2.zero;
        }


    }
    
    private void checkForObstacles()
    {
        if (!canRotate)
        {
            if (checkForLedge() || checkForWall())
            {
                dir *= -1;
            }
        }
        else
        {
            if (checkForLedgeRotate())
            {
                print("ledge");
                rotate(dir);
            }
            else if (checkForWall())
            {
                print("wall");
                rotate(-dir);
            }
        }
        if(Mathf.Sign(dir) != transform.localScale.x)
        {
            Vector3 newScale = transform.localScale;
            newScale.Scale(new Vector3(-1f, 1f, 1f));
            ledgeCheckPos.Scale(new Vector2(-1f, 1));
            transform.localScale = newScale;
        }
    }
    private void rotate(int dir)
    {
        StartCoroutine(rotationSubroutine(dir));
        this.currentRotation = (this.currentRotation + dir)%4;
        if (currentRotation < 0) { currentRotation += 4; }
    }

    private IEnumerator rotationSubroutine(int dir)
    {
        rotating = true;
        int numOfRots = (int)Mathf.Ceil(rotationTime / Time.fixedDeltaTime);
        for(int i = 0; i < numOfRots; i++)
        {
            transform.Rotate(new Vector3(0, 0, 1), -dir*rotationAngle / numOfRots);
            yield return new WaitForFixedUpdate();
        }
        rotating = false;
    }

    private bool checkForLedge()
    {
        Debug.DrawRay(transform.position + (Vector3)rotateVector(ledgeCheckPos), -verticalDir[currentRotation]*0.5f,Color.green, Time.fixedDeltaTime);
        return Physics2D.Raycast((Vector2)transform.position + rotateVector(ledgeCheckPos), -verticalDir[currentRotation], 0.5f, mask).collider == null;
    }
    private bool checkForLedgeRotate()
    {

        Debug.DrawRay(transform.position + (Vector3)rotateVector(ledgeRotatePos), -verticalDir[currentRotation], Color.green, Time.fixedDeltaTime); ;
        return checkForLedge() && Physics2D.Raycast((Vector2)transform.position + rotateVector(ledgeRotatePos), -verticalDir[currentRotation], 0.5f, mask).collider == null;
    }
    private bool checkForWall()
    {
        Debug.DrawRay(transform.position + (Vector3)verticalDir[currentRotation] * 0.1f, (Vector3)horizontalDir[currentRotation] * dir * wallCheckLength, Color.green, Time.fixedDeltaTime);
        //print(Physics2D.Raycast(transform.position, Vector2.right * dir, wallCheckLength, mask).collider.name);
        return Physics2D.Raycast((Vector2)transform.position + verticalDir[currentRotation] * 0.1f, horizontalDir[currentRotation] * dir, wallCheckLength, mask).collider != null;
    }
    private Vector2 rotateVector(Vector2 a)
    {
        return a.x * horizontalDir[currentRotation] + a.y * verticalDir[currentRotation];
    }
}
