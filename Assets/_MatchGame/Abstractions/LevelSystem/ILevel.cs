namespace Abstractions.LevelSystem
{
    public interface ILevel
    {
        GoalData[] GoalData { get; }
        
        void Load();
        void Unload();
    }
}