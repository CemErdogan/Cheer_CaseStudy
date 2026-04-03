using UnityEngine;

namespace Abstractions.FigureSystem
{
    [System.Serializable]
    public struct FigureData
    {
        [field: SerializeField] public Vector2Int GridCoord { get; private set; }
        [field: SerializeField] public bool IsSquare { get; private set; }
        [field: SerializeField] public PointData[] Points { get; private set; }

        public FigureData(Vector2Int gridCoord, bool isSquare, PointData[] points)
        {
            GridCoord = gridCoord;
            IsSquare = isSquare;
            Points = points;
        }

        public FigureData WithGridCoord(Vector2Int coord) => new FigureData(coord, IsSquare, Points);
    }
}
