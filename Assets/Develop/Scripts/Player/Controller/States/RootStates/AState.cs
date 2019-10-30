using System;
using UnityEngine;

/// <summary>
/// An abstract class used in <see cref=""/>
/// This class provides access to interfaces to access the statemachine,
/// and is the class all concrete states are based upon. The class provides many virtual methods
/// which are important i an State Machine, such as OnEntry, OnExit and CheckTriggers.
/// The class derives from monobehaviour to allow concrete states to have update and fixedupdate calls in Unity.
/// </summary>
public abstract class AState : MonoBehaviour
{

    private bool isActive;

    private string stateName;


    private IStateMachine stateMachine;
    private IPlayerModel playerModel;
    private Rigidbody2D body;

    internal IStateMachine StateMachine
    {
        get => stateMachine;
        set
        {
            if (value != null)
            {
                stateMachine = value;
            }
            else
            {
                stateMachine = value;
            }
        }
    }
    internal bool IsActive { get => isActive; set => isActive = value; }

    internal IPlayerModel PlayerModel
    {
        get => playerModel;
        set
        {
            if (value != null)
            {
                playerModel = value;
            }
        }
    }


    internal string StateName { get => stateName; set => stateName = value; }
    internal Rigidbody2D Body { get => body; set => body = value; }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Body = GameObject.Find("View").GetComponent<Rigidbody2D>();
        isActive = false; // Disable all states at start, let the statemachine.cs control who is active.
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }

    protected virtual void FixedUpdate()
    {
    }

    // An activity executed when entering the state
    internal virtual void EntryAction()
    {
        IsActive = true;
    }

    // An activity executed when exiting the state
    internal virtual void ExitAction()
    {
        IsActive = false;
    }


    // Utility method to run all trigger functions. A triggering activity that causes a transition to occur.
    // T = AState of some sort.

    // protected abstract T CheckTriggers<T>() where T : AState;

    internal abstract StateTransition GetTransition();

}