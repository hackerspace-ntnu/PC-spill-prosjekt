using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum TransitionType { Sibling, Inner, InnerEntry, No };
public struct Transition<T> where T:BaseState
{
    private T source;
    private T target;
    private TransitionType transType;
    public Transition(T sourceState, T targetState, TransitionType type)
    {
        target = targetState;
        transType = type;
        source = sourceState;
    }
    public T Source { get => source; set => source = value; }
    public T Target { get => target;  set => target = value; }
    public TransitionType TransType { get => transType; set => transType = value; }
}

