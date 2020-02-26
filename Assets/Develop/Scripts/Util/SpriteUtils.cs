using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using GlobalEnums;
using UnityEngine;

public class SpriteUtils
{
    public static Vector2 GetLocalSize(Vector3 spriteSize, Transform spriteTransform)
    {
        Vector3 localScale = spriteTransform.localScale;
        return new Vector2(spriteSize.x * localScale.x, spriteSize.y * localScale.y);
    }

    public static Vector2 GetWorldSize(Vector3 spriteSize, Transform spriteTransform)
    {
        Vector3 worldScale = spriteTransform.lossyScale;
        return new Vector2(spriteSize.x * worldScale.x, spriteSize.y * worldScale.y);;
    }
    
    /// <summary>
    ///   <para>Returns the position of the given side of the sprite.</para>
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="spriteSide">Must be either TOP or BOTTOM.</param>
    public static Vector3 GetSidePos(SpriteRenderer sprite, SquareSide spriteSide)
    {
        Transform spriteTransform = sprite.transform;
        float spriteLocalHeight = GetLocalSize(sprite.sprite.bounds.size, spriteTransform).y;
        float positionOffset;
        switch (spriteSide)
        {
            case SquareSide.TOP:
                positionOffset = spriteLocalHeight / 2f;
                break;

            case SquareSide.BOTTOM:
                positionOffset = -spriteLocalHeight / 2f;
                break;

            default:
                throw new InvalidEnumArgumentException($"{nameof(spriteSide)} must be either TOP or BOTTOM.");
        }

        Vector3 spriteLocalPosition = spriteTransform.localPosition + positionOffset * spriteTransform.up;
        return spriteTransform.parent.TransformPoint(spriteLocalPosition);
    }

    public static Vector3 GetDistanceBetween(Transform fromTransform, SpriteRenderer toSprite, SquareSide relativeToSpriteSide)
    {
        Transform toTransform = toSprite.transform;
        float spriteWorldHeight = GetWorldSize(toSprite.sprite.bounds.size, toTransform).y;
        float distanceOffset;
        switch (relativeToSpriteSide)
        {
            case SquareSide.TOP:
                distanceOffset = spriteWorldHeight / 2f;
                break;

            case SquareSide.BOTTOM:
                distanceOffset = -spriteWorldHeight / 2f;
                break;

            default:
                throw new InvalidEnumArgumentException($"{nameof(relativeToSpriteSide)} must be either TOP or BOTTOM.");
        }

        return GetDistanceBetween(fromTransform, toTransform, distanceOffset);
    }

    public static Vector3 GetDistanceBetween(Transform fromTransform, Transform toTransform, float distanceOffset = 0f)
    {
        Vector3 distance = toTransform.position - fromTransform.position;
        Vector3 adjustedDistance = VectorUtils.ExtendVectorInDirection(distance, distance.normalized, distanceOffset);
        return adjustedDistance;
    }
}
