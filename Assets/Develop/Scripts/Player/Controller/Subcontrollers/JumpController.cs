using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController : MonoBehaviour
{
    private IJump jumpRules;

    public IJump JumpRules
    {

        set
        {
            jumpRules = value;
        }
    }

    internal void Jumpstate()
    {
        // TODO: Move Jumpstate code from playercontroller here. 
    }
    internal void Jump(float input, Rigidbody2D body)
    {
        jumpRules.VerticalVelocity = (jumpRules.JumpSpeed * input + Mathf.Abs(body.velocity.y)) * jumpRules.FlipGravityScale;
        jumpRules.IsVelocityDirty = true;
        jumpRules.JumpTime = Time.time;
    }

}
