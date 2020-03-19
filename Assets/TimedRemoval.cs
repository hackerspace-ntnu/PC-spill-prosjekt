using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedRemoval : MonoBehaviour
{
    [SerializeField]
    private float deathTimer;

    private void Start()
    {
        StartCoroutine(waitForDeath());
    }
    public IEnumerator waitForDeath()
    {
        yield return new WaitForSeconds(deathTimer);
        Destroy(transform.gameObject);
    }
}
