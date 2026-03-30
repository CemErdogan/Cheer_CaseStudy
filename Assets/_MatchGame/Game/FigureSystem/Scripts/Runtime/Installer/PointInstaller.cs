using Abstractions.FigureSystem;
using Zenject;

namespace Game.FigureSystem.Runtime
{
    public class PointInstaller : MonoInstaller<PointInstaller>
    {
        public override void InstallBindings()
        {
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