using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockedBackState : PlayerState
{
    // Start is called before the first frame update
    private const float knockBackMinThreshold = 0.25f;
    public static readonly KnockedBackState INSTANCE = new KnockedBackState();

    private const float xBounce = 0.3f;
    private const float yBounce = 0;
    private Vector2 oldSpeed, nextOldSpeed;
    private Rigidbody2D body;
    private float TimeOfEnter;
    private PhysicsMaterial2D oldMaterial;
    private Transform spineContainer;
    public override string Name => "KNOCKED_BACK";
    

    public override void Enter()
    {
        base.Enter();
        TimeOfEnter = Time.time;
        controller.HasAirJumped = true;
        body = base.controller.GetComponent<Rigidbody2D>();
        controller.BodyCollider.sharedMaterial = controller.BouncyMaterial;
        controller.KnockedBack = true;
        oldSpeed = body.velocity;
        spineContainer = controller.transform.Find("SpineHolder");
        spineContainer.localScale = new Vector3(-1f, 1f, 1f);
        Debug.Log(spineContainer.name);

    }
    public override void Update()
    {
        base.Update();
        spineContainer.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(-body.velocity.x, body.velocity.y)*360/(2*Mathf.PI));
    }
    public override void FixedUpdate()
    {
        oldSpeed = nextOldSpeed;
        nextOldSpeed = body.velocity;

        //base.FixedUpdate();
        if (TimeOfEnter + controller.KnockBackDuration < Time.time || body.velocity.magnitude < knockBackMinThreshold)
        {
            if (controller.Grounded)
            {
                controller.ChangeState(IdleState.INSTANCE);
            }
            else
            {
                controller.ChangeState(AirborneState.INSTANCE);
            }
        }
        
    }
    public override void OnCollisionEnter2D(Collision2D collision)
    {
        
        base.OnCollisionEnter2D(collision);
        Vector2 col = collision.contacts[0].normal;

        //collision with ground or roof
        if (Mathf.Abs(col.y) > Mathf.Abs(col.x))
        {
            body.velocity = new Vector2(oldSpeed.x, -oldSpeed.y*yBounce);
        }
        else
        {
            body.velocity = new Vector2(-oldSpeed.x * xBounce, oldSpeed.y);
        }
    }

    public override void Exit()
    {
        spineContainer.rotation = Quaternion.identity;
        spineContainer.localScale = Vector3.one;
        controller.BodyCollider.sharedMaterial = oldMaterial;
        controller.KnockedBack = false;
        base.Exit();
    }
}
