using Abstractions.FigureSystem;
using UnityEngine;
using Zenject;

namespace Game.FigureSystem.Runtime
{
    public class FigureSystemInstaller : MonoInstaller<FigureSystemInstaller>
    {
        [SerializeField] private FigureDatabase figureDatabase;
        
        public override void InstallBindings()
        {
            Container.BindFactory<FigureData, Figure, Figure.Factory>()
                .FromSubContainerResolve()
                .ByNewContextPrefab<FigureInstaller>(figureDatabase.FigurePrefab)
                .UnderTransformGroup("Figures");

            Container.BindFactory<FigureData, FigureSelected, FigureSelected.Factory>()
                .FromSubContainerResolve()
                .ByNewContextPrefab<FigureSelectedInstaller>(figureDatabase.FigurePrefab)
                .UnderTransformGroup("FigureSelected");

            Container.BindInterfacesAndSelfTo<FigureManager>().FromNew().AsSingle();
        }
    }
}