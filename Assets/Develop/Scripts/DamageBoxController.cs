using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DamageBoxController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private bool givesKnockBack;

    [SerializeField]
    private float force;

    [SerializeField]
    private int damage;

    Collider2D damageBoxCollider;
    void Start()
    {
        damageBoxCollider = GetComponent<Collider2D>();
        damageBoxCollider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "ColliderFullHeight" || collision.collider.tag == "ColliderCrouch")
        {
            collision.collider.GetComponentInParent<HealthController>().TakeDamage(damage);
        }
    }
}
