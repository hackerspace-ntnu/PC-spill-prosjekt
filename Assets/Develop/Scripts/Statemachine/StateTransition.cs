using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// An enum used by <see cref="StateTransition" to indicate what type of transition the struct is./>
/// </summary>
public enum TransitionType { Sibling, Inner, InnerEntry, No };

/// <summary>
/// A struct which represents a transition between states, with a target state and a source state.
/// </summary>
public struct StateTransition
{
    private AState source;
    private AState target;
    private TransitionType transType;
    public StateTransition(AState sourceState, AState targetState, TransitionType type)
    {
        target = targetState;
        transType = type;
        source = sourceState;
    }
    public AState Source { get => source; set => source = value; }
    public AState Target { get => target;  set => target = value; }
    public TransitionType TransType { get => transType; set => transType = value; }

}

