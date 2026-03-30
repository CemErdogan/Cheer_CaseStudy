using UnityEngine;

namespace Abstractions.FigureSystem
{
    [System.Serializable]
    public struct PointData
    {
        [field:SerializeField] public bool IsActive { get; private set; }
        [field:SerializeField] public Vector2Int Coord { get; private set; }
        [field:SerializeField] public ColorType ColorType { get; private set; }
    }
}