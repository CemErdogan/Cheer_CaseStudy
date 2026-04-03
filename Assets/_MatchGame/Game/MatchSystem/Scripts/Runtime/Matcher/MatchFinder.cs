using System.Collections.Generic;
using System.Linq;
using Abstractions.FigureSystem;
using Abstractions.GridSystem;

namespace Game.MatchSystem.Runtime
{
    public class MatchFinder
    {
        public List<IPoint> FindPointsToDestroy(IEnumerable<IFigure> figures, IGridManager grid)
        {
            var destroySet = new HashSet<IPoint>();

            foreach (var figure in figures)
            {
                foreach (var point in figure.PointMap.Values.ToList())
                {
                    if (point.IsBigSquare)
                        CollectBigSquareMatches(point, figure, grid, destroySet);
                    else
                        CollectRegularMatches(point, figure, grid, destroySet);
                }
            }

            return destroySet.ToList();
        }

        private void CollectRegularMatches(IPoint point, IFigure ownerFigure,
            IGridManager grid, HashSet<IPoint> destroySet)
        {
            var neighbors = NeighborUtil.GetNeighborPoints(point, ownerFigure, grid);

            foreach (var neighbor in neighbors)
            {
                if (neighbor.Color != point.Color) continue;

                destroySet.Add(point);
                destroySet.Add(neighbor);

                foreach (var p in neighbor.ParentFigure.PointMap.Values)
                {
                    if (p.Color == point.Color)
                        destroySet.Add(p);
                }

                AddConnectedPoint(point, destroySet);
                AddConnectedPoint(neighbor, destroySet);
            }
        }

        private void CollectBigSquareMatches(IPoint point, IFigure ownerFigure,
            IGridManager grid, HashSet<IPoint> destroySet)
        {
            var neighbors = NeighborUtil.GetNeighborPoints(point, ownerFigure, grid);

            foreach (var neighbor in neighbors)
            {
                if (neighbor.Color != point.Color) continue;

                foreach (var p in ownerFigure.PointMap.Values)
                {
                    if (p.Color == point.Color)
                        destroySet.Add(p);
                }

                foreach (var p in neighbor.ParentFigure.PointMap.Values)
                {
                    if (p.Color == point.Color)
                        destroySet.Add(p);
                }
            }
        }

        private void AddConnectedPoint(IPoint point, HashSet<IPoint> destroySet)
        {
            if (!point.IsConnected) return;
            if (!point.ParentFigure.TryGetPoint(point.ConnectedWith, out var connected)) return;
            if (connected.Color == point.Color)
                destroySet.Add(connected);
        }
    }
}
