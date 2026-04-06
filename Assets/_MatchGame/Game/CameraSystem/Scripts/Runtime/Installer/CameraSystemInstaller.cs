using Abstractions.CameraSystem;
using Zenject;

namespace Game.CameraSystem.Runtime
{
    public class CameraSystemInstaller : MonoInstaller<CameraSystemInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CameraManager>().FromNew().AsSingle().NonLazy();
            
            Container.DeclareSignalWithInterfaces<SetupCameraSignal>().OptionalSubscriber();
        }
    }
}