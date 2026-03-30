using UnityEngine;

namespace Game.GridSystem.Runtime
{
    [CreateAssetMenu(fileName = "GridDatabase", menuName = "Game/GridSystem/Database/GridDatabase", order = 0)]
    public class GridDatabase : ScriptableObject
    {
        [field:SerializeField] public GameObject GridPrefab { get; private set; }
        [field:SerializeField] public GameObject CellPrefab { get; private set; }
    }
}