using Abstractions.FigureSystem;
using UnityEngine;

namespace Game.FigureSystem.Runtime
{
    [CreateAssetMenu(fileName = "FigureDatabase", menuName = "Game/FigureSystem/Database/FigureDatabase", order = 0)]
    public class FigureDatabase : ScriptableObject
    {
        [field:SerializeField] public GameObject FigurePrefab { get; private set; }
        [field:SerializeField] public GameObject FigureSelectedPrefab { get; private set; }

        [field: SerializeField, EditableFigure] public FigureData[] FiguresData { get; private set; }
    }
}