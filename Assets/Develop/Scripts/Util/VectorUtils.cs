using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtils
{
    public static Vector2 To2(this Vector3 vec)
    {
        return new Vector2(vec.x, vec.y);
    }

    public static Vector2Int Sign(this Vector2 vec)
    {
        // Do not use Unity's Mathf.Sign(), as it doesn't return 0
        return new Vector2Int(Math.Sign(vec.x), Math.Sign(vec.y));
    }

    public static bool HasEqualSignAs(this Vector2 vec1, Vector2 vec2)
    {
        return vec1.Sign() == vec2.Sign();
    }

    public static Vector2 DirectionTo(this Vector2 fromVec, Vector2 toVec)
    {
        return (toVec - fromVec).normalized;
    }

    public static Vector3 DirectionTo(this Vector3 fromVec, Vector3 toVec)
    {
        Vector3 direction = toVec - fromVec;
        direction.z = 0f; // in case the two vectors are on different Z coordinates
        return direction.normalized;
    }

    /// <summary>
    ///   <para>Returns a vector that has been extended a distance in the given direction from the base vector.</para>
    /// </summary>
    /// <param name="vec">The base vector.</param>
    /// <param name="direction">The direction to extend in. It is assumed to be a normalized vector.</param>
    /// <param name="distance"></param>
    public static Vector2 ExtendInDirection(this Vector2 vec, Vector2 direction, float distance)
    {
        Vector2 extendedVec = vec;
        extendedVec.x += direction.x * distance;
        extendedVec.y += direction.y * distance;
        return extendedVec;
    }

    /// <inheritdoc cref="ExtendInDirection(Vector2,Vector2,float)"/>
    public static Vector2 ExtendInDirection(this Vector3 vec, Vector2 direction, float distance)
    {
        return ExtendInDirection((Vector2) vec, direction, distance);
    }
}
