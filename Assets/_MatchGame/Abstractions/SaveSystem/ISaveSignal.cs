namespace Abstractions.SaveSystem
{
    public interface ISaveSignal
    {
        object Data { get; }
        string Key { get; }
    }
}