using System;
using System.Collections;
using System.Collections.Generic;
using GlobalEnums;
using UnityEngine;

public enum TriggerType
{
    GROUND,
    WALL_LEFT,
    WALL_RIGHT,
    CEILING,
    GLITCH_CEILING,
}

public class TriggerDetector : MonoBehaviour
{
#pragma warning disable 649 // disable unassigned field warning
    [SerializeField] private PlayerController controller;
    [SerializeField] private TriggerType triggerType;
#pragma warning restore 649

    private int ceilingCount = 0; // Keeps track of number of objects above player when crouching
    private int glitchCeilingCount = 0; // Keeps track of number of objects above player when glitch crouching

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (triggerType)
        {
            case TriggerType.GROUND:
                controller.Grounded = true;
                break;

            case TriggerType.WALL_LEFT:
                controller.WallTrigger = WallTrigger.LEFT;
                break;

            case TriggerType.WALL_RIGHT:
                controller.WallTrigger = WallTrigger.RIGHT;
                break;

            case TriggerType.CEILING:
                controller.CanUncrouch = false;
                ceilingCount++;
                break;
            case TriggerType.GLITCH_CEILING:
                controller.CanUnglitch = false;
                glitchCeilingCount++;
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
                controller.WallTrigger = WallTrigger.NONE;
                break;

            case TriggerType.CEILING:
                ceilingCount--;
                if (ceilingCount <= 0)
                {
                    controller.CanUncrouch = true;
                }

                break;
            case TriggerType.GLITCH_CEILING:
                glitchCeilingCount--;
                if (glitchCeilingCount <= 0)
                {
                    controller.CanUnglitch = true;
                }

                break;
        }
    }
}
