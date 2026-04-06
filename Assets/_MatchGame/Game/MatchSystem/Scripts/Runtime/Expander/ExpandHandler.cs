using System.Collections.Generic;
using Abstractions.FigureSystem;
using Game.FigureSystem.Runtime;

namespace Game.MatchSystem.Runtime
{
    public class ExpandHandler
    {
        private static readonly SlotPosition[] AllSlots =
        {
            SlotPosition.TopLeft,
            SlotPosition.TopRight,
            SlotPosition.BottomLeft,
            SlotPosition.BottomRight
        };

        private static readonly Dictionary<SlotPosition, SlotPosition> VerticalPartner = new()
        {
            { SlotPosition.TopLeft, SlotPosition.BottomLeft },
            { SlotPosition.BottomLeft, SlotPosition.TopLeft },
            { SlotPosition.TopRight, SlotPosition.BottomRight },
            { SlotPosition.BottomRight, SlotPosition.TopRight },
        };

        private static readonly Dictionary<SlotPosition, SlotPosition> HorizontalPartner = new()
        {
            { SlotPosition.TopLeft, SlotPosition.TopRight },
            { SlotPosition.TopRight, SlotPosition.TopLeft },
            { SlotPosition.BottomLeft, SlotPosition.BottomRight },
            { SlotPosition.BottomRight, SlotPosition.BottomLeft },
        };

        public void CheckExpand(IFigure figure, Point.Factory pointFactory, ConnectionDatabase connDb)
        {
            foreach (var slot in AllSlots)
            {
                if (figure.TryGetPoint(slot, out _)) continue;

                if (!TryExpandVertically(figure, slot, pointFactory, connDb))
                    TryExpandHorizontally(figure, slot, pointFactory, connDb);
            }
        }

        private bool TryExpandVertically(IFigure figure, SlotPosition emptySlot, Point.Factory pointFactory, ConnectionDatabase connDb)
        {
            return TryCloneAndConnect(figure, emptySlot, VerticalPartner[emptySlot], pointFactory, connDb);
        }

        private bool TryExpandHorizontally(IFigure figure, SlotPosition emptySlot, Point.Factory pointFactory, ConnectionDatabase connDb)
        {
            return TryCloneAndConnect(figure, emptySlot, HorizontalPartner[emptySlot], pointFactory, connDb);
        }

        private bool TryCloneAndConnect(IFigure figure, SlotPosition targetSlot, SlotPosition sourceSlot, Point.Factory pointFactory, ConnectionDatabase connDb)
        {
            if (!figure.TryGetPoint(sourceSlot, out var sourcePoint)) return false;
            if (sourcePoint.IsConnected) return false;

            var newPointData = new PointData(
                position: targetSlot,
                color:    sourcePoint.Color);

            var targetSlotData = connDb.GetData(targetSlot);
            var newPoint       = pointFactory.Create(newPointData);
            newPoint.Initialize(newPointData, figure, targetSlotData);
            figure.AddPoint(targetSlot, newPoint);

            newPoint.SetConnection(sourceSlot, targetSlotData);

            var sourceSlotData = connDb.GetData(sourceSlot);
            sourcePoint.SetConnection(targetSlot, sourceSlotData);

            return true;
        }
    }
}
