using UnityEngine;

namespace Abstractions.LevelSystem
{
    [System.Serializable]
    public struct GoalData
    {
        [field:SerializeField] public BaseLevelGoal Goal { get; private set; }
        [field:SerializeField] public int Amount { get; private set; }
    }
}