using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeController : MonoBehaviour
{

    private ILife lifeRules;

    public ILife LifeRules
    {
        set
        {
            lifeRules = value;
        }
    }

    internal void DamagedState() //Fix this shit
    {
        lifeRules.HasAirJumped = false;
        lifeRules.HasDashed = false;
    }
}
