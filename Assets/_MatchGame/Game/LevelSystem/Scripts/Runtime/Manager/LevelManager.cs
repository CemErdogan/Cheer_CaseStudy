using System;
using Abstractions.LevelSystem;
using Zenject;

namespace Game.LevelSystem.Runtime
{
    public class LevelManager : ILevelManager, IInitializable, IDisposable
    {
        [Inject] private readonly SignalBus _signalBus;
        [Inject] private readonly LevelDatabase _database;
        
        public void Initialize()
        {
            _signalBus.Subscribe<ICompleteLevelSignal>(OnLevelCompleted);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<ICompleteLevelSignal>(OnLevelCompleted);
        }
        
        private void OnLevelCompleted(ICompleteLevelSignal signal)
        {
            
        }
    }
}