using System.Collections.Generic;
using Abstractions.FigureSystem;
using Abstractions.GridSystem;
using UnityEngine;
using Zenject;

namespace Game.GridSystem.Runtime
{
    public class GridManager : IGridManager
    {
        [Inject] private readonly Grid.Factory _gridFactory;
        [Inject] private readonly Cell.Factory _cellFactory;

        private readonly Dictionary<Vector2Int, IFigure> _figureGrid = new();

        public int ColumnCount { get; private set; }
        public int RowCount    { get; private set; }

        public void SetGridSize(int columns, int rows)
        {
            ColumnCount = columns;
            RowCount    = rows;
        }

        public void PlaceFigure(IFigure figure, Vector2Int coord)
        {
            _figureGrid[coord] = figure;
        }

        public IFigure GetFigureAt(Vector2Int coord)
        {
            _figureGrid.TryGetValue(coord, out var figure);
            return figure;
        }

        public bool IsOccupied(Vector2Int coord) => _figureGrid.TryGetValue(coord, out var fig) && fig != null;

        public void RemoveFigure(Vector2Int coord)
        {
            _figureGrid.Remove(coord);
        }

        public IEnumerable<IFigure> GetAllFigures() => _figureGrid.Values;
        public IEnumerable<Vector2Int> GetOccupiedCoords() => _figureGrid.Keys;
    }
}
