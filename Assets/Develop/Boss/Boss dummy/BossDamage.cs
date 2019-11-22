using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamage : MonoBehaviour
{
    private BossController bc;

    private BossHealth bossHealth;

    // Start is called before the first frame update
    void Start()    
    {
        bc = GetComponent<BossController>();
        bossHealth = GetComponent<BossHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        bc.updateState(this);
    }

    private void OnEnable()
    {
        StartCoroutine(damaged());
    }

    IEnumerator damaged()
    {
        yield return new WaitForSeconds(2);
        enabled = false;
        yield return null;
    }
}
