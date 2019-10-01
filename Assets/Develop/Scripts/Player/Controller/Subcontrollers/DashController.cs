using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashController : MonoBehaviour
{
    private IDash dashRules;

    public IDash DashRules
    {
        set
        {
            dashRules = value;
        }
    }

    internal MovementStat Dash(float input)
    {
        MovementStat temp = MovementStat.DASHING;
        dashRules.HasDashed = true;
        if (Time.time - dashRules.LastDashTime <= dashRules.DashDuration)
        {
            dashRules.NewVelocity = new Vector2(dashRules.SpriteDirection * dashRules.DashSpeed * dashRules.FlipGravityScale, -input);
            dashRules.NewGravityScale = 0;
            dashRules.LastDashTime = Time.time;
        }
        else
        {
            temp = MovementStat.STANDARD;
            dashRules.NewGravityScale = dashRules.BaseGravityScale * dashRules.FlipGravityScale;
            dashRules.HorizontalVelocity = 0;
        }
        return temp;
    }
}
