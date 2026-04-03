using System.Collections.Generic;
using Abstractions.FigureSystem;
using Abstractions.GridSystem;
using UnityEngine;

namespace Game.MatchSystem.Runtime
{
    public static class NeighborUtil
    {
        /// <summary>
        /// Returns the exact grid-adjacent neighbor Points for a given Point.
        /// Diagonal cells are NOT neighbors — only shared-edge Points count.
        /// </summary>
        public static List<IPoint> GetNeighborPoints(IPoint point, IFigure ownerFigure, IGridManager grid)
        {
            var neighbors = new List<IPoint>();
            var coord     = ownerFigure.GridCoord;

            switch (point.SlotPos)
            {
                case SlotPosition.TopLeft:
                    TryAddPoint(grid, new Vector2Int(coord.x - 1, coord.y), SlotPosition.TopRight,    neighbors);
                    TryAddPoint(grid, new Vector2Int(coord.x, coord.y - 1), SlotPosition.BottomLeft,  neighbors);
                    break;

                case SlotPosition.TopRight:
                    TryAddPoint(grid, new Vector2Int(coord.x + 1, coord.y), SlotPosition.TopLeft,     neighbors);
                    TryAddPoint(grid, new Vector2Int(coord.x, coord.y - 1), SlotPosition.BottomRight, neighbors);
                    break;

                case SlotPosition.BottomLeft:
                    TryAddPoint(grid, new Vector2Int(coord.x - 1, coord.y), SlotPosition.BottomRight, neighbors);
                    TryAddPoint(grid, new Vector2Int(coord.x, coord.y + 1), SlotPosition.TopLeft,     neighbors);
                    break;

                case SlotPosition.BottomRight:
                    TryAddPoint(grid, new Vector2Int(coord.x + 1, coord.y), SlotPosition.BottomLeft,  neighbors);
                    TryAddPoint(grid, new Vector2Int(coord.x, coord.y + 1), SlotPosition.TopRight,    neighbors);
                    break;
            }

            return neighbors;
        }

        private static void TryAddPoint(IGridManager grid, Vector2Int coord,
            SlotPosition slot, List<IPoint> results)
        {
            var figure = grid.GetFigureAt(coord);
            if (figure == null) return;
            if (figure.TryGetPoint(slot, out var point))
                results.Add(point);
        }
    }
}
