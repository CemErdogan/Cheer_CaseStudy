using UnityEngine;

namespace Abstractions.FigureSystem
{
    [System.Serializable]
    public struct PointData
    {
        [field: SerializeField] public SlotPosition Position      { get; private set; }
        [field: SerializeField] public ColorType    Color         { get; private set; }
        [field: SerializeField] public bool         IsConnected   { get; private set; }
        [field: SerializeField] public SlotPosition ConnectedWith { get; private set; }
        [field: SerializeField] public bool         IsBigSquare   { get; private set; }

        public PointData(SlotPosition position, ColorType color,
                         bool isConnected = false, SlotPosition connectedWith = default,
                         bool isBigSquare = false)
        {
            Position      = position;
            Color         = color;
            IsConnected   = isConnected;
            ConnectedWith = connectedWith;
            IsBigSquare   = isBigSquare;
        }
    }
}
