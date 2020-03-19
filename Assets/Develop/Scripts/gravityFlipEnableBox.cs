using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravityFlipEnableBox : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().GravityFlipEnabled = true;
        }
    }
}
