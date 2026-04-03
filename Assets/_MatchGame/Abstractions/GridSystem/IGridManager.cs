using System.Collections.Generic;
using Abstractions.FigureSystem;
using UnityEngine;

namespace Abstractions.GridSystem
{
    public interface IGridManager
    {
        int ColumnCount { get; }
        int RowCount { get; }

        void SetGridSize(int columns, int rows);
        void PlaceFigure(IFigure figure, Vector2Int coord);
        IFigure GetFigureAt(Vector2Int coord);
        bool IsOccupied(Vector2Int coord);
        void RemoveFigure(Vector2Int coord);
        IEnumerable<IFigure> GetAllFigures();
        IEnumerable<Vector2Int> GetOccupiedCoords();
    }
}
