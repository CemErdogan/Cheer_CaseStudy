using Abstractions.GridSystem;
using UnityEngine;
using Zenject;

namespace Game.GridSystem.Runtime
{
    public class GridSystemInstaller : MonoInstaller<GridSystemInstaller>
    {
        [SerializeField] private GridDatabase database;
        
        public override void InstallBindings()
        {
            Container.BindFactory<GridData, Grid, Grid.Factory>()
                .FromSubContainerResolve()
                .ByNewContextPrefab<GridInstaller>(database.GridPrefab)
                .UnderTransformGroup("Grids");
            
            Container.BindFactory<CellData, Cell, Cell.Factory>()
                .FromSubContainerResolve()
                .ByNewContextPrefab<CellInstaller>(database.CellPrefab)
                .UnderTransformGroup("Cells");

            Container.BindInterfacesAndSelfTo<GridManager>().FromNew().AsSingle().NonLazy();
        }
    }
}