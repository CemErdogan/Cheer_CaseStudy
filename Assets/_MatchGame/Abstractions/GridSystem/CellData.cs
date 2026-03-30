using UnityEngine;

namespace Abstractions.GridSystem
{
    public struct CellData
    {
        [field:SerializeField] public Vector2Int Coord { get; private set; }
        
        public CellData(Vector2Int coord)
        {
            Coord = coord;
        }
    }
}