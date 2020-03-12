using System;

namespace GlobalEnums
{
    public enum Direction
    {
        LEFT = -1,
        RIGHT = 1,
    }

    public enum WallTrigger
    {
        NONE = 0,
        LEFT = 1,
        RIGHT = -1,
    }

    public enum SquareEdge
    {
        TOP,
        BOTTOM,
        LEFT,
        RIGHT,
    }

    public static class DirectionUtils
    {
        public static Direction Parse(int i)
        {
            switch (i)
            {
                case (int)Direction.LEFT:
                    return Direction.LEFT;

                case (int)Direction.RIGHT:
                    return Direction.RIGHT;

                default:
                    throw new ArgumentException($"Value must be either {(int)Direction.LEFT} or {(int)Direction.RIGHT}.", nameof(i));
            }
        }
    }
}
