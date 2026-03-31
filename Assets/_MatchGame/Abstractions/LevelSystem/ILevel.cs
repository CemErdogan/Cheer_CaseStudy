namespace Abstractions.LevelSystem
{
    public interface ILevel
    {
        GoalData[] GoalData { get; }
        string name { get; set; }

        void Load();
        void Unload();
    }
}