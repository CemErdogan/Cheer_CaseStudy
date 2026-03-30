using UnityEngine;
using Zenject;

namespace Game.FigureSystem.Runtime
{
    public class FigureSystemInstaller : MonoInstaller<FigureSystemInstaller>
    {
        [SerializeField] private FigureDatabase figureDatabase;
        
        public override void InstallBindings()
        {
            Container.BindFactory<Figure, Figure.Factory>()
                .FromSubContainerResolve()
                .ByNewContextPrefab(figureDatabase.FigurePrefab)
                .UnderTransformGroup("Figures");

            Container.BindInterfacesAndSelfTo<FigureManager>().AsSingle();
        }
    }
}