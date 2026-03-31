namespace Abstractions.SaveSystem
{
    public interface ISaveManager
    {
        void Save<T>(T data, string key);
        T Load<T>(string key);
    }
}