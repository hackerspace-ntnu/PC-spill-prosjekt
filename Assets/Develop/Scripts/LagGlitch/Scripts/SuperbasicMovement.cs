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
        } else if (Input.GetKey(KeyCode.LeftArrow)) // Dash
        {
            StartCoroutine(Dash("left"));

        } else if (Input.GetKey(KeyCode.RightArrow)) // Dash
        {
            StartCoroutine(Dash("right"));
        }
    }
    IEnumerator Dash(string dir)
    {
        this.gameObject.layer = 10; // makes it so that the player can go through secret walls
        if(dir == "right")
        {
            rb.AddForce(new Vector2(force * 10, force));
        } else
        {
            rb.AddForce(new Vector2(-force * 10, force));
        }
        yield return new WaitForSeconds(0.15f);
        this.gameObject.layer = 8; // player can no longer go through secret walls
        rb.velocity /= 10;
    }
}
