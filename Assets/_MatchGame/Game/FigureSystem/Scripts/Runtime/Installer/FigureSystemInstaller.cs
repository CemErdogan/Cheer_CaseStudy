using Abstractions.FigureSystem;
using UnityEngine;
using Zenject;

namespace Game.FigureSystem.Runtime
{
    public class FigureSystemInstaller : MonoInstaller<FigureSystemInstaller>
    {
        [SerializeField] private FigureDatabase database;
        
        public override void InstallBindings()
        {
            Container.BindFactory<FigureData, Figure, Figure.Factory>()
                .FromSubContainerResolve()
                .ByNewContextPrefab<FigureInstaller>(database.FigurePrefab)
                .UnderTransformGroup("Figures");

            Container.BindFactory<FigureData, FigureSelected, FigureSelected.Factory>()
                .FromSubContainerResolve()
                .ByNewContextPrefab<FigureSelectedInstaller>(database.FigureSelectedPrefab)
                .UnderTransformGroup("FigureSelected");
            
            Container.BindFactory<PointData, Point, Point.Factory>()
                .FromSubContainerResolve()
                .ByNewContextPrefab<PointInstaller>(database.PointPrefab)
                .UnderTransformGroup("Points");

            Container.BindInterfacesAndSelfTo<FigureManager>().FromNew().AsSingle().NonLazy();
        }
    }
}