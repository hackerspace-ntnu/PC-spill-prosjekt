using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    // Start is called before the first frame update

    public HealthSystem hpSys;
    public string damageTag; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == damageTag)
        {
            hpSys.takeDamage();
        }
    }
}
