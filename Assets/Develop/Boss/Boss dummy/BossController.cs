using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private BossJump bossJump;

    private void Awake()
    {
        //Play boss cutscene or something
    }

    void Start()
    {
        //Set boss state to it's starting state, potencially an attack

        bossJump = GetComponent<BossJump>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            bossJump.enabled = true;
        }
    }


    

}
