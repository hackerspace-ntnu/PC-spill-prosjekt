using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TriggerType
{
    GROUND,
    WALL_LEFT,
    WALL_RIGHT,
    CEILING,
}

public class TriggerDetector : MonoBehaviour
{
    [SerializeField]
    private PlayerController controller;
    [SerializeField] private TriggerType triggerType;

    private int CeilingCount = 0;   // Keep track of number of objects above player when crouching.

    void Start()
    {
        controller = transform.parent.GetComponent<PlayerController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (triggerType)
        {
            case TriggerType.GROUND:
                controller.Grounded = true;
                break;

            case TriggerType.WALL_LEFT:
                controller.WallTrigger = 1;
                break;

            case TriggerType.WALL_RIGHT:
                controller.WallTrigger = -1;
                break;

            case TriggerType.CEILING:
                controller.CanUncrouch = false;
                break;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        switch (triggerType)
        {
            case TriggerType.GROUND:
                controller.Grounded = false;
                break;

            case TriggerType.WALL_LEFT:
            case TriggerType.WALL_RIGHT:
                controller.WallTrigger = 0;
                break;

            case TriggerType.CEILING:
                controller.CanUncrouch = true;
                break;
        }
    }
}
