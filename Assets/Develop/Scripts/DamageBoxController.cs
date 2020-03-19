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
    private float knockBackDuration = 0f;

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
        //print(collision.tag);
        if(collision.tag == "Player" || collision.tag == "CeilingDetector")
        {
            PlayerController controller = collision.GetComponentInParent<PlayerController>();
            
            if (givesKnockBack && !controller.KnockedBack && !controller.Invunerable)
            {
                controller.KnockBackDuration = knockBackDuration;
                float dir = Mathf.Sign(collision.transform.position.x - transform.position.x);
                controller.GetComponent<Rigidbody2D>().velocity = knockBackVelocity * new Vector2(dir, 1);
                controller.ChangeState(KnockedBackState.INSTANCE);
            }
            collision.GetComponentInParent<HealthController>().TakeDamage(damage);
        }
    }
}
