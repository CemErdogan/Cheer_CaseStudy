using Abstractions.FigureSystem;
using Game.FigureSystem.Runtime;

namespace Game.MatchSystem.Runtime
{
    public class BigSquareHandler
    {
        private static readonly SlotPosition[] AllSlots =
        {
            SlotPosition.TopLeft,
            SlotPosition.TopRight,
            SlotPosition.BottomLeft,
            SlotPosition.BottomRight
        };

        public void TryMakeBigSquare(IFigure figure, Point.Factory pointFactory, ConnectionDatabase connDb)
        {
            if (figure.PointMap.Count < 2) return;

            ColorType? firstColor = null;
            foreach (var point in figure.PointMap.Values)
            {
                if (firstColor == null)
                {
                    firstColor = point.Color; continue;
                }
                if (point.Color != firstColor) return;
            }

            if (firstColor == null) return;

            foreach (var slot in AllSlots)
            {
                if (figure.TryGetPoint(slot, out _)) continue;

                var newPointData = new PointData(position:slot, color: firstColor.Value, isBigSquare: true);

                var slotData = connDb.GetData(slot);
                var newPoint = pointFactory.Create(newPointData);
                newPoint.Initialize(newPointData, figure, slotData);
                figure.AddPoint(slot, newPoint);
            }

            figure.ApplyBigSquare();
        }
    }
}
