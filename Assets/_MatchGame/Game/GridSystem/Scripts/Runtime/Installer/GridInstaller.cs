using Abstractions.GridSystem;
using Zenject;

namespace Game.GridSystem.Runtime
{
    public class GridInstaller : MonoInstaller<GridInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<GridModel>().To<GridModel>().FromNew().AsSingle();
            Container.Bind<GridView>().To<GridView>().FromComponentOn(gameObject).AsSingle();
            Container.BindInterfacesAndSelfTo<GridController>().FromNew().AsSingle().NonLazy();

            Container.DeclareSignal<GridSpawnSignal>();
        }
    }

    public readonly struct GridSpawnSignal
    {
        public readonly GridData Data;

        public GridSpawnSignal(GridData data)
        {
            Data = data;
        }
    }
}