using Abstractions.SaveSystem;
using Zenject;

namespace Project.SaveSystem.Runtime
{
    public class SaveSystemInstaller : MonoInstaller<SaveSystemInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SaveManager>().FromNew().AsSingle().NonLazy();

            Container.DeclareSignalWithInterfaces<SaveSignal>().OptionalSubscriber();
        }
    }

    public readonly struct SaveSignal : ISaveSignal
    {
        public object Data { get; }
        public string Key { get; }
        
        public SaveSignal(object data, string key)
        {
            Data = data;
            Key = key;
        }
    }
}