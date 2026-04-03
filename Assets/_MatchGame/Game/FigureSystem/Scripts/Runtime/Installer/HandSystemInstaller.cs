using UnityEngine;
using Zenject;

namespace Game.FigureSystem.Runtime
{
    public class HandSystemInstaller : MonoInstaller<HandSystemInstaller>
    {
        [SerializeField] private TrayConfig trayConfig;
        [SerializeField] private FigureDatabase figureDatabase;

        public override void InstallBindings()
        {
            Container.BindInstance(trayConfig).AsSingle();
            Container.BindInstance(figureDatabase.FiguresData).AsSingle();
            Container.Bind<Camera>().FromInstance(Camera.main).AsSingle();
            Container.BindInterfacesAndSelfTo<TrayManager>().FromNew().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<DragManager>().FromNew().AsSingle().NonLazy();
        }
    }
}
