using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    // Start is called before the first frame update

    public int maxHealth;
    public int currentHealth;

    public void takeDamage()
    {
        currentHealth -= 1;
        if(currentHealth <= 0)
        {
            die();
        }
    }

    private void die()
    {
        print("Faen da");
    }
    void Start()
    {
        currentHealth = maxHealth;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

}

