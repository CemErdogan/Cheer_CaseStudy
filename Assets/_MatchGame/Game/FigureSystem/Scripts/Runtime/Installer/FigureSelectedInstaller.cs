using Abstractions.FigureSystem;
using Zenject;

namespace Game.FigureSystem.Runtime
{
    public class FigureSelectedInstaller : MonoInstaller<FigureSelectedInstaller>
    {
        [Inject] private readonly FigureData _figureData;

        public override void InstallBindings()
        {
            Container.BindInstance(_figureData).AsSingle();

            Container.Bind<FigureSelected>().FromComponentOn(gameObject).AsSingle();

            Container.Bind<IFigureSelectedModel>().To<FigureSelectedModel>().FromNew().AsSingle();
            Container.Bind<IFigureSelectedView>().To<FigureSelectedView>().FromComponentOn(gameObject).AsSingle();
            Container.BindInterfacesAndSelfTo<FigureSelectedController>().FromNew().AsSingle().NonLazy();

            Container.DeclareSignal<FigureSelectedSpawnSignal>();
        }
    }
    
    public readonly struct FigureSelectedSpawnSignal
    {
        public readonly FigureData  Data;
        
        public FigureSelectedSpawnSignal(FigureData data)
        {
            Data = data;
        }
    }
}