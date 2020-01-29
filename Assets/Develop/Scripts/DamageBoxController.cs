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
    private Vector2 knockBackVelocity;

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
    private void OnTriggerStay2D(Collider2D collision)
    {
        print(collision.tag);
        if(collision.tag == "ColliderFullHeigth" || collision.tag == "ColliderCrouch")
        {
           
            collision.GetComponentInParent<HealthController>().TakeDamage(damage);
            if (givesKnockBack)
            {
                PlayerController controller = collision.GetComponentInParent<PlayerController>();
                float dir = Mathf.Sign(collision.transform.position.x - transform.position.x);
                controller.GetComponent<Rigidbody2D>().velocity = knockBackVelocity * new Vector2(dir, 1);
                controller.ChangeState(KnockedBackState.INSTANCE);
            }
        }
    }
}
