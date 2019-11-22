using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private BossJump bossJump;
    private BossSpit bossSpit;
    private BossBGCrawl bossBGCrawl;
    private BossDamage bossDamage;
    private BossHealth bossHealth;

    private MonoBehaviour activeState;
    public bool overrideState = false;

    private void Awake()
    {
        // Play boss cutscene or something
    }

    void Start()
    {
        // Set boss state to it's starting state, potencially an attack

        bossJump = GetComponent<BossJump>();
        bossSpit = GetComponent<BossSpit>();
        bossBGCrawl = GetComponent<BossBGCrawl>();
        bossDamage = GetComponent<BossDamage>();
        bossHealth = GetComponent<BossHealth>();

        activeState = bossBGCrawl;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            bossJump.enabled = true;
        }
    }


    public void updateState(MonoBehaviour requestor)
    {
        if (overrideState) 
        {
            if (requestor == bossHealth)
            {
                print(1);
                activeState.enabled = false;
                updateActiveState(bossDamage);
                overrideState = false;
            }
        }
        else
        {
            if (requestor == bossSpit)
            {
                updateActiveState(bossBGCrawl);
            }
            else if (requestor == bossBGCrawl)
            {
                updateActiveState(bossSpit);
            }
            else if (requestor == bossDamage)
            {
                updateActiveState(bossBGCrawl);
            }
        }
    }

    private void updateActiveState(MonoBehaviour newState)
    {
        activeState = newState;
        newState.enabled = true;
    }
}
