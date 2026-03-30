using Abstractions.FigureSystem;
using Zenject;

namespace Game.FigureSystem.Runtime
{
    public class FigureInstaller : MonoInstaller<FigureInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IFigureModel>().To<FigureModel>().FromNew().AsSingle();
            Container.Bind<IFigureView>().To<FigureView>().FromComponentOn(gameObject).AsSingle();
            Container.BindInterfacesAndSelfTo<FigureController>().FromNew().AsSingle().NonLazy();
            
            Container.DeclareSignal<FigureSpawnSignal>();
        }
    }

    public readonly struct FigureSpawnSignal
    {
        public readonly FigureData  Data;
        
        public FigureSpawnSignal(FigureData data)
        {
            Data = data;
        }
    }
}