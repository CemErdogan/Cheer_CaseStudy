using UnityEngine;

namespace Game.FigureSystem.Runtime
{
    [CreateAssetMenu(fileName = "TrayConfig", menuName = "Game/FigureSystem/Config/TrayConfig", order = 0)]
    public class TrayConfig : ScriptableObject
    {
        [field: SerializeField] public Vector3[] TrayPositions { get; private set; }
        
        [field: SerializeField] public Vector3 SelectionPosition { get; private set; }
    }
}
