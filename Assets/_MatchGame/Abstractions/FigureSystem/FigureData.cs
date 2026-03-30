using UnityEngine;

namespace Abstractions.FigureSystem
{
    [System.Serializable]
    public struct FigureData
    {
        [field:SerializeField] public Vector2 Size { get; private set; }
        [field:SerializeField] public PointData[]  Points { get; private set; }
    }
}