using UnityEngine;

namespace Common.Util
{
    public static class PositionUtil
    {
        public static Vector3 ToPosition(this Vector2Int coord) => new(coord.x, coord.y, 0);
    }
}