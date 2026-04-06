using Abstractions.CameraSystem;
using Abstractions.LevelSystem;
using UnityEngine;
using Zenject;

namespace Game.LevelSystem.Runtime
{
    public class LevelSystemInstaller : MonoInstaller<LevelSystemInstaller>
    {
        [SerializeField] private LevelDatabase database;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LevelManager>().FromNew().AsSingle().NonLazy();
            
            Container.Bind<LevelDatabase>().FromInstance(database).AsSingle();
            foreach (var lvl in database.Levels)
            {
                Container.QueueForInject(lvl);
            }

            Container.DeclareSignalWithInterfaces<CompleteLevelSignalSignal>().OptionalSubscriber();
        }
    }
    
    public readonly struct CompleteLevelSignalSignal : ICompleteLevelSignal
    {
        public CompleteType CompleteType { get; }
        
        public CompleteLevelSignalSignal(CompleteType completeType)
        {
            CompleteType = completeType;
        }
    }
}