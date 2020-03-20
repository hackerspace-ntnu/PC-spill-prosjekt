using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class HealthController : MonoBehaviour
{
    private PlayerController controller;
    private MeshRenderer rend;
    private Material oldMaterial;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponentInChildren<MeshRenderer>();
        controller = GetComponent<PlayerController>();
    }

    // Update is called once per frame

    public void TakeDamage(int damage)
    {
        if (controller.Invunerable) { return; }
        if (damage <= 0) { return; }
        if(damage >= controller.CurrentHealth)
        {
            controller.CurrentHealth = 0;
            die();     
        }
        else
        {
            controller.CurrentHealth = controller.CurrentHealth - damage;
        }
        StartCoroutine(waitForInvunerability());
    }
    public void die()
    {
        print("Death, too bad");
        //Her skjer ting
    }

    public IEnumerator waitForInvunerability()
    {
        print("Hei");
        oldMaterial = rend.material;
        rend.material = controller.GlitchMaterial;
        controller.Invunerable = true;
        yield return new WaitForSeconds(controller.InvunerabilityTime);
        controller.Invunerable = false;
        rend.material = oldMaterial;
    }
}
