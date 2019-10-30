using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OnGrapplingHookState : AActionState
{

    // used to calculate input. Always set to 0 at entry and exit functions.
    private Vector2 mousePos;
    private bool haveShotGrapplingHook = false;
    public GameObject grapplingAnchor;


    public Vector2 MousePos { get => mousePos; set => mousePos = value; }
    public bool HaveShotGrapplingHook { get => haveShotGrapplingHook; set => haveShotGrapplingHook = value; }

    protected override AActionState CheckTriggers()
    {
        // TODO : Tweaking.
        return StateMachine.OnNoActionState;
    }


    protected override void Start()
    {
        Body = GameObject.Find("View").GetComponent<Rigidbody2D>();
        StateName = "- Using grappling hook -";
        IsActive = false;
    }

    protected override void Update()
    {
        if (IsActive)
        {
            // check if any other states can be transitioned into
            this.TargetTransitionState = CheckTriggers();

            if(this.TargetTransitionState == null || this.TargetTransitionState == this)
            {
                if (HaveShotGrapplingHook) {
                // TODO : Add logic after grappling hook have been fired to a position.
                }
            }
        }
    }

    internal override void EntryAction()
    {
        IsActive = true;
    }

    internal override void ExitAction()
    {
        this.TargetTransitionState = null;
        IsActive = false;
    }


    internal override StateTransition GetTransition()
    {
        if (this.TargetTransitionState == this || this.TargetTransitionState == null)
        {
            return new StateTransition(null, null, TransitionType.No);
        }
        else
        {
            return new StateTransition(this, TargetTransitionState, TransitionType.Sibling);
        }
    }

    private bool ShootRayCastAtMousePosition()
    {
        Vector2 playerPos = GameObject.Find("Player").transform.position;
        Vector2 mousePos = Input.mousePosition;
        RaycastHit2D hit = Physics2D.Raycast(playerPos, mousePos, 20f);
        if(hit.collider != null) // TODO : Add check for tags that include only levels ( levels need to be updated first)
        {
            // The ray cast hit something within the hard-coded range (20f), and can now shoot the hook. Player cannot move while this happens.
            Body.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

            return true;
        }
        else
        {
            return false;
        }
    }

    protected override void FixedUpdate()
    {
        if (IsActive)
        {
            if (!HaveShotGrapplingHook)
            {
                bool canShootHook = ShootRayCastAtMousePosition();
                if (canShootHook)
                {

                }
                else
                {
                    return;
                }
            }
        }

    }
}
