using Abstractions.FigureSystem;
using UnityEngine;

namespace Game.FigureSystem.Runtime
{
    [CreateAssetMenu(menuName = "Game/FigureSystem/Database/ConnectionDatabase", fileName = "ConnectionDatabase")]
    public class ConnectionDatabase : ScriptableObject
    {
        [SerializeField] private ConnectionData topLeft;
        [SerializeField] private ConnectionData topRight;
        [SerializeField] private ConnectionData bottomLeft;
        [SerializeField] private ConnectionData bottomRight;

        public ConnectionData GetData(SlotPosition slot)
        {
            return slot switch
            {
                SlotPosition.TopLeft     => topLeft,
                SlotPosition.TopRight    => topRight,
                SlotPosition.BottomLeft  => bottomLeft,
                SlotPosition.BottomRight => bottomRight,
                _                        => default
            };
        }
    }
}
