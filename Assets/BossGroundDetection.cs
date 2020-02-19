using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGroundDetection : MonoBehaviour
{
    BossLick bossLick;
    
    // Start is called before the first frame update
    void Start()
    {
        bossLick = GetComponentInParent<BossLick>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            bossLick.StartCoroutine("Jump");
        }
    }


}
