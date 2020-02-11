using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorUtils
{
    public static Vector3 GetDirectionToVector(Vector3 fromVector, Vector3 toVector)
    {
        Vector3 direction = toVector - fromVector;
        direction.z = 0; // because z is not used
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
