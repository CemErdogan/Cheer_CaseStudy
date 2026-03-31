using Abstractions.LevelSystem;
using UnityEngine;

namespace Game.LevelSystem.Runtime
{
    [CreateAssetMenu(fileName = "Level_", menuName = "Game/LevelSystem/Level/GameLevel", order = 0)]
    public class GameLevel : ScriptableObject, ILevel
    {
        [field:SerializeField] public GoalData[] GoalData { get; private set; }
        
        public void Load()
        {
            
        }

        public void Unload()
        {
            
        }
    }
}