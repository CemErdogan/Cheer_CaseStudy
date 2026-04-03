using System;
using UnityEngine;

namespace Abstractions.FigureSystem
{
    public interface IPointView
    {
        void Prepare(SlotPosition slot, ColorType colorType, ConnectionData slotData, bool isConnected);
        void SetConnectedVisual(ConnectionData data);
        void AnimateDestroy(Vector3 center, float duration, Action onComplete);
    }
}
