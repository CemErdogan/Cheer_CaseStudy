using System.Collections.Generic;
using UnityEngine;

namespace Abstractions.FigureSystem
{
    public interface IFigure
    {
        Vector2Int GridCoord  { get; set; }
        bool IsBigSquare{ get; }
        IReadOnlyDictionary<SlotPosition, IPoint> PointMap{ get; }
        Transform Transform { get; }
        GameObject GameObject { get; }

        void Prepare(FigureData data);
        bool  TryGetPoint(SlotPosition slot, out IPoint point);
        void  AddPoint(SlotPosition slot, IPoint point);
        void  RemovePoint(SlotPosition slot);
        bool  IsEmpty();
        void  ApplyBigSquare();
    }
}
