using Abstractions.FigureSystem;
using UnityEngine;
using Zenject;

namespace Game.FigureSystem.Runtime
{
    public class FigureSystemInstaller : MonoInstaller<FigureSystemInstaller>
    {
        [SerializeField] private FigureDatabase database;
        [SerializeField] private ConnectionDatabase connectionDatabase;
        
        public override void InstallBindings()
        {
            Container.Bind<FigureDatabase>().FromInstance(database).AsSingle();
            Container.Bind<ConnectionDatabase>().FromInstance(connectionDatabase).AsSingle();

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

            Container.Bind<IFigureFactory>().To<Figure.Factory>().FromResolve();

            Container.BindInterfacesAndSelfTo<FigureManager>().FromNew().AsSingle().NonLazy();
        }
    }
}