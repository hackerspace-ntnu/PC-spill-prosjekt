using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretRoom : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enter");
        this.GetComponent<SpriteRenderer>().enabled = false;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Exit");
        this.GetComponent<SpriteRenderer>().enabled = true;
    }
}
