using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int health = 100;

    

    private BossController bc;

    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BossController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            takeDamage(10);
            bc.overrideState = true;
            print("Damaged");
            
            bc.updateState(this);
        }
    }

    private void updateHealthStatus()
    {
        if (health <= 0)
        {
            // Death
            print("NOOOOO!");
            Destroy(gameObject);
        }

        else if (health < 33)
        {
            // Berzerk mode
            print("Rough");
            // Activate BossTranform
        }
        else if (health < 66)
        {
            // Hard mode
            print("Bloodied");
        }
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        updateHealthStatus();
    }


}
