using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidBehavior : MonoBehaviour
{
    public float fadeTime = 5;

    private float deltaTime = 0;
    private SpriteRenderer sr;
    private Color tempColor;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        tempColor = sr.color;
        StartCoroutine(Despawn());
    }


    // Fade of the acid
    private void Update()
    {
        deltaTime += Time.deltaTime;
        tempColor.a = 1f - (deltaTime / fadeTime);
        sr.color = tempColor;
    }


    // Handle player collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // TODO: Damage / knowckback / glitchmeter
            print("Acid damage!");
        }
    }


    // Despawn
    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(fadeTime);
        Destroy(gameObject);
        yield return null;
    }
}
