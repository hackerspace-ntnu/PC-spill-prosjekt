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
}

public class TriggerDetector : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    [SerializeField] private TriggerType triggerType;

    private int CeilingCount = 0;   // Keep track of number of objects above player when crouching.

    void OnTriggerEnter2D(Collider2D collision)
    {
        switch (triggerType)
        {
            case TriggerType.GROUND:
                if (collision.tag == "Standard")
                    controller.Grounded = true;
                break;

            case TriggerType.WALL_LEFT:
                if (collision.tag == "Standard")
                    controller.WallTrigger = WallTrigger.LEFT;
                break;

            case TriggerType.WALL_RIGHT:
                if (collision.tag == "Standard")
                    controller.WallTrigger = WallTrigger.RIGHT;
                break;

            case TriggerType.CEILING:
                controller.CanUncrouch = false;
                break;
        }
    }
    
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Standard")
        {
            if (this.gameObject.name == "Wall Trigger Left")
                controller.WallTrigger = WallTrigger.LEFT;
            else if (this.gameObject.name == "Wall Trigger Right")
                controller.WallTrigger = WallTrigger.RIGHT;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        switch (triggerType)
        {
            case TriggerType.GROUND:
                if (collision.tag == "Standard")
                    controller.Grounded = false;
                break;

            case TriggerType.WALL_LEFT:
            case TriggerType.WALL_RIGHT:
                if (collision.tag == "Standard")
                    controller.WallTrigger = WallTrigger.NONE;
                break;

            case TriggerType.CEILING:
                controller.CanUncrouch = true;
                break;
        }
    }
}
