using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidBehavior : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Despawn());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // TODO: Damage player here and increase glitch-meter
            print("Acid damage!");
        }
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
        yield return null;
    }
}
