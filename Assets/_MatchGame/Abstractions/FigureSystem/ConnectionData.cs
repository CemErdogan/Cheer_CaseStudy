using UnityEngine;

namespace Abstractions.FigureSystem
{
    [System.Serializable]
    public struct ConnectionData
    {
        [field: SerializeField] public Vector3 LocalPosition { get; private set; }
        [field: SerializeField] public Vector3 LocalScale { get; private set; }
        [field: SerializeField] public Vector3 PositionOffset { get; private set; }
        [field: SerializeField] public Vector3 ScaleAdjustment { get; private set; }
    }
}
