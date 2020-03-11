using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Layers
{
    // Builtin layers
    public const int DEFAULT = 0;
    public const int TRANSPARENT_FX = 1;
    public const int IGNORE_RAYCAST = 2;
    public const int WATER = 4;
    public const int UI = 5;

    // User layers
    public const int PLAYER = 8;
    public const int PLAYER_WEAPONS = 9;
    public const int COLLIDERS = 10;
    public const int TRIGGERS = 11;

    public static int MaskFromLayer(int layer)
    {
        return 1 << layer;
    }
}
