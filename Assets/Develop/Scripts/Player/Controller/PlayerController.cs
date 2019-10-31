using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private PlayerState currentState;
    [SerializeField] private string currentStateName;

    public void ChangeState(PlayerState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentStateName = newState.Name;
        newState.Enter();
    }

    void Awake()
    {
        currentState = IdleState.INSTANCE;
        currentStateName = currentState.Name;
    }

    void Start()
    {
        WalkingState.INSTANCE.Init(this);
        IdleState.INSTANCE.Init(this);
        // TODO: adddd the rest of the states
    }

    void Update()
    {
        currentState.Update();
    }

    void FixedUpdate()
    {
        currentState.FixedUpdate();
    }
}
