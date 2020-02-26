using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private BossLick bossLick;
    private BossSpit bossSpit;
    private BossBGCrawl bossBGCrawl;
    private BossDamage bossDamage;
    private BossHealth bossHealth;

    private MonoBehaviour activeState;
    public bool overrideState = false;

    public int bossMode = 1;

    private void Awake()
    {
        // Play boss cutscene or something
    }

    void Start()
    {
        // Set boss state to its starting state, potentially an attack
        bossLick = GetComponent<BossLick>();
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
            bossLick.enabled = true;
        }
    }


    public void updateState(MonoBehaviour requestor)
    {
        if (overrideState)
        {
            if (requestor == bossHealth)
            {
                print("Override State");
                activeState.enabled = false;
                overrideState = false;
            }
        }

        else {
            switch (bossMode)
            {
                case 1:
                    if (requestor == bossSpit)
                    {
                        updateActiveState(bossBGCrawl);
                    }
                    else if (requestor == bossBGCrawl)
                    {
                        updateActiveState(bossLick);
                    }
                    else if (requestor == bossLick)
                    {
                        updateActiveState(bossSpit);
                    }



                    else if (requestor == bossDamage)
                    {
                        updateActiveState(bossBGCrawl);
                    }
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    print("Death");
                    break;
                default:
                    print("Should not be here");
                    break;
            }
        }
    }

    private void updateActiveState(MonoBehaviour newState)
    {
        activeState = newState;
        activeState.enabled = true;
    }
}
