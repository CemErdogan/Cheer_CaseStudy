using Zenject;

namespace Game.MatchSystem.Runtime
{
    public class MatchSystemInstaller : MonoInstaller<MatchSystemInstaller>
    {
        public override void InstallBindings()
        {
            Container.DeclareSignal<FigurePlacedSignal>();
            Container.DeclareSignal<FlowCompletedSignal>();
            Container.DeclareSignal<GridClearedSignal>();

            Container.Bind<MatchFinder>().FromNew().AsSingle();
            Container.Bind<DestroyAnimator>().FromNew().AsSingle();
            Container.Bind<ExpandHandler>().FromNew().AsSingle();
            Container.Bind<BigSquareHandler>().FromNew().AsSingle();
            Container.Bind<GravityHandler>().FromNew().AsSingle();

            Container.BindInterfacesAndSelfTo<FlowManager>().FromNew().AsSingle().NonLazy();
        }
    }

    // Signals
    public readonly struct FigurePlacedSignal { }
    public readonly struct FlowCompletedSignal { }
    public readonly struct GridClearedSignal { }
}
