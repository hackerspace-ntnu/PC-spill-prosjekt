using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpit : MonoBehaviour
{
    public GameObject spitPref;
    public Transform spawnPos;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Fire();
        }
    }

    void Fire()
    {
        GameObject spit = Instantiate(spitPref, new Vector2 (spawnPos.position.x, transform.position.y), Quaternion.identity);
    }
}
