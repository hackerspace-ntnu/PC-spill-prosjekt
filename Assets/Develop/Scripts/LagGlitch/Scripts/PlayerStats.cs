using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int playerHealth;
    public GameObject healthbar;
    public void Start()
    {
        playerHealth = 100;
    }
    public void GetHit(int damage)
    {
        playerHealth -= damage;
        UpdateHealthbar();
        Debug.Log(playerHealth);
        if(playerHealth <= 0)
        {
            Die();
            Debug.Log("Isded");
        }
    }

    public void Die()
    {
        playerHealth = 100;
        UpdateHealthbar();
        this.transform.position = new Vector3(0, 0, 0);
    }
    public void UpdateHealthbar()
    {
        healthbar.transform.localScale = new Vector3(playerHealth * 0.01f, 1, 0);
    }

}
