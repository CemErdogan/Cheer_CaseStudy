using Abstractions.GridSystem;
using Zenject;

namespace Game.GridSystem.Runtime
{
    public class CellInstaller : MonoInstaller<CellInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<ICellModel>().To<CellModel>().FromNew().AsSingle();
            Container.Bind<ICellView>().To<CellView>().FromComponentOn(gameObject).AsSingle();
            Container.BindInterfacesAndSelfTo<CellController>().FromNew().AsSingle().NonLazy();
            
            Container.DeclareSignal<CellSpawnSignal>();
        }
    }
    
    public readonly struct CellSpawnSignal
    {
        public readonly CellData Data;
        
        public CellSpawnSignal(CellData data)
        {
            Data = data;
        }
    }
}