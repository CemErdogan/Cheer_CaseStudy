using UnityEngine;

namespace Abstractions.FigureSystem
{
    public interface IPointView
    {
        void Prepare(ColorType colorType, Vector3 localPosition);
    }
}