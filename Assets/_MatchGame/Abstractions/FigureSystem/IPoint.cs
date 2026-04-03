using System;
using UnityEngine;

namespace Abstractions.FigureSystem
{
    public interface IPoint
    {
        void Prepare(PointData data);

        SlotPosition SlotPos      { get; }
        ColorType    Color        { get; }
        bool         IsConnected  { get; }
        SlotPosition ConnectedWith{ get; }
        bool         IsBigSquare  { get; }
        IFigure      ParentFigure { get; }
        Transform    Transform    { get; }

        void Initialize(PointData data, IFigure parent, ConnectionData slotData);
        void SetConnection(SlotPosition targetSlot, ConnectionData mySlotData);
        void SetBigSquare();
        void AnimateDestroy(Vector3 center, float duration, Action onComplete);
    }
}
