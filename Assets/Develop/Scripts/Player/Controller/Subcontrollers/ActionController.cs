using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    private IAction actionRules;

    public IAction ActionRules
    {
        set
        {
            actionRules = value;
        }
    }
}
