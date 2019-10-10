using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperbasicMovement : MonoBehaviour
{
    Rigidbody2D rb;
    float force = 35f;
    public void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.W))
        {
            rb.AddForce(new Vector2(0, force));
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(new Vector2(-force, 0));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(new Vector2(0, -force));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(new Vector2(force, 0));
        }
    }
}
