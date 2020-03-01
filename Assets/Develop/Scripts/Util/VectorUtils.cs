using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorUtils
{
    public static Vector2 To2(Vector3 vec)
    {
        return new Vector2(vec.x, vec.y);
    }

    public static Vector2Int Sign(Vector2 vec)
    {
        // Do not use Unity's Mathf.Sign(), as it doesn't return 0
        return new Vector2Int(Math.Sign(vec.x), Math.Sign(vec.y));
    }

    public static bool HaveDifferentSigns(Vector2 vec1, Vector2 vec2)
    {
        return Sign(vec1) != Sign(vec2);
    }

    public static Vector3 GetDirectionToVector(Vector3 fromVec, Vector3 toVec)
    {
        Vector3 direction = toVec - fromVec;
        direction.z = 0f; // in case the two vectors are on different Z coordinates
        direction.Normalize();
        return direction;
    }

    /// <summary>
    ///   <para>Returns a vector that has been extended a distance in the given direction from the base vector.</para>
    /// </summary>
    /// <param name="vec">The base vector.</param>
    /// <param name="direction">The direction to extend in. Is assumed to be a normalized vector.</param>
    /// <param name="distance"></param>
    public static Vector3 ExtendVectorInDirection(Vector3 vec, Vector3 direction, float distance)
    {
        Vector3 extendedVec = vec;
        extendedVec.x += direction.x * distance;
        extendedVec.y += direction.y * distance;
        return extendedVec;
    }
}
