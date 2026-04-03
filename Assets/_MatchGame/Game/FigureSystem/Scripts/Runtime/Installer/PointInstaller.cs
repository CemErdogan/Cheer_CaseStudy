using Abstractions.FigureSystem;
using Zenject;

namespace Game.FigureSystem.Runtime
{
    public class PointInstaller : MonoInstaller<PointInstaller>
    {
        [Inject] private readonly PointData _pointData;

        public override void InstallBindings()
        {
            Container.BindInstance(_pointData).AsSingle();

            Container.Bind<Point>().FromComponentOn(gameObject).AsSingle();

            Container.Bind<IPointModel>().To<PointModel>().FromNew().AsSingle();
            Container.Bind<IPointView>().To<PointView>().FromComponentOn(gameObject).AsSingle();
            Container.BindInterfacesAndSelfTo<PointController>().FromNew().AsSingle().NonLazy();

            Container.DeclareSignal<PointSpawnSignal>();
        }
    }
    
    public readonly struct PointSpawnSignal
    {
        public readonly PointData  Data;
        
        public PointSpawnSignal(PointData data)
        {
            Data = data;
        }
    }
}