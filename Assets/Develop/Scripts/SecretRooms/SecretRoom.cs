using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretRoom : MonoBehaviour
{
    private byte time = 255;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enter");
        StopAllCoroutines();
        StartCoroutine(fadeOut());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Exit");
        StopAllCoroutines();
        StartCoroutine(fadeIn());
    }

    IEnumerator fadeOut()
    {
        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
        Color32 color = renderer.color; // get color 
        while (time > 0) // fade
        {
            time -= 15;
            color.a = time;
            renderer.color = color;
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator fadeIn()
    {
        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
        Color32 color = renderer.color; // get color 
        while (time < 255) // fade
        {
            time += 15;
            color.a = time;
            renderer.color = color;
            yield return new WaitForSeconds(0.01f);
        }
        
    }
}
