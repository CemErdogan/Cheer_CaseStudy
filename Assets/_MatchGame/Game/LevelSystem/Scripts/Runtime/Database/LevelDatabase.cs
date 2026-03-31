using UnityEngine;

namespace Game.LevelSystem.Runtime
{
    [CreateAssetMenu(fileName = "LevelDatabase", menuName = "Game/LevelSystem/Database/LevelDatabase", order = 0)]
    public class LevelDatabase : ScriptableObject
    {
        [field:SerializeField] public GameLevel[] Levels { get; private set; }
    }
}